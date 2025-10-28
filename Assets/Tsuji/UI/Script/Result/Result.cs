using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    [Header("score")]
    [SerializeField] private ScoreData scoreData;
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("rank")]
    [SerializeField] private Image rank;

    [Header("logo")]
    [SerializeField] private Image logo;

    private bool isResultShown = false;

    private void Start()
    {
        // 初期状態で非表示などの設定をしておく
        rank.transform.localScale = Vector3.zero;
        logo.transform.localScale = Vector3.zero;

        // 実行開始
        WaitForSubmitAsync().Forget();
    }

    // 入力待ち
    private async UniTaskVoid WaitForSubmitAsync()
    {
        //await UniTask.WaitUntil(() => Input.GetButtonDown("Submit"));
        await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0));
        if (isResultShown) return; // 二重実行防止

        isResultShown = true;

        // ScriptableObject からスコアを取得
        int score = scoreData != null ? scoreData.score : 0;
        await ShowResultAsync(score);
    }

    // 一連のリザルト演出
    private async UniTask ShowResultAsync(int score)
    {
        // ロゴ演出
        await LogoAnimAsync();

        // スコアアニメーション
        await ScoreAnimationAsync(score, 2.5f);

        // ランク演出
        await RankAnimAsync();

    }

    // スコアアニメーション
    private async UniTask ScoreAnimationAsync(int addScore, float time)
    {
        float before = 0f;
        float after = addScore;
        float elapsedTime = 0f;

        while (elapsedTime < time)
        {
            float rate = elapsedTime / time;
            scoreText.text = Mathf.Lerp(before, after, rate).ToString("f0");
            elapsedTime += Time.deltaTime;
            await UniTask.Yield(); // 毎フレーム更新
        }

        scoreText.text = after.ToString("N0");
    }

    // ランク演出
    private async UniTask RankAnimAsync()
    {
        rank.transform.localScale = Vector3.zero;
        rank.DOFade(1f, 0f); // フェード即時反映
        rank.transform.DOScale(Vector3.one * 1.2f, 0.6f).SetEase(Ease.OutBack);
        await UniTask.Delay(600);
        rank.transform.DOScale(Vector3.one, 0.4f).SetEase(Ease.OutCubic);
        await UniTask.Delay(400);
    }

    // ロゴ演出
    private async UniTask LogoAnimAsync()
    {
        logo.transform.localScale = Vector3.zero;

        // ポップアップ風に表示
        logo.transform.DOScale(Vector3.one * 1.2f, 0.4f).SetEase(Ease.OutBack);
        await UniTask.Delay(400);
        logo.transform.DOScale(Vector3.one, 0.2f);
    }
}
