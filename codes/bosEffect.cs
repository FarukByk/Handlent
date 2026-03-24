using UnityEngine;

public class bosEffect : MonoBehaviour
{
    public GameObject broke;
    Transform pos;
    public float second;
    void Start()
    {
        pos = transform.Find("efPos");
    }
    void Update()
    {
        
    }
    public void crack()
    {
        GameObject go = Instantiate(broke);
        go.transform.position = pos.position;
        go.transform.rotation = pos.rotation;
        Camera.main.transform.parent.GetComponent<Animator>().SetTrigger("hit");
        Destroy(go,second);
        soundSystem.play("hit2");
    }
}

