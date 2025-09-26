using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorScript : MonoBehaviour
{

    [SerializeField] Animator anim = null;
    bool horizontal = true;

    void Start()
    {
    }

    void Update()
    {
        //bool horizontalKey = Input.GetKey(KeyCode.RightArrow);

        if (horizontal == true)
        {
            anim.SetBool("Run", true);
            anim.SetBool("Fly", true);
        }
        else
        {
            anim.SetBool("Run", false);
            anim.SetBool("Fly", false);
        }
    }
}
