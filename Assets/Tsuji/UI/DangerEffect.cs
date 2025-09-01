using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DangerEffect : MonoBehaviour
{
    [SerializeField]GameObject gameObject;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            gameObject.gameObject.SetActive(true);
        }
    }
}
