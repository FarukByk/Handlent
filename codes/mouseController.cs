using UnityEngine;

public class mouseController : MonoBehaviour
{
    public LayerMask mask;
    public Transform redDot;
    RaycastHit hit;
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit , 1000,mask))
        {
            redDot.position = hit.point;
            redDot.gameObject.SetActive(true);

        }
        else
        {
            redDot.gameObject.SetActive(false);
        }
    }
}

