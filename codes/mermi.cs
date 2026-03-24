using UnityEngine;

public class mermi : MonoBehaviour
{
    public Transform target;
    public float speed;
    public float turnSpeed;
    Vector3 dif;
    bool a;
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        myMath.waitAndStart(1,()=> a = true);
        soundSystem.play("dog");
    }
    void Update()
    {
        if (!a)
        {
            dif = Vector3.Lerp(dif, (target.position + Vector3.up - transform.position).normalized, Time.deltaTime * turnSpeed);
            rb.velocity = dif * speed;
        }
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Destroy(gameObject,.1f);
        }
    }
}

