using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DangerEffect : MonoBehaviour
{
    [SerializeField] Image targetImage;   // 点滅させたいUIイメージ
    public float speed = 2.0f;    // 点滅スピード
    public float minAlpha = 0.2f; // 一番暗い時の透明度
    public float maxAlpha = 1f;   // 一番明るい時の透明度

    void Update()
    {
        // 0〜1を繰り返す値を生成（サイン波）
        float t = (Mathf.Sin(Time.time * speed) + 1f) / 2f;

        // minAlpha〜maxAlphaの範囲で補間
        float alpha = Mathf.Lerp(minAlpha, maxAlpha, t);

        // Imageの色に反映
        Color c = targetImage.color;
        c.a = alpha;
        targetImage.color = c;
    }
}
