using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class IrisShot : MonoBehaviour
{
    [SerializeField] RectTransform unmask;

    readonly Vector2 IRIS_IN_SCALE = new Vector2(40f, 40f);
    readonly Vector2 IRIS_MID_SCALE1 = new Vector2(5.0f, 5.0f);
    readonly Vector2 IRIS_MID_SCALE2 = new Vector2(2.0f, 2.0f);

    public static IrisShot Instance { get; private set; }

    //遷移中か？
    public bool isIrisShot { get; set; } = false;   

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        IrisIn();
    }

    private void Update()
    {
        //おためし
        if (Input.GetMouseButtonDown(0))
        {
            IrisIn();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            IrisOut();
        }
    }

    //フェードアウト
    public void IrisIn()
    {
        isIrisShot = true;
        unmask.DOScale(IRIS_MID_SCALE2, 0.6f).SetEase(Ease.InCubic); 
        unmask.DOScale(IRIS_MID_SCALE1, 0.4f).SetDelay(0.4f).SetEase(Ease.OutCubic);
        unmask.DOScale(IRIS_IN_SCALE, 0.4f).SetDelay(0.6f).SetEase(Ease.InCubic);
        isIrisShot=false;
    }

    //フェードイン
    public void IrisOut()
    {
        isIrisShot = true;
        unmask.DOScale(IRIS_MID_SCALE1,0.4f).SetEase(Ease.InCubic);
        unmask.DOScale(IRIS_MID_SCALE2, 0.4f).SetDelay(0.2f).SetEase(Ease.OutCubic);
        unmask.DOScale(new Vector2(0, 0), 0.6f).SetDelay(0.4f).SetEase(Ease.InCubic);
        isIrisShot = false;
    }
}
