using UnityEngine;
using UnityEngine.UI;

public class dmgnum : MonoBehaviour
{
    public TMPro.TMP_Text dmgnumtext;
    public Image dmgnumimg;
    public Animation dmgnumanim;
    public void Update()
    {
        
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,
            Camera.main.transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }
    public void DisableDmgNum()
    {
        this.gameObject.SetActive(false);
    }
    
}
