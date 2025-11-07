using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class ScoreGauge : MonoBehaviour
{
    [Header("ランク画像")]
    [NamedArray(new string[] { "F", "C", "B", "A", "S" })]
    [SerializeField] private Image[] rankImages = new Image[5];

    [Header("スコアデータ")]
    [SerializeField] private ScoreData scoreData;

    [Header("スコアゲージ")]
    [SerializeField] private Slider scoreSlider;

    [Header("ゲージ更新速度 (スコア連動速度)")]
    [SerializeField] private float fillSpeed = 0.5f;

    // 現在のランクを記録
    private int currentRankIndex = 0;

    private void Start()
    {
        foreach (var img in rankImages)
        {
            img.transform.localScale = Vector3.one;
            img.color = new Color(1, 1, 1, 0.3f); // 半透明
        }
    }

    private void Update()
    {
        // スコア値を正規化してスライダーに反映
        float normalizedScore = Mathf.InverseLerp(0, ScoreRankBoundary.RANK_S, scoreData.score);
        scoreSlider.value = Mathf.Lerp(scoreSlider.value, normalizedScore, Time.deltaTime * fillSpeed);

        // ランク判定
        int rankIndex = GetRankIndex(scoreData.score);

        // ランクが上がった時だけ演出する
        if (rankIndex != currentRankIndex)
        {
            currentRankIndex = rankIndex;
            AnimateRank(rankIndex).Forget();
        }

        
    }

    /// <summary>
    /// 現在のスコアからランクインデックスを返す (0=F, 1=C, 2=B, 3=A, 4=S)
    /// </summary>
    private int GetRankIndex(int score)
    {
        if (score >= ScoreRankBoundary.RANK_S) return 4;
        if (score >= ScoreRankBoundary.RANK_A) return 3;
        if (score >= ScoreRankBoundary.RANK_B) return 2;
        if (score >= ScoreRankBoundary.RANK_C) return 1;
        return 0;
    }

    /// <summary>
    /// 指定ランクを拡大表示
    /// </summary>
    private async UniTask AnimateRank(int index)
    {
        for (int i = 0; i < rankImages.Length; i++)
        {
            Image img = rankImages[i];
            if (i == index)
            {
                img.DOFade(1f, 0.2f);
                img.transform.DOScale(1.3f, 0.2f).SetEase(Ease.OutBack);
            }
            else
            {
                img.DOFade(0.3f, 0.2f);
                img.transform.DOScale(1f, 0.2f).SetEase(Ease.InOutSine);
            }
        }

        // 光る
        await UniTask.Delay(200);
        rankImages[index].transform
            .DOScale(1.4f, 0.1f)
            .SetLoops(2, LoopType.Yoyo)
            .SetEase(Ease.OutQuad);
    }
}
