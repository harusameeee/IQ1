using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Skill_icon : MonoBehaviour
{
    public skilldata skill;
    public PlayerLineMove owner;
    public Image skill_icon;
   // public Transform stacks_transform;

    public Transform coincost_transform;

    public Image gcd_icon;
   // public TMP_Text stacks_text;
    public TMP_Text coincost_text;
    public TMP_Text cooldown_text;

    void Start()
    {
        
        gcd_icon.material = Instantiate(gcd_icon.material);
    }

    // Update is called once per frame
    void Update()
    {

        if (skill != null)
        {
            float gcdper = 1-owner.gcd_timer / owner.current_max_gcd;
            gcd_icon.material.SetFloat("_removesegment", gcdper);
            //if (stacks_transform.gameObject.activeSelf)
            //{
            //    stacks_text.text = skill.currentstacks.ToString();
            //}
            if (skill.has_cooldown && skill.currentcooldown > 0 && skill.maxstacks > skill.currentstacks)
            {
                cooldown_text.text = Mathf.Ceil(skill.currentcooldown).ToString();
            }
            else
            {
                cooldown_text.text = "";
            }
        }
    }
    public void setskill(skilldata s,PlayerLineMove o)
    {
        owner = o;
        skill = s;
        skill_icon.sprite = skill.icon;

        if (skill.coincost > 0)
        {
            coincost_transform.gameObject.SetActive(true);
            coincost_text.text = skill.coincost.ToString();
        }
        else
        {
            coincost_transform.gameObject.SetActive(false);
        }
    }
}
