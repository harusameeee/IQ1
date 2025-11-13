using TMPro;
using UnityEngine;

public class delayedhb : MonoBehaviour
{
    
   
    [HideInInspector] public hitbox hb;
    public float delaycountdown = 3;
    public TMP_Text timertext;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hb = GetComponentInChildren<hitbox>();
        if (delaycountdown <= 0)
        {
            timertext.text = "";
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (delaycountdown > 0)
        {
            delaycountdown -= Time.deltaTime;
            if(delaycountdown <= 0)
            {
                timertext.text = "";
                hb.active = true;
            }
            else
            {
                timertext.text = delaycountdown.ToString("F0");
            }
            return;
        }
    }
}
