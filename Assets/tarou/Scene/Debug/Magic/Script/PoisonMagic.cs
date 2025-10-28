using System.Collections;
using UnityEngine;
using TMPro;

public class PoisonMagic : MonoBehaviour
{
    [SerializeField]
    private float stretchDuration = 5.0f; // 伸びにかける時間（秒）※5秒なら5→0ぴったり


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
       
        int startCount = 5; // カウント開始値
        int currentCount = startCount;

        while (elapsed < stretchDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / stretchDuration);
          
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


        // カウントを0にしてテキストを消す
        if (countdownTMP != null)
        {
            countdownTMP.text = "0";
            yield return new WaitForSeconds(0.5f); // 少し間をおいて非表示に
            countdownTMP.gameObject.SetActive(false);
        }
    }
}
