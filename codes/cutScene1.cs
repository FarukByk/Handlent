using UnityEngine;
using UnityEngine.SceneManagement;

public class cutScene1 : MonoBehaviour
{
    public GameObject t0, t1;
    int a = 0;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            a++;
        }
        switch (a)
        {
            case 1:
                t0.SetActive(true);
                break;
            case 2:

                t1.SetActive(true);
                break;
            case 3:
                a = 0;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                break;
        }
    }
}

