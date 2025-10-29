using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StageSelect : MonoBehaviour
{
    [Header("ボタンのYスケール（押し込み度）")]
    [SerializeField] private float pressedScaleY = 0.01f;

    [Header("円ゲージUI")]
    [SerializeField] private Image circle;

    [Header("選択ステージ情報")]
    [SerializeField] private SelectedStage selectedStage;

    // ステージ選択完了状態
    public bool isSelect { get; private set; } = false;

    // Tweenの参照
    private Tween fillTween;

    // 衝突中のプレイヤーリスト
    private readonly List<GameObject> hitObjects = new();

    private void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag("Player") || other.CompareTag("Player2")) && !isSelect)
        {
            // 重複登録を防ぐ
            if (!hitObjects.Contains(other.gameObject))
            {
                hitObjects.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Player2"))
        {
            hitObjects.Remove(other.gameObject);

            // 誰も乗っていない場合はリセット
            if (hitObjects.Count < 2 && !isSelect)
            {
                ResetCircle();
            }
        }
    }

    private void FixedUpdate()
    {
        // 2人乗っていて、まだ選択完了していない場合
        if (hitObjects.Count >= 2 && !isSelect)
        {
            // 押し込みエフェクト
            transform.localScale = new Vector3(transform.localScale.x, pressedScaleY, transform.localScale.z);

            // DOTweenが未再生なら再生開始
            if (fillTween == null || !fillTween.IsPlaying())
            {
                circle.fillAmount = 0;
                fillTween = circle.DOFillAmount(1f, 3f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    isSelect = true;
                    OnStageSelected();
                });
            }
        }
    }

    private void ResetCircle()
    {
        // Tweenを停止してリセット
        fillTween?.Kill();
        fillTween = null;
        circle.fillAmount = 0f;
        transform.localScale = new Vector3(1f, 0.1f, 5f);
    }

    private void OnStageSelected()
    {
        Debug.Log($"ステージ「{gameObject.name}」が選択されました！");
        // ScriptableObjectに記録（例：選ばれたステージ名を保存）
        if (selectedStage != null)
        {
            selectedStage.SetStageName(gameObject.name);
        }

        // 選択演出（例：光る・拡大）
        circle.transform.DOScale(1.2f, 0.3f).SetLoops(2, LoopType.Yoyo);
    }
}
