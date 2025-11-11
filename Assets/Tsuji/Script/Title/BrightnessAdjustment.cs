using UnityEngine;
using UnityEngine.SceneManagement;

public class BrightnessAdjustment : MonoBehaviour
{
    public static BrightnessAdjustment Instance { get; private set; }

    [SerializeField] private Light lightSource;

    private float currentBrightness;
    private float targetBrightness;

    private const string KEY_BRIGHT = "BRIGHTNESS";

    void Awake()
    {
        // --- シングルトン ---
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // 保存された明るさを読み込む
            targetBrightness = PlayerPrefs.GetFloat(KEY_BRIGHT, 1f);
            currentBrightness = targetBrightness;

            if (lightSource != null)
                lightSource.intensity = currentBrightness;

            // シーンがロードされた時にライトを再取得
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //// 新しいシーンでライトを探す（必要なら）
        if (lightSource == null)
            lightSource = FindAnyObjectByType<Light>();

        if (lightSource != null)
            lightSource.intensity = targetBrightness;
    }

    void Update()
    {
        // 明るさを滑らかに補間
        currentBrightness = Mathf.Lerp(currentBrightness, targetBrightness, Time.deltaTime * 3f);

        if (lightSource != null)
            lightSource.intensity = currentBrightness;
    }

    /// <summary>
    /// スライダーなどから新しい明るさを設定
    /// </summary>
    public void SetBrightness(float newValue)
    {
        targetBrightness = newValue;
        PlayerPrefs.SetFloat(KEY_BRIGHT, targetBrightness);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// 現在の明るさ値を取得
    /// </summary>
    public float GetBrightness() => targetBrightness;
}
