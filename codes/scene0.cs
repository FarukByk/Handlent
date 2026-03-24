using UnityEngine;

public class scene0 : MonoBehaviour
{
    public GameObject boss;
    public BoxCollider bx;
    public character chara;
    private void Update()
    {
        if (boss == null)
        {
            bx.enabled = true;
            chara.lookTarget = bx.gameObject.transform;
            chara.look = true;
           
        }
    }
}

