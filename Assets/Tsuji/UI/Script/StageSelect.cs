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

    // 衝突しているオブジェクトリスト
    private List<GameObject> hitObjects = new List<GameObject>();

    private void OnTriggerStay(UnityEngine.Collider other)
    {
        if (!other.CompareTag("Stage")&&!isSelect)
        {
            // 衝突しているオブジェクトをリストに登録する
            hitObjects.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((other.CompareTag("Player")|| other.CompareTag("Player2"))&&!isSelect)
        {
            // 離れたらリセット
            transform.localScale = new Vector3(1f, 0.1f, 5f);
            isSelect = false;

            // DOTween を止める
            fillTween?.Kill();
            circle.fillAmount = 0;

            hitObjects.Clear();
        }
    }

    private void FixedUpdate()
    {
        if (hitObjects.Count > 3)
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
}
