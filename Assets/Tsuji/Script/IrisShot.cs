using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class IrisShot : MonoBehaviour
{
    public static IrisShot Instance { get; private set; }

    [SerializeField] private RectTransform unmask;
    [SerializeField] private Vector2 IRIS_IN_SCALE = new Vector2(20, 20);
    [SerializeField] private Vector2 IRIS_MID_SCALE1 = new Vector2(2.5f, 2.5f);
    [SerializeField] private Vector2 IRIS_MID_SCALE2 = new Vector2(2.8f, 2.8f);

    public bool isIrisShot { get; private set; } = false;

    private void Awake()
    {
        // シーンを跨いでも破棄されないように
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        //DontDestroyOnLoad(gameObject);

        IrisIn().Forget();
    }

    // フェードイン
    public async UniTask IrisIn()
    {

        isIrisShot = true;

        var seq = DOTween.Sequence();

        seq.Append(unmask.DOScale(IRIS_MID_SCALE2, 0.6f).SetEase(Ease.InCubic));
        seq.Append(unmask.DOScale(IRIS_MID_SCALE1, 0.4f).SetEase(Ease.OutCubic));
        seq.Append(unmask.DOScale(IRIS_IN_SCALE, 0.6f).SetEase(Ease.InCubic));

        await seq.AsyncWaitForCompletion();
        isIrisShot = false;
    }

    //フェードアウト
    public async UniTask IrisOut()
    {
        isIrisShot = true;

        var seq = DOTween.Sequence();

        seq.Append(unmask.DOScale(IRIS_MID_SCALE1, 0.4f).SetEase(Ease.InCubic));
        seq.Append(unmask.DOScale(IRIS_MID_SCALE2, 0.2f).SetEase(Ease.OutCubic));
        seq.Append(unmask.DOScale(Vector2.zero, 0.4f).SetEase(Ease.InCubic));

        await seq.AsyncWaitForCompletion();
        isIrisShot = false;
    }
}
