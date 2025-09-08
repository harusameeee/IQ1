using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorScript : MonoBehaviour
{

    [SerializeField] Animator anim = null;
    bool horizontalKey = true;

    void Start()
    {
    }

    void Update()
    {
        //bool horizontalKey = Input.GetKey(KeyCode.RightArrow);

        if (horizontalKey == true)
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
