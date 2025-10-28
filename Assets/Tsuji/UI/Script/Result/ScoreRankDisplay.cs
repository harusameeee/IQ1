using UnityEngine;
using UnityEngine.UI;


public class ScoreRankDisplay : MonoBehaviour
{
    //スコアデータ
    [SerializeField] private ScoreData scoreData;

    //ランク表示用
    [NamedArray(new string[] { "C", "B", "A", "S" })] 
    [SerializeField] Sprite[] rankSprites = new Sprite[4];
    Image rankImage;

    void Start()
    {
        // ScriptableObject からスコアを取得
        int score = scoreData != null ? scoreData.score : 0;
        rankImage = gameObject.GetComponent<Image>();

        ScoreCheck(score);
    }


    private void ScoreCheck(int score)
    {
        //スコアによってランク表示を切り替える
        if (score < ScoreRankBoundary.RANK_C)
        {
            rankImage.sprite = rankSprites[0];
        }
        else if (score < ScoreRankBoundary.RANK_B)
        {
            rankImage.sprite = rankSprites[1];
        }
        else if (score < ScoreRankBoundary.RANK_A)
        {
            rankImage.sprite = rankSprites[2];
        }
        else if (score < ScoreRankBoundary.RANK_S)
        {
            rankImage.sprite = rankSprites[3];
        }
    }
}
