using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class VolumeOption : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI masterText;
    [SerializeField] private TextMeshProUGUI bgmText;
    [SerializeField] private TextMeshProUGUI sfxText;

    [SerializeField] private UnityEngine.UI.Image[] images;

    [SerializeField] private Sprite[] volumeImages;

    public static VolumeOption instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        //最初は表示させない
        Close();
    }

    void Update()
    {
        var master = (int)(SoundManager.Instance.GetMasterVolume() * 100);
        var bgm = (int)(SoundManager.Instance.GetBGMVolume() * 100);
        var sfx = (int)(SoundManager.Instance.GetSFXVolume() * 100);
        var brightness = (int)(BrightnessAdjustment.Instance.GetBrightness()*10);
        masterText.text = master.ToString();
        bgmText.text = bgm.ToString();
        sfxText.text = sfx.ToString();
        //brightnessText.text = brightness.ToString();

        ChangeImages(master, 0);
        ChangeImages(bgm, 1);
        ChangeImages(sfx, 2);
    }

    //表示させる
    public void Open()
    {
        gameObject.SetActive(true);
    }

    //非表示にする
    public void Close()
    {
        gameObject.SetActive (false);
    }

    //音量に応じて画像を変更
    private void ChangeImages(int volume,int number)
    {
        //0
        if (volume == 0)
        {
            images[number].sprite = volumeImages[0];
        }
        //1
        else if(volume>=1&&volume<=33)
        {
            images[number].sprite = volumeImages[1];
        }
        //2
        else if (volume >= 34 && volume <= 64)
        {
            images[number].sprite = volumeImages[2];
        }
        //3
        else if (volume >= 65)
        {
            images[number].sprite = volumeImages[3];
        }
    }

}
