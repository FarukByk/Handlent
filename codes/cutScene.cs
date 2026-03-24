using UnityEngine;
using UnityEngine.SceneManagement;

public class cutScene : MonoBehaviour
{	
    public GameObject g0 , g1,t0,t1;
    public GameObject music;

    public void Start()
    {
        DontDestroyOnLoad(music);
    }
    int a = -1;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            a++;
        }
        switch (a)
        {
            case 0:
                g0.SetActive(true); break;
            case 1:
                g0.SetActive(false);
                g1.SetActive(true); break;
            case 2:
                t0.SetActive(true);
                break;
            case 3:

                t1.SetActive(true);
                break;
            case 4:
                a = 0;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                break;
        }
    }
}

