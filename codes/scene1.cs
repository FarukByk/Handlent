using System;
using UnityEngine;

public class scene1 : MonoBehaviour
{
    public GameObject[] aloneNugget;
    public BoxCollider bx;
    public character chara;
    void Update()
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
            bx.enabled = true;
            chara.lookTarget = bx.gameObject.transform;
            chara.look = true;
        }
    }
}

