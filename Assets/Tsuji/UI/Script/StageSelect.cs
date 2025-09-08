using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StageSelect : MonoBehaviour
{
    //　ボタン押すときの
    private float scaleY = 0.01f;
    //　円ゲージ
    [SerializeField] Image circle;
    //　ステージ選択状態か
    public bool isSelect { get; set; } = false;
    //
    private Tween fillTween;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // ボタン押してる風
            transform.localScale = new Vector3(1.0f, scaleY, 5.0f);

            if (fillTween == null || !fillTween.IsPlaying())
            {
                // 3秒かけて fillAmount を 1 に
                circle.fillAmount = 0;
                fillTween = circle.DOFillAmount(1f, 3f).OnComplete(() =>
                {
                    isSelect = true;
                });
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 離れたらリセット
            transform.localScale = Vector3.one;
            isSelect = false;

            // DOTween を止める
            fillTween?.Kill();
            circle.fillAmount = 0;
        }
    }
}
