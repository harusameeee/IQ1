using UnityEngine;
using UnityEngine.UI;

public class IconChange : MonoBehaviour
{
    [NamedArray(new string[]
    { "ninja","merlion","tontto"})]
    [SerializeField] Sprite[] hp50below;

    [NamedArray(new string[]
    { "ninja","merlion","tontto"})]
    [SerializeField] Sprite[] hp0;

    [NamedArray(new string[]
    { "ninja","merlion","tontto"})]
    [SerializeField] Sprite[] normal;

    [SerializeField] player_ui ui;
    //
    [SerializeField] Image icon;
    [SerializeField] SelectedPlayerJob job;
    private int num = 0;

    private void Start()
    {
        if (job.playerJobName=="merlion")
        {
            num = 1;
        }
        else if (job.playerJobName == "tonto")
        {
            num = 2;
        }
    }

    private void Update()
    {
        if (ui.hp_bar.value > 0.5)
        {
            icon.sprite = normal[num];
            
        }
        else if (ui.hp_bar.value > 0.001)
        {
            icon.sprite = hp50below[num];
        }
        else
        {
            icon.sprite = hp0[num];
        }
    }
}
