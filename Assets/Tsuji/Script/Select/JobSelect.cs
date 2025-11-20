using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using DG.Tweening;

public class JobSelect : MonoBehaviour
{
    [Header("ジョブ画像")]
    [NamedArray(new string[] { "Ninja", "Marlion", "Tonto" })]
    [SerializeField] private Sprite[] jobImages = new Sprite[3];

    [Header("ジョブ名")]
    [NamedArray(new string[] { "Ninja", "Marlion", "Tonto" })]
    [SerializeField] private Sprite[] jobNames = new Sprite[3];

    [Header("矢印画像")]
    [SerializeField] private Image[] arrows = new Image[2];

    [Header("ジョブ差し替え位置")]
    [SerializeField] private Image select;
    
    [Header("ジョブ差し替え位置")]
    [SerializeField] private Image selectJobName;

    [Header("プレイヤー番号 (1P=0, 2P=1)")]
    public int playerNumber = 0;

    [Header("チェック管理")]
    [SerializeField] private PlayerCheck pc;

    private bool isSelect = false;
    private int jobNum = 0;
    private bool isInputCooldown = false;

    [SerializeField] private float inputCooldownTime = 0.4f;

    [SerializeField] private SelectedPlayerJob job;

    private void Start()
    {
        select.sprite = jobImages[jobNum];
        UpdateArrowVisibility();
    }

    private void Update()
    {
        if (isInputCooldown || !pc.isActive) return;

        string check = playerNumber == 0 ? "Horizontal" : "Horizontal2";
        string submit = playerNumber == 0 ? "Submit" : "Submit2";
        string cancel = playerNumber == 0 ? "Cancel" : "Cancel2";
        float horizontal = Input.GetAxis(check);

        // 左入力
        if (horizontal < -0.5f)
        {
            if (isSelect || jobNum == 0) return;
            jobNum--;
            ChangeJobAsync().Forget();
        }
        // 右入力
        else if (horizontal > 0.5f)
        {
            if (isSelect || jobNum == jobImages.Length - 1) return;
            jobNum++;
            ChangeJobAsync().Forget();
        }

        // 決定
        if (Input.GetButtonDown(submit))
        {
            if (!isSelect)
            {
                isSelect = true;
                pc.playerReady[playerNumber] = true;
                //Debug.Log($"{playerNumber + 1}P 準備完了: {jobImages[jobNum].name}");
                foreach (Image img in arrows)
                    img.enabled = false;
            }
        }

        // キャンセル
        //if (Input.GetButtonDown(cancel))
        //{
        //    if (!pc.playerReady[playerNumber]) return;

        //    isSelect = false;
        //    pc.playerReady[playerNumber] = false;
        //    foreach (Image img in arrows)
        //        img.enabled = true;

        //    Debug.Log($"{playerNumber + 1}P キャンセル");
        //}
    }

    private async UniTask ChangeJobAsync()
    {
        isInputCooldown = true;
        select.transform.DOScale(1.2f, 0.1f).SetEase(Ease.OutQuad);
        await UniTask.Delay(100);
        select.sprite = jobImages[jobNum];
        selectJobName.sprite=jobNames[jobNum];
        select.transform.DOScale(1f, 0.2f).SetEase(Ease.OutBack);
        selectJobName.transform.DOScale(1f, 0.2f).SetEase(Ease.OutBack);
        job.playerJobName = jobImages[jobNum].name;
        UpdateArrowVisibility();
        await UniTask.Delay((int)(inputCooldownTime * 1000));
        isInputCooldown = false;
    }

    private void UpdateArrowVisibility()
    {
        // 左端・右端なら矢印非表示
        arrows[0].enabled = jobNum > 0;
        arrows[1].enabled = jobNum < jobImages.Length - 1;
    }
}
