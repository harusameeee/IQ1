using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ExitProgressUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image progressFill;          // 青い進捗バー
    [SerializeField] private RectTransform playerIcon;    // 人アイコン
    [SerializeField] private TextMeshProUGUI distanceText;// 残り距離表示

    [Header("設定")]
    [SerializeField] private float totalDistance = 1000f; // 初期距離
    [SerializeField] private float duration = 120f;       // 0m までにかかる時間(秒)

    [Header("プレイヤー参照")]
    public Player3DController player; // State参照用
    public Player3DController player2; // State参照用

    private float elapsedTime = 0f;

    void Update()
    {
        if (elapsedTime < duration)
        {
            float timeScale = 1f;

            // State.Slow なら timeScale = 0.5f
            if ((player != null && player.currentState == Player3DController.State.Slow) ||
                (player2 != null && player2.currentState == Player3DController.State.Slow))
            {
                timeScale = 0.5f;
            }

            elapsedTime += Time.deltaTime * timeScale;
            UpdateUI();
        }

        if (elapsedTime >= duration)
        {
            // ゴール処理
            SceneManager.LoadScene("ResultScene");
        }
    }

    void UpdateUI()
    {
        // 残り距離を時間経過で計算
        float remainingDistance = Mathf.Lerp(totalDistance, 0, elapsedTime / duration);

        // 進捗率
        float progress = Mathf.Clamp01(1f - (remainingDistance / totalDistance));
        progressFill.rectTransform.localScale = new Vector3(progress, 1, 1);

        // アイコンの位置を進捗バーに合わせる
        float barWidth = ((RectTransform)progressFill.transform.parent).rect.width;
        Vector3 pos = playerIcon.localPosition;
        pos.x = -barWidth / 2f + (barWidth * progress);
        playerIcon.localPosition = pos;

        // テキスト更新
        distanceText.text = $"<b>{Mathf.CeilToInt(remainingDistance)} m</b>";
    }
}
