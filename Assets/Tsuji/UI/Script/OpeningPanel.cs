using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class OpeningPanel : MonoBehaviour
{
    [SerializeField] Image image;

    public static OpeningPanel instance;

    public bool isfinish=false;
    float alpha=0.0f;
    float fadespeed = 0.2f;
    bool startFade = false;


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

        alpha=gameObject.GetComponent<Image>().color.a;
    }


    private async UniTask Close()
    {
        await UniTask.WaitUntil(() => Input.anyKey);
        startFade = true; // フェード開始
    }

    void Update()
    {
        if (startFade && !isfinish)
        {
            FadeIn();
        }
    }
    private  async UniTask Blinking()
    {
        image.enabled = false;
        await UniTask.Delay(TimeSpan.FromSeconds(1f));
        image.enabled = true;
    }

    private void FadeIn()
    {
        alpha -= Time.deltaTime/fadespeed;
        if (alpha <= 0.0f)
        {
            isfinish = true;
        }

        foreach (var img in GetComponentsInChildren<Image>())
        {
            Color c = img.color;
            img.color = new Color(c.r, c.g, c.b, alpha);
        }
    }

}
