using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Skill_icon : MonoBehaviour
{
    public skilldata skill;
    public Image cooldown_iconn;
    public Transform cooldown_transform;
    public Transform stacks_transform;

    public Transform coincost_transform;

    public Image gcd_icon;
    public TMP_Text stacks_text;
    public TMP_Text coincost_text;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (skill != null)
        {
            
        }
    }
}
