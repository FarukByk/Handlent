using System.Security.Cryptography;
using UnityEditor.Rendering;
using UnityEngine;

public class enemy : MonoBehaviour
{	
    Rigidbody rb;
    Animator animator;
    public float health;
    public Material hitMaterial;
    public MeshRenderer[] mrs;
    public float speed;
    bool hitable = true;
    public GameObject mermi;
    character ch;
    public float hitSecond;
    float maxHitSecond;
    bool attackable;
    bool walkable;
    public GameObject part;
    void Start()
    {
        ch = FindAnyObjectByType<character>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        maxHitSecond = hitSecond;
    }
    void Update()
    {
        transform.LookAt(new Vector3(ch.transform.position.x,0,ch.transform.position.z));
        walk();
        if (health < 0)
        {
            Instantiate(part,transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
    void walk()
    {
        walkable = hitable;
        if (walkable)
        {
            Vector3 velo = (ch.transform.position - transform.position).normalized;
            float ds = Vector3.Distance(transform.position, ch.transform.position);
            if (ds < 5)
            {
                rb.velocity = new Vector3(velo.x * -speed,rb.velocity.y,velo.z * -speed);
                attackable = true;
            }
            else if(ds > 10 && ds < 20)
            {
                rb.velocity = new Vector3(velo.x * speed, rb.velocity.y, velo.z * speed);
                attackable = false;
            }
            else if(ds < 10)
            {
                attackable = true;
            }

            if (attackable)
            {
                
                if (hitSecond > maxHitSecond)
                {
                    hitSecond = 0;
                    animator.SetTrigger("hit");
                }
            }
            if (hitSecond <= maxHitSecond)
            {
                hitSecond += Time.deltaTime;
            }
        }
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
            myMath.waitAndStart(.2f, () => { if (meshRenderer != null)
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
        rb.velocity = velo * 5;
        hitEffect();
        health -= damage;
        myMath.slowMoEf(.1f);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "hit" && hitable)
        {
            hit(ch.damage);
        }
    }
    public void attack()
    {
        GameObject go = Instantiate(mermi);
        go.transform.position = transform.Find("Particle System").position;
        go.GetComponent<mermi>().target = ch.transform;
        go.transform.LookAt(ch.transform.position + Vector3.up);
        Destroy(go,5);
        
    }
}

