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

    private float elapsedTime = 0f;

    void Update()
    {
        if (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            UpdateUI();
        }

        if (elapsedTime >= duration)
        {
            // ゴール処理
            SceneManager.LoadScene("Result");
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
