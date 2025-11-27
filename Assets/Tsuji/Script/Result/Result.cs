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
    
    [Header("button")]
    [SerializeField] private Button[] buttons;

    //職業
    [SerializeField] private SelectedPlayerJob[] job = new SelectedPlayerJob[2];
    [SerializeField] private ResultPlayer resultPlayer;

    private bool isResultShown = false;

    [SerializeField] private AudioClip[] se=new AudioClip[2];
    [SerializeField] private AudioClip[] bgm=new AudioClip[2];

    private bool clear=true;

    private void Start()
    {
        // 初期状態で非表示などの設定をしておく
        rank.transform.localScale = Vector3.zero;
        logo.transform.localScale = Vector3.zero;

        for (int i = 0; i < buttons.Length; i++) 
        {
            buttons[i].interactable = false;
        }

        resultPlayer.JobChange(job[0].playerJobName, 0);
        resultPlayer.JobChange(job[1].playerJobName, 1);

        // 実行開始
        WaitForSubmitAsync().Forget();
    }

    // 入力待ち
    private async UniTaskVoid WaitForSubmitAsync()
    {
        await UniTask.Delay(3000);
        if (isResultShown) return; // 二重実行防止

        isResultShown = true;

        // ScriptableObject からスコアを取得
        int score = (int)scoreData.score;
        if (score <= 0) { score = 0; clear = false; }
        await ShowResultAsync(score);
    }

    // 一連のリザルト演出
    private async UniTask ShowResultAsync(int score)
    {
        SoundManager.Instance.PlaySFX(se[0], SoundManager.Instance.GetSFXVolume(), 2.0f);

        // スコアアニメーション
        await ScoreAnimationAsync(score, 2.5f);
        // ランク演出
        await RankAnimAsync();
        // ロゴ演出
        await LogoAnimAsync();
        //ボタン使えるようにする
        ButtonsControl();

        
        //BGM流す
        await SoundManager.Instance.PlayBGM(bgm[clear? 0:1]);
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
        SoundManager.Instance.PlaySFX(se[1]);

    }

    // ランク演出
    private async UniTask RankAnimAsync()
    {
        SoundManager.Instance.PlaySFX(se[1]);
        rank.transform.localScale = Vector3.zero;
        rank.DOFade(1f, 0f); // フェード即時反映
        rank.transform.DOScale(Vector3.one * 1.2f, 0.6f).SetEase(Ease.OutBack);
        await UniTask.Delay(300);
        rank.transform.DOScale(Vector3.one, 0.4f).SetEase(Ease.OutCubic);
        await UniTask.Delay(400);
    }

    // ロゴ演出
    private async UniTask LogoAnimAsync()
    {
        logo.transform.localScale = Vector3.zero;
        SoundManager.Instance.PlaySFX(se[1]);

        // ① 1段階目の演出を待つ
        await logo.transform
            .DOScale(Vector3.one * 1.2f, 0.4f)
            .SetEase(Ease.OutBack)
            .AsyncWaitForCompletion(); // ←重要！

        // ② 2段階目も待つ
        await logo.transform
            .DOScale(Vector3.one, 0.2f)
            .AsyncWaitForCompletion();
        // ③ 全て終わってからアニメーション再生
        resultPlayer.PlayerAnim(clear, 0);
        resultPlayer.PlayerAnim(clear, 1);

    }


    private void ButtonsControl()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = true;
        }        
    }

    private void OnDestroy()
    {
        scoreData.score = 0;
    }
}
