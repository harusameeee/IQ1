using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class player_canvas_handler : MonoBehaviour
{
    // note due to time restrictions, the buff visualizer only supports one buff at a time
    public Image anchor_image;
    public Image dmg_zoneimg;
    public TMP_Text stay_together_text;
    public TMP_Text stay_apart_text;
    public Slider progress_slider;
    public PlayerLineMove owner;
    public buffdata visualized_buff;
    float buffdurationmax = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (owner == null)
        {
            return;
        }
        if (owner.buffs.Any(obj => obj.type == bufftypes.nojump))
        {
            anchor_image.gameObject.SetActive(true);
        }
        else
        {
            anchor_image.gameObject.SetActive(false);
        }
        if(visualized_buff != null)
        {
            if(visualized_buff.duration <= 0)
            {
                if (visualized_buff.type == bufftypes.stayaway)
                {
                    if(owner != null)
                    {
                        owner.exit_stayaway_buff(visualized_buff.pow);
                    }
                }
                else if (visualized_buff.type == bufftypes.sticktogether)
                {
                    if(owner != null)
                    {
                        owner.exit_staytogether_buff(visualized_buff.pow);
                    }
                }
                visualized_buff = null;
                buffdurationmax = 0;
                stay_apart_text.gameObject.SetActive(false);
                stay_together_text.gameObject.SetActive(false);
                dmg_zoneimg.gameObject.SetActive(false);
                progress_slider.gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("updating buff visualizer=" + visualized_buff.duration + "/" + visualized_buff.duration / buffdurationmax);
                progress_slider.value = (buffdurationmax-visualized_buff.duration) / buffdurationmax;
            }
        }
    }
    public void addbuffvisual(ref buffdata b)
    {
        visualized_buff = b;
        buffdurationmax = b.duration;

        stay_apart_text.gameObject.SetActive(false);
        stay_together_text.gameObject.SetActive(false);
        anchor_image.gameObject.SetActive(false);
        dmg_zoneimg.gameObject.SetActive(false);
        progress_slider.gameObject.SetActive(false);

        if (visualized_buff.type == bufftypes.stayaway)
        {

            stay_apart_text.gameObject.SetActive(true);
            dmg_zoneimg.gameObject.SetActive(true);
            progress_slider.gameObject.SetActive(true);
            dmg_zoneimg.color = new Color(1,0,0,0.5f);
        }
        else if (visualized_buff.type == bufftypes.sticktogether)
        {
            stay_together_text.gameObject.SetActive(true);
            dmg_zoneimg.gameObject.SetActive(true);
            progress_slider.gameObject.SetActive(true);
            dmg_zoneimg.color = new Color(0,1,0,0.5f);
        }

        progress_slider.value = 1;
    }
    public void removebuffvisual()
    {
        visualized_buff = null;
        buffdurationmax = 0;
        stay_apart_text.gameObject.SetActive(false);
        stay_together_text.gameObject.SetActive(false);
        anchor_image.gameObject.SetActive(false);
        dmg_zoneimg.gameObject.SetActive(false);
    }
}
