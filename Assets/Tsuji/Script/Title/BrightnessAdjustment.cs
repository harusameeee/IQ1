using UnityEngine;

public class BrightnessAdjustment : MonoBehaviour
{
    public static BrightnessAdjustment Instance { get; private set; }

    [SerializeField] private Light lightSource;
    private float brightness = 1f;

    [Range(1f, 10f)]
    [SerializeField] private float defaultBrightness = 1f;

    private const string KEY_BRIGHT = "BRIGHTNESS";

    void Awake()
    {
        // --- シングルトン ---
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // 保存された明るさを読み込む
        defaultBrightness = PlayerPrefs.GetFloat(KEY_BRIGHT, defaultBrightness);
        brightness = defaultBrightness;

        if (lightSource != null)
            lightSource.intensity = brightness;
    }

    void Update()
    {
        // 明るさを滑らかに補間
        brightness = Mathf.Lerp(brightness, defaultBrightness, Time.deltaTime * 3f);

        if (lightSource != null)
            lightSource.intensity = brightness;
    }

    /// <summary>
    /// スライダーなどから新しい明るさを設定
    /// </summary>
    public void SetBrightness(float newValue)
    {
        defaultBrightness = newValue;
        PlayerPrefs.SetFloat(KEY_BRIGHT, defaultBrightness);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// 現在の明るさ値を取得
    /// </summary>
    public float GetBrightness() => PlayerPrefs.GetFloat(KEY_BRIGHT, defaultBrightness);
}
