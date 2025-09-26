using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaiderWeb : MonoBehaviour
{

    [Header("ˆÚ“®İ’è")]
    public float speed = 2.0f; // ’Êí‚ÌˆÚ“®‘¬“x
    public Vector3 direction = new Vector3(0, 0, 1); // ‰œ(Z)•ûŒü‚É“®‚­



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // ˆÚ“®ˆ—
        transform.Translate(direction * speed * Time.deltaTime);
    }
}
