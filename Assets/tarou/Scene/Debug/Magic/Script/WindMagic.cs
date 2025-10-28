using System.Collections;
using UnityEngine;
using TMPro;

public class WindMagic : MonoBehaviour
{
    [SerializeField]
    private float stretchDuration = 5.0f; // 伸びにかける時間（秒）※5秒なら5→0ぴったり

    [SerializeField]
    private float targetLength = 25.0f; // 最終的な長さ（Z）

    [SerializeField]
    private TextMeshPro countdownTMP; // 3D TMP テキスト参照

    private Vector3 initialScale;
    private Vector3 initialPosition;

    void Awake()
    {
        initialScale = transform.localScale;
        initialPosition = transform.position;
    }

    void Start()
    {
        StartCoroutine(StretchAndCountdownCoroutine());
    }

    IEnumerator StretchAndCountdownCoroutine()
    {
        float elapsed = 0f;
        float startLength = initialScale.z;
        float delta = targetLength - startLength;

        int startCount = 5; // カウント開始値
        int currentCount = startCount;

        while (elapsed < stretchDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / stretchDuration);
            float currentLength = Mathf.Lerp(startLength, targetLength, t);

            // スケールと位置を更新
            transform.localScale = new Vector3(initialScale.x, initialScale.y, currentLength);
            transform.position = initialPosition + new Vector3(0, 0, (currentLength - startLength) / 2f);

            // 残り時間に応じてカウント更新
            float remaining = Mathf.Lerp(startCount, 0, t);
            int newCount = Mathf.CeilToInt(remaining);

            if (newCount != currentCount)
            {
                currentCount = newCount;
                if (countdownTMP != null)
                    countdownTMP.text = currentCount.ToString();
            }

            yield return null;
        }

        // 最終値補正
        transform.localScale = new Vector3(initialScale.x, initialScale.y, targetLength);
        transform.position = initialPosition + new Vector3(0, 0, delta / 2f);

        // カウントを0にしてテキストを消す
        if (countdownTMP != null)
        {
            countdownTMP.text = "0";
            yield return new WaitForSeconds(0.5f); // 少し間をおいて非表示に
            countdownTMP.gameObject.SetActive(false);
        }
    }
}
