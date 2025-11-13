using System.Collections;
using UnityEngine;
using TMPro;

public class PoisonMagic : MonoBehaviour
{
    [SerializeField]
    private float stretchDuration = 5.0f; // カウントダウン時間

    [SerializeField]
    private TextMeshPro countdownTMP; // 3Dテキスト参照

    private Vector3 initialScale;
    private Vector3 initialPosition;

    private PoisonZone zone; // Zone 参照
    private MoveForwardAndDestroy root; // おおもと参照

    void Awake()
    {
        initialScale = transform.localScale;
        initialPosition = transform.position;
    }

    void Start()
    {
        // Zone と Root を取得しておく（まだ開始はしない）
        zone = GetComponentInChildren<PoisonZone>(includeInactive: true);
        root = GetComponentInParent<MoveForwardAndDestroy>();

        if (zone != null && root != null)
        {
            zone.SetRoot(root);
        }

        // カウントダウン開始
        StartCoroutine(StretchAndCountdownCoroutine());
    }

    IEnumerator StretchAndCountdownCoroutine()
    {
        float elapsed = 0f;
        int startCount = Mathf.RoundToInt(stretchDuration);
        int currentCount = startCount;

        while (elapsed < stretchDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / stretchDuration);

            // 残り時間のカウント更新
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

        // カウント終了後 
        if (countdownTMP != null) 
        { 
            countdownTMP.text = "0"; yield return new WaitForSeconds(0.5f); 
            countdownTMP.gameObject.SetActive(false); 
        } 
        
        // カウント終了後にZoneを起動！
         if (zone != null) 
            zone.gameObject.SetActive(true); 
         // 非アクティブなら有効化
         zone.StartZone(); // Zoneの処理をスタート } }
    }
}
