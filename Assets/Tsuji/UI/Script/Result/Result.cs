using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    //Score取得
    [SerializeField] int score;
    [SerializeField] TextMeshProUGUI scoreText;

    //rank表示
    [SerializeField] Sprite[] rankSprites;
    [SerializeField] Image rank;

    //clearロゴ
    [SerializeField] Sprite[] rogoSprites;
    [SerializeField] Image rogo;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //クリアロゴ表示
        //でかくする
       // rank.sprite.scale(new Vector3(1.5f, 1.5f, 1.5f));

    }

    // Update is called once per frame
    private async UniTask Update()
    {
        //ボタンを押したら
        //スコア
        await UniTask.WaitUntil(() => Input.GetButtonDown("Submit"));
        await ScoreAnimation(score, 3f);
        //ランク表示

        //効果音
    }

    //ランクアニメーション
    public async UniTask RankAnim(float time)
    {
        //一気に小さくする
        rank.transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 1f);
        //戻す
        rank.transform.DOScale(new Vector3(1,1,1), 1f);
    }


    // スコアをアニメーションさせる
    public async UniTask ScoreAnimation(int addScore, float time)
    {
        float before = 0;
        float after = score + addScore;
        score += addScore;

        float elapsedTime = 0f;


        // timeが経過するまでループ
        while (elapsedTime < time)
        {
            float rate = elapsedTime / time;
            scoreText.text = (before + (after - before) * rate).ToString("f0");

            elapsedTime += Time.deltaTime;

            await UniTask.Delay(10); // 10ミリ秒待つ
        }

        // 最終的な着地スコアを表示
        scoreText.text = after.ToString("f0");
    }
}
