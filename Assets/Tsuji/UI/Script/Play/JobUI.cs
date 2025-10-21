using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JobUI : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private float coolTime = 5f;   // クールタイムの長さ
    [SerializeField] private TextMeshProUGUI coolTimeText;

    private float currentTime;                      // 残り時間
    private bool isCoolingDown = false;             // クールタイム中かどうか

    void Start()
    {
        image.fillAmount = 1.0f;
        coolTimeText.enabled = false;

    }

    void Update()
    {
        // テスト用: スペースでクールタイム開始
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoolTime();
        }

        if (isCoolingDown)
        {
            currentTime -= Time.deltaTime;
            if (currentTime < 0) currentTime = 0;

            // fillAmountは0→1に戻っていく
            image.fillAmount = 1 - (currentTime / coolTime);

            // 残り秒数を表示
            coolTimeText.text = Mathf.CeilToInt(currentTime).ToString();

            if (currentTime <= 0)
            {
                EndCoolTime();
            }
        }
    }

    public void StartCoolTime()
    {
        isCoolingDown = true;
        currentTime = coolTime;
        coolTimeText.enabled = true;
        image.fillAmount = 0.0f;
        image.color = new Color(1, 1, 1, 0.5f); // 半透明に
    }

    private void EndCoolTime()
    {
        isCoolingDown = false;
        image.fillAmount = 1.0f;
        coolTimeText.enabled = false;
        image.color = new Color(1, 1, 1, 1.0f); // 元に戻す
    }
}
