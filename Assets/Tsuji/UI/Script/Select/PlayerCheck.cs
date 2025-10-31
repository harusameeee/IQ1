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

    private StageSelect stageSelect;
    private SceneChanger changer;

    private bool p1Ready = false;
    private bool p2Ready = false;

    private int playerNumber = 0;
    private int player2Number = 0;

    [SerializeField] private SelectedStage stage;

    private void Start()
    {
        GameObject obj = GameObject.Find(stage.StageName);
        //GameObject obj = GameObject.Find("Stage1");
        stageSelect = obj.GetComponent<StageSelect>();
        // 最初は非表示
        transform.localScale = Vector3.zero;
        p1ReadyImage.transform.localScale = Vector3.zero;
        p2ReadyImage.transform.localScale = Vector3.zero;

        playerNumber = p1.GetComponent<PlayerMove>().playerNumber;
        player2Number = p2.GetComponent<PlayerMove>().playerNumber;

        // 処理スタート
        CheckSelect().Forget();
        CheckBothReady().Forget();
    }

    private void LateUpdate()
    {
        if (!stageSelect.isSelect) return;

        string jumpButton = playerNumber == 1 ? "Submit" : "Submit2";
        string jumpButton2 = player2Number == 2 ? "Submit2" : "Submit";

        // 1P の入力 (例: Aボタン)
        if (Input.GetButtonDown(jumpButton)&&!p1Ready)
        {
            p1Ready = true;
            p1ReadyImage.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);
            Debug.Log(playerNumber);
        }

        // 2P の入力 (例: Aボタン)
        else if (Input.GetButtonDown(jumpButton2) && !p2Ready)
        {
            p2Ready = true;
            p2ReadyImage.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);
            Debug.Log(player2Number);

        }
    }

    private async UniTask CheckSelect()
    {
        await UniTask.WaitUntil(() => stageSelect.isSelect);
        transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);
    }

    private async UniTask CheckBothReady()
    {
        await UniTask.WaitUntil(() => p1Ready && p2Ready);
        await UniTask.Delay(1000); // 少し待ってからシーン遷移
        changer = new SceneChanger();
        changer.ToPlay(stage.StageName);
    }
}
