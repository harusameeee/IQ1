using Cysharp.Threading.Tasks;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class JobSelect : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image select;
    [SerializeField] private Image selectJobName;
    [SerializeField] private Image[] arrows = new Image[2];

    [Header("Game Settings")]
    [SerializeField] private int playerNumber = 0;
    [SerializeField] private PlayerCheck playerCheck;
    [SerializeField] private SelectedPlayerJob job;
    [SerializeField] private JobExplanation jobExplanation;

    private bool isSelected = false;
    private bool isFlipped = false;
    private bool isInputCooldown = false;
    private int jobNum = 0;

    private const int MaxJobIndex = 3;
    private const float InputCooldownTime = 0.4f;

    private void Start()
    {
        playerCheck.InitStatus();
        UpdateUI();
        playerCheck.CheckSelect().Forget();
        playerCheck.CheckBothReady().Forget();
    }
    private void Update()
    {
        if (isInputCooldown || !playerCheck.isActive) return;
        
        string submit = playerNumber == 0 ? "Submit" : "Submit2";
        string detailBtn = playerNumber == 0 ? "Button_X1" : "Button_X2";
        string cancel = playerNumber == 0 ? "Cancel" : "Cancel2";

        HandleMove();
        if (Input.GetButtonDown(submit) || Input.GetKeyDown(KeyCode.Space)) HandleSelect();
        if (Input.GetButtonDown(detailBtn) || Input.GetKeyDown(KeyCode.V))  HandleDetail();
        if (Input.GetButtonDown(cancel) || Input.GetKeyDown(KeyCode.Backspace)) HandleCancel();
    }

    // 操作処理
    private void HandleMove()
    {
        string axis = playerNumber == 0 ? "Horizontal" : "Horizontal2";
        float horizontal = Input.GetAxis(axis);

        if (isSelected) return;
        if (horizontal < -0.5f||Input.GetKeyDown(KeyCode.A)) ChangeJob(-1);
        if (horizontal > 0.5f || Input.GetKeyDown(KeyCode.D)) ChangeJob(1);
    }
    private void HandleSelect()
    {
        if (!isSelected && !playerCheck.playerReady[playerNumber])
        {
            isSelected = true;
            playerCheck.PlayerReady(playerNumber, true);
            SetArrows(false);
        }
    }

    private void HandleDetail()
    {
        isFlipped = !isFlipped;
        jobExplanation.TurnOverImage(!isFlipped);
    }
    private void HandleCancel()
    {
        // Ready解除
        if (playerCheck.playerReady[playerNumber])
        {
            playerCheck.PlayerReady(playerNumber, false);
            isSelected = false;
            SetArrows(true);
            ResetScale();
        }
        // 全員未準備 → 全キャンセル
        else if (!playerCheck.playerReady[0] && !playerCheck.playerReady[1])
        {
            playerCheck.CancelAll();
        }
        // 裏返し表示を戻す
        if (isFlipped)
        {
            isFlipped = false;
            jobExplanation.TurnOverImage(true);
        }
    }

    // UI演出
    private async void ChangeJob(int direction)
    {
        isInputCooldown = true;
        jobNum = Mathf.Clamp(jobNum + direction, 0, MaxJobIndex);

        AnimateSelection();
        UpdateUI();

        await UniTask.Delay((int)(InputCooldownTime * 1000));
        isInputCooldown = false;
    }
    private void AnimateSelection()
    {
        select.transform.DOScale(1.2f, 0.1f).SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                select.transform.DOScale(1f, 0.2f).SetEase(Ease.OutBack);
                selectJobName.transform.DOScale(1f, 0.2f).SetEase(Ease.OutBack);
            });
    }

    private void ResetScale()
    {
        select.transform.DOScale(1f, 0.2f);
        selectJobName.transform.DOScale(1f, 0.2f);
    }

    // 表示更新
    private void UpdateUI()
    {
        jobExplanation.ChangeJobImage(jobNum);
        job.playerJobName = jobExplanation.GetJobName(jobNum);
        UpdateArrowVisibility();
    }

    private void UpdateArrowVisibility()
    {
        arrows[0].enabled = jobNum > 0;
        arrows[1].enabled = jobNum < MaxJobIndex;
    }

    private void SetArrows(bool state)
    {
        foreach (var arrow in arrows)
            arrow.enabled = state;
    }
}
