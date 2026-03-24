using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;


public class character : MonoBehaviour
{
    public GameObject armPrefab;
    public float damage;
    Rigidbody rb;
    Animator animator;
    public float speed;
    public float jumpHeight;
    public bool armed;
    public Transform kol1,kol2;
    public Transform cam;
    float xRotation,yRotation;
    public bool walkable;
    mouseController mC;
    public float charge;
    public bool look;
    public float health;
    float maxHealth;
    bool ded;
    bool damagable = true;
    float healthSecond;
    public SkinnedMeshRenderer[] mrs;
    public MeshRenderer[] mrs2;
    public Material hitMaterial;
    public Transform lookTarget;
    float maxStamina;
    public bool follow;

    public Image bar1, bar2;
    void Start()
    {
        maxStamina = charge;
        charge = 0;
        maxHealth = health;
        mC = GetComponent<mouseController>();
        rb = GetComponent<Rigidbody>();
        animator = transform.Find("armature").GetComponent<Animator>();
        animator.SetBool("armed", armed);
        animator.SetTrigger("change");
    }
    void Update()
    {
        move();
        arm();
        hit();
        healthSys();
    }
    void healthSys()
    {
        bar1.fillAmount = (maxStamina-charge) / maxStamina;
        bar2.fillAmount = health / maxHealth;



        if (health <= 0 && !ded)
        {
            Camera.main.transform.parent.GetComponent<Animator>().SetBool("death",true);
            ded = true;
            myMath.waitAndStart(0.5f, () =>
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            });
        }
        if (health < maxHealth && healthSecond > 5)
        {
            health += Time.deltaTime * 10;
        }
        if (healthSecond < 6)
        {
            healthSecond += Time.deltaTime;
        }
    }
    void hitEffect()
    {
        foreach (SkinnedMeshRenderer meshRenderer in mrs)
        {
            Material[] saveMats = meshRenderer.materials;
            Material[] myEfMats = new Material[saveMats.Length];
            for (int i = 0; i < saveMats.Length; i++)
            {
                myEfMats[i] = hitMaterial;
            }
            meshRenderer.materials = myEfMats;
            myMath.waitAndStart(.2f, () => { meshRenderer.materials = saveMats; });
        }
        foreach (MeshRenderer meshRenderer in mrs2)
        {
            Material[] saveMats = meshRenderer.materials;
            Material[] myEfMats = new Material[saveMats.Length];
            for (int i = 0; i < saveMats.Length; i++)
            {
                myEfMats[i] = hitMaterial;
            }
            meshRenderer.materials = myEfMats;
            myMath.waitAndStart(.2f, () => { meshRenderer.materials = saveMats; });
        }
    }
    void move()
    {
        if (follow)
        {
            cam.position = transform.position;
        }
        Vector3 Inputs = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        Vector3 velo = Quaternion.Euler(0, cam.localEulerAngles.y, 0) * Inputs * speed;
        Vector3 addVelo = Vector3.zero;
        if (look && armed)
        {
            Vector3 direction = (lookTarget.position - kol1.position).normalized;
            addVelo = direction * speed /2;
            kol1.rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(-90, 0, 0);
            kol2.localRotation = Quaternion.identity;
        }

        animator.SetBool("walk", Inputs != Vector3.zero);
        if (walkable)
        {
            rb.velocity = new Vector3(velo.x, 0, velo.z) + addVelo;
            float yrot = Mathf.Atan2(velo.x, velo.z) * Mathf.Rad2Deg;
            if (Inputs != Vector3.zero)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, yrot, 0), Time.deltaTime * 10);
            }
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
        walkable = hitable && !ded;
    }

    void arm()
    {
        animator.SetBool("armed", armed);
        if (charge > 0)
        {
            charge -= Time.deltaTime;
        }
        if (charge > maxStamina)
        {
            getArm();
            charge = 0; 
        }

    }
    public void getArm()
    {
        armed = !armed;
        animator.SetBool("armed", armed);
        animator.SetTrigger("change");
        if (!armed)
        {
            GameObject go = Instantiate(armPrefab);
            go.transform.position = transform.position;
            go.transform.rotation = transform.rotation * Quaternion.Euler(0,-90,0);
            go.GetComponent<freeArm>().enabled = false;
            go.GetComponent<Rigidbody>().velocity = (go.transform.forward * 10) + (go.transform.up*8);
            myMath.waitAndStart(2, () => { go.GetComponent<freeArm>().enabled = true; });
        }
    }
    int combo;
    float hitSecond;
    bool hitable = true;
    float hitWait = 0.6f;
    float s;
    int maxCombo = 2;
    void hit()
    {

        if (Input.GetMouseButtonDown(0) && hitable && armed)
        {
            charge += 3;
            transform.LookAt(new Vector3(mC.redDot.position.x,0, mC.redDot.position.z));
            animator.SetTrigger($"hit{combo}");
            if (combo == 0 || combo == 1)
            {
                soundSystem.play("hit1");
            }
            else
            {
                soundSystem.play("hit2");
            }
            hitable = false;
            hitSecond = 1.2f;
            combo++;
            if (combo > maxCombo)
            {
                combo = 0;
            }
            rb.velocity = Vector3.zero;
            rb.velocity = transform.forward * 5;
        }
        if (combo != 0)
        {
            hitSecond -= Time.deltaTime;
            if (hitSecond < 0)
            {
                combo = 0;
                hitSecond = 1.2f;
            }
        }
        if (!hitable)
        {
            s += Time.deltaTime;
            if (s > hitWait)
            {
                s = 0;
                hitable = true;
            }
        }

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "enemyHit" && other.enabled)
        {
            hit(other.GetComponent<enemyHit>().damage);
        }
    }

    public void hit(float damage)
    {
        if (damagable)
        {
            soundSystem.play("damage");
            healthSecond = 0;
            damagable = false;
            health -= damage;
            hitEffect();
            Camera.main.transform.parent.GetComponent<Animator>().SetTrigger("hit");
            myMath.waitAndStart(1,()=> { damagable = true; });
        }
    }
}

