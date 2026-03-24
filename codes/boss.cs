using UnityEngine;

public class boss : MonoBehaviour
{
    public bool faz;
    public LayerMask groundLayer;
    public float speed,dashSpeed;
    Rigidbody rb;
    Animator animator;
    bool walkable = true,hitable = true;
    Transform ch;
    public float hitSec;
    float maxHitSec;
    bool charging;
    float s1;
    bool change;
    bool dashing;
    public MeshRenderer[] mrs;
    public Material hitMaterial;
    public float health;
    bool wait = true;
    public GameObject particl;
    public GameObject kol;
    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        ch = FindAnyObjectByType<character>().transform;
    }

    public void death()
    {
        if (!faz)
        {
            GameObject go = Instantiate(kol,transform.position + Vector3.up,transform.rotation);
            myMath.waitAndStart(1, () => go.GetComponent<freeArm>().enabled = true);
            Instantiate(particl,transform.position,transform.rotation);
            Destroy(gameObject);
        }
        else
        {
            GameObject go = Instantiate(kol, transform.position + Vector3.up, transform.rotation);
            Instantiate(particl, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
    public void Update()
    {
        if (health <= 0)
        {
            death();
        }
        walkable = hitable && wait;
        float ds = Vector3.Distance(transform.position,ch.position);
        Vector3 velo = (ch.position - transform.position).normalized;
        if (walkable)
        {
            if (ds < 15)
            {
               

                if (ds < 4)
                {
                    wait = false;
                    animator.SetBool("walk", false);
                    change = !change;
                    if (!faz)
                    {
                        animator.SetTrigger("hit"+ (change ? "0" : "1"));
                    }
                    else
                    {
                        animator.SetTrigger("hit1");
                    }
                    myMath.waitAndStart(1, () => { wait = true; });
                }
                else
                {
                    rb.velocity = new Vector3(velo.x * speed, rb.velocity.y, velo.z * speed);
                    transform.LookAt(new Vector3(ch.position.x,transform.position.y, ch.position.z));
                    animator.SetBool("walk", true);
                }
            }
            else
            {
                animator.SetBool("walk", false);
                wait = false;
                charging = true;
            }
        }

        if (charging)
        {
            s1 += Time.deltaTime;
            transform.LookAt(new Vector3(ch.position.x, transform.position.y, ch.position.z));
            if (s1 >2)
            {
                s1 = 0;
                charging = false;
                dashing = true;
            }
        }

        if (dashing)
        {
            rb.velocity = transform.forward * dashSpeed;
            RaycastHit hit;
            if (Physics.OverlapSphere(transform.position + transform.forward + transform.up, 1f,groundLayer).Length > 0)
            {
                dashing = false;
                animator.SetBool("stunned", true);
                this.hit(50);
                soundSystem.play("crush");
                Instantiate(particl, transform.position + transform.forward + transform.up, Quaternion.identity);
                Camera.main.transform.parent.GetComponent<Animator>().SetTrigger("hit");
                myMath.waitAndStart(4, () => { if (animator != null)
                    {
                        animator.SetBool("stunned", false); wait = true;
                    }
});
            }
        }
        animator.SetBool("charge", charging);

        
    }
    void hitEffect()
    {
        foreach (MeshRenderer meshRenderer in mrs)
        {
            Material[] saveMats = meshRenderer.materials;
            Material[] myEfMats = new Material[saveMats.Length];
            for (int i = 0; i < saveMats.Length; i++)
            {
                myEfMats[i] = hitMaterial;
            }
            meshRenderer.materials = myEfMats;
            myMath.waitAndStart(.2f, () => {if (meshRenderer != null)
                {
                    meshRenderer.materials = saveMats;
                }
});
        }
    }
    public void hit(float damage)
    {
        Camera.main.transform.parent.GetComponent<Animator>().SetTrigger("hit");
        hitable = false;
        myMath.waitAndStart(.5f, () => { hitable = true; });
        Vector3 velo = (transform.position - ch.transform.position).normalized;
        rb.velocity = velo * 2;
        hitEffect();
        health -= damage;
        myMath.slowMoEf(.1f);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "hit" && hitable)
        {
            hit(ch.GetComponent<character>().damage);
        }
    }

}

