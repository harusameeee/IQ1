using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; // ← DOTweenを使う
using Cysharp.Threading.Tasks; // ← UniTaskを使う

public class PlayToResult : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] Image finishImage;

    private SceneChanger changer;
    private bool isFinished = false;

    void Start()
    {
        changer = new SceneChanger();

        // 最初は非表示
        if (finishImage != null)
        {
            var c = finishImage.color;
            c.a = 0f;
            finishImage.color = c;
        }
    }

    void Update()
    {
        if (!isFinished && slider.value >= slider.maxValue)
        {
            isFinished = true;
            OnFinishAsync().Forget();
        }
    }

    private async UniTaskVoid OnFinishAsync()
    {

        // フェードイン（1秒でαを1に）
        finishImage.DOFade(1f, 1f).SetEase(Ease.InOutQuad);

        // 2秒待つ
        await UniTask.Delay(3000);

        // シーン遷移（SceneChangerに合わせて）
        changer.ToResult(); 
    }
}
