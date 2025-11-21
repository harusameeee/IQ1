using UnityEngine;
using UnityEngine.UI;

public class ScoreRankDisplay : MonoBehaviour
{
    [SerializeField] private ScoreData scoreData;

    [NamedArray(new string[] { "F","C", "B", "A", "S" })]
    [SerializeField] private Sprite[] rankSprites = new Sprite[5];

    private Image rankImage;

    private void Start()
    {
        rankImage = GetComponent<Image>();

        // 初期表示
        ScoreCheck(scoreData.score);

        // スコア変更イベント購読
        if (scoreData != null)
        {
            scoreData.OnScoreChanged += ScoreCheck;
        }
    }

    private void OnDestroy()
    {
        if (scoreData != null)
        {
            scoreData.OnScoreChanged -= ScoreCheck;
        }
    }

    private void ScoreCheck(int score)
    {
        if (rankImage == null) return;

        if(score <= ScoreRankBoundary.RANK_X)
            rankImage.sprite = rankSprites[0];
        else if (score >= ScoreRankBoundary.RANK_C && score < ScoreRankBoundary.RANK_B)
            rankImage.sprite = rankSprites[1];
        else if (score >= ScoreRankBoundary.RANK_B && score < ScoreRankBoundary.RANK_A)
            rankImage.sprite = rankSprites[2];
        else if (score >= ScoreRankBoundary.RANK_A && score < ScoreRankBoundary.RANK_S)
            rankImage.sprite = rankSprites[3];
        else if (score >= ScoreRankBoundary.RANK_S)
            rankImage.sprite = rankSprites[4];
        else
            rankImage.sprite = rankSprites[0];
    }
}
