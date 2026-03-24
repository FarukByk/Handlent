using UnityEngine;

public class freeArm : MonoBehaviour
{	
    Rigidbody rb;
    public float speed;
    public bool escape;
    Transform ch;
    Animator animator;
    void Start()
    {
        ch = FindAnyObjectByType<character>().transform;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        if (escape && Vector3.Distance(transform.position,ch.position) < 10)
        {
            Vector3 velo = (transform.position - ch.position).normalized;
            rb.velocity = new Vector3(velo.x * speed, rb.velocity.y, velo.z * speed);
            float roty = Mathf.Atan2(velo.x, velo.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, roty, 0);
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
        animator.SetBool("walk",escape && Vector3.Distance(transform.position, ch.position) < 10);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && this.enabled)
        {
            FindAnyObjectByType<character>().getArm();
            Destroy(gameObject);
            soundSystem.play("hit1");
        }
    }
}

