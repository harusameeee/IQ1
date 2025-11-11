using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

public class SceneChanger : MonoBehaviour
{
    private async UniTask FadeLoadScene(string sceneName)
    {
        // 🔹 フェードアウト開始（閉じる）
        await IrisShot.Instance.IrisOut();

        // 🔹 完全に黒 → スピナー表示
        //IrisShot.Instance.SetBlack();
        LoadingSpinner.Instance.Show();

        // 🔹 シーンを非同期ロード（停止中）
        var asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        // 🔹 ロード完了まで待機
        await UniTask.WaitUntil(() => asyncLoad.progress >= 0.9f);

        // 🔹 ロード完了、スピナー消してシーン有効化
        LoadingSpinner.Instance.Hide();
        asyncLoad.allowSceneActivation = true;

        // 🔹 シーン完全ロード待ち
        await UniTask.WaitUntil(() => asyncLoad.isDone);

        // 🔹 フェードイン（黒→開く）
        await IrisShot.Instance.IrisIn();
    }

    public async void ToTitle() => await FadeLoadScene("Title");
    public async void ToSelect() => await FadeLoadScene("SelectScene");
    public async void ToResult() => await FadeLoadScene("ResultScene");
    public async void ToPlay(string stageName) => await FadeLoadScene(stageName);

    public async void ToEnd()
    {
        await IrisShot.Instance.IrisOut();
        Application.Quit();
    }
}
