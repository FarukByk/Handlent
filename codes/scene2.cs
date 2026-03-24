using UnityEngine;
using UnityEngine.SceneManagement;

public class scene2 : MonoBehaviour
{
    public Transform camPos;
    public Transform cam;
    public GameObject door;
    public character chara;
    public GameObject boss;
    public GameObject[] aloneNugget;
    bool z;

    void Update()
    {
        if (!z)
        {
            bool a = false;
            foreach (GameObject o in aloneNugget)
            {
                if (o != null)
                {
                    a = true;
                }
            }
            if (!a)
            {
                door.SetActive(false);
                chara.look = true;
            }
        }
        if (boss == null)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            z = true;
            boss.SetActive(true);
            cam.position = camPos.position;
            cam.rotation = camPos.rotation;
            door.SetActive(true);
            chara.look = false;

        }
    }
}

