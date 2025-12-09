using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCheck : MonoBehaviour
{
    [SerializeField] private Image p1ReadyImage;
    [SerializeField] private Image p2ReadyImage;

    [SerializeField] private GameObject p1;
    [SerializeField] private GameObject p2;

    [SerializeField] private StageSelect stageSelect;
    [SerializeField] private SelectedStage stage;
    public bool[] playerReady { get; private set; }
    //panelを開いているか
    public bool isActive = false;
    private bool ready = false;
    private SceneChanger changer;

    public void PlayerReady(int index,bool toReady)
    {
        playerReady[index] = toReady;
        Vector3 scale=toReady ? Vector3.one : Vector3.zero;
        if (index == 0)
            p1ReadyImage.transform.DOScale(scale, 0.2f).SetEase(Ease.OutBack);
        else
            p2ReadyImage.transform.DOScale(scale, 0.2f).SetEase(Ease.OutBack);
    }

    public async UniTask CheckSelect()
    {
        await UniTask.WaitUntil(() => stageSelect.isSelect);
        isActive = true;
        transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBack);
    }

    public async UniTask CheckBothReady()
    {
        await UniTask.WaitUntil(() => {
            ready = (playerReady[0] && playerReady[1]);
            return ready;
        });
        await UniTask.Delay(1000);
        changer = new SceneChanger();
        changer.ToPlay(stage.StageName);
    }

    public void CancelAll()
    {
        stageSelect.isSelect = false;
        stageSelect.isMove = false;
        InitStatus();
        // UI アニメーションで隠す
        transform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.InBack);
        p1ReadyImage.transform.DOScale(Vector3.zero, 0.2f);
        p2ReadyImage.transform.DOScale(Vector3.zero, 0.2f);

        CheckSelect().Forget();
    }

    public void InitStatus()
    {
        playerReady = new bool[2] { false, false };
        isActive = false;
        ready = false;

        transform.localScale = Vector3.zero;
        p1ReadyImage.transform.localScale = Vector3.zero;
        p2ReadyImage.transform.localScale = Vector3.zero;
    }
}
