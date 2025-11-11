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

    [SerializeField] StageSelect stageSelect;
    private SceneChanger changer;

    public bool[] playerReady{get; set; }

    public bool isActive = false;

    [SerializeField] private SelectedStage stage;

    private void Start()
    {
        // 最初は非表示
        transform.localScale = Vector3.zero;
        p1ReadyImage.transform.localScale = Vector3.zero;
        p2ReadyImage.transform.localScale = Vector3.zero;

        // 初期化を明示
        playerReady = new bool[2] { false, false };

        // 処理スタート
        CheckSelect().Forget();
        CheckBothReady().Forget();
    }

    private void LateUpdate()
    {
        if (!stageSelect.isSelect) return;

        // 1P の入力 (Aボタン)
        if (Input.GetButtonDown("Submit") && playerReady[0])
        {
            p1ReadyImage.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);
        }
        // 2P の入力 (Aボタン)
        if (Input.GetButtonDown("Submit2") && playerReady[1])
        {
            p2ReadyImage.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);
        }
    }

    private async UniTask CheckSelect()
    {
        //ステージ選択状態
        await UniTask.WaitUntil(() => stageSelect.isSelect);
        transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);
        isActive = true;
    }

    private async UniTask CheckBothReady()
    {
        await UniTask.WaitUntil(() => {
            bool ready = (playerReady[0] && playerReady[1]);
            return ready;
        });
        
        await UniTask.Delay(1000); // 少し待ってからシーン遷移
        IrisShot.Instance.IrisOut();
        await UniTask.WaitUntil(() => !IrisShot.Instance.isIrisShot);
        changer = new SceneChanger();
        changer.ToPlay(stage.StageName);
    }
}
