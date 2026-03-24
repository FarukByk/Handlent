using UnityEngine;
using UnityEngine.SceneManagement;

public class cutScene2 : MonoBehaviour
{
    public GameObject g0,t0, t1,t2;
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

                t2.SetActive(true);
                break;
                
            case 4:
                g0.SetActive(false);
                break;
            case 5:
                a = 0;
                Application.Quit();
                break;
        }
    }
}

