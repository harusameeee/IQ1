using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class BrightnessOption : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Sprite[] changeSprites;


    private void Update()
    {
        var brightness = (int)(BrightnessAdjustment.Instance.GetBrightness() * 10);
        ChangeImages(brightness);
    }

    private void ChangeImages(int brightness)
    {
        //0
        if (brightness == 0)
        {
            image.sprite = changeSprites[0];
        }
        //1
        else if (brightness >= 1 && brightness <= 3)
        {
            image.sprite = changeSprites[1];
        }
        //2
        else if (brightness >= 3 && brightness <= 6)
        {
            image.sprite = changeSprites[2];
        }
        //3
        else if (brightness >= 6)
        {
            image.sprite = changeSprites[3];
        }
    }

}

