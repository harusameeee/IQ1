using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// シーン遷移時および任意条件でのフェード（画面縁の逆マスクをスケールで操作）を制御するクラス。
/// ご要望に合わせて、シーン遷移時の挙動を次のようにしています：
///   フェードイン (暗転) : スケール最大値（初期） -> 0 に徐々に縮める
///   シーン切替
///   フェードアウト (復帰) : 0 -> スケール最大値 に徐々に拡大する
///
/// またデバッグ用の OnGUI と一時暗転 (StartTemporaryDark) 機能は維持しています。
/// </summary>
public class MaskFadeManager : MonoBehaviour
{
    [Header("Scene Settings")]
    [Tooltip("シーン遷移でフェードを入れるシーンの名前を入力")]
    [SerializeField] private string m_SceneName1;
    [SerializeField] private string m_SceneName2;
    [SerializeField] private string m_SceneName3;
    [SerializeField] private string m_SceneName4;

    [Header("Fade Settings")]
    [SerializeField, Range(0.01f, 5f)]
    private float m_FadeTime = 0.8f;

    [Header("Mask Settings")]
    [Tooltip("フェードで拡大/縮小する逆マスクの Transform (UI の RectTransform でも可) をセット")]
    [SerializeField] private Transform m_MaskTransform;

    [Tooltip("マスクの最大スケール (X, Y)。Zはマスクの現在値を維持します")]
    [SerializeField] private Vector2 m_MaxScale = Vector2.one;

    [Tooltip("マスクをシーン間で保持したい場合は true にする（FadeManager 自体は DontDestroyOnLoad です）")]
    [SerializeField] private bool m_PreserveMaskAcrossScenes = false;

    [Header("Behavior")]
    [Tooltip("true: スケールを増やす (0 -> max) と『画面が暗くなる (暗転)』と判定します。\nfalse: スケールを増やすと『明るくなる』と判定します。\nこのフラグは一時暗転などでの意味判定に影響します。")]
    [SerializeField] private bool IncreaseScaleMeansDark = true;

    #region Singleton
    private static MaskFadeManager instance;
    public static MaskFadeManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (MaskFadeManager)FindObjectOfType(typeof(MaskFadeManager));
                if (instance == null)
                {
                    Debug.LogError(typeof(MaskFadeManager) + " is nothing");
                }
            }
            return instance;
        }
    }
    #endregion

    /// <summary>デバッグモードで OnGUI にシーン選択ボタンを表示</summary>
    public bool DebugMode = true;

    /// <summary>フェード中かどうか（シーン遷移や一時暗転などを含む）</summary>
    private bool isFading = false;

    private void Awake()
    {
        if (this != Instance)
        {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);

        if (m_PreserveMaskAcrossScenes && m_MaskTransform != null)
        {
            DontDestroyOnLoad(m_MaskTransform.gameObject);
        }
    }

    public void OnGUI()
    {
        if (DebugMode)
        {
            if (!isFading)
            {
                List<string> scenes = new List<string>();
                if (!string.IsNullOrEmpty(m_SceneName1)) scenes.Add(m_SceneName1);
                if (!string.IsNullOrEmpty(m_SceneName2)) scenes.Add(m_SceneName2);
                if (!string.IsNullOrEmpty(m_SceneName3)) scenes.Add(m_SceneName3);
                if (!string.IsNullOrEmpty(m_SceneName4)) scenes.Add(m_SceneName4);

                if (scenes.Count == 0)
                {
                    GUI.Box(new Rect(10, 10, 220, 50), "Fade Manager(Debug Mode)");
                    GUI.Label(new Rect(20, 35, 200, 20), "Scene not found.");
                    return;
                }

                GUI.Box(new Rect(10, 10, 300, 50 + scenes.Count * 25), "Fade Manager(Debug Mode)");
                GUI.Label(new Rect(20, 30, 280, 20), "Current Scene : " + SceneManager.GetActiveScene().name);

                int i = 0;
                foreach (string sceneName in scenes)
                {
                    if (GUI.Button(new Rect(20, 55 + i * 25, 100, 20), "Next Scene"))
                    {
                        LoadScene(i + 1);
                    }
                    GUI.Label(new Rect(125, 55 + i * 25, 160, 20), sceneName);
                    i++;
                }

                // デバッグ用に一時暗転トリガーボタンを追加
                if (GUI.Button(new Rect(20, 60 + scenes.Count * 25, 150, 24), "Temporary Dark (Demo)"))
                {
                    // デモ: toZero 0.6s, wait 2s, toMax 0.6s
                    StartTemporaryDark(0.6f, 2.0f, 0.6f);
                }
            }
            else
            {
                GUI.Box(new Rect(10, 10, 220, 40), "Fade Manager(Debug Mode)");
                GUI.Label(new Rect(20, 30, 200, 20), "Fading...");
            }
        }
    }

    /// <summary>
    /// シーン遷移呼び出し (1..4)
    /// </summary>
    public void LoadScene(int sceneNumber)
    {
        if (isFading) return;

        switch (sceneNumber)
        {
            case 1:
                StartCoroutine(TransScene(m_SceneName1, m_FadeTime));
                break;
            case 2:
                StartCoroutine(TransScene(m_SceneName2, m_FadeTime));
                break;
            case 3:
                StartCoroutine(TransScene(m_SceneName3, m_FadeTime));
                break;
            case 4:
                StartCoroutine(TransScene(m_SceneName4, m_FadeTime));
                break;
            default:
                Debug.LogWarning("Invalid scene number: " + sceneNumber);
                break;
        }
    }

    /// <summary>
    /// シーン遷移用フェード。
    /// ユーザ要望に合わせて、以下の流れにしました:
    ///   フェードイン (暗転) : max -> 0 に徐々に縮める
    ///   シーン切替
    ///   フェードアウト (復帰) : 0 -> max に徐々に拡大する
    /// IncreaseScaleMeansDark フラグは一時暗転等の意味判定に影響しますが、
    /// TransScene では上記固定の流れ (max->0 -> load -> 0->max) を使います。
    /// </summary>
    private IEnumerator TransScene(string scene, float interval)
    {
        if (string.IsNullOrEmpty(scene))
        {
            Debug.LogWarning("Scene name is empty. Abort TransScene.");
            yield break;
        }

        if (m_MaskTransform == null)
        {
            Debug.LogWarning("Mask Transform is not assigned. Performing instant scene load.");
            SceneManager.LoadScene(scene);
            yield break;
        }

        isFading = true;

        float z = m_MaskTransform.localScale.z;
        Vector3 maxScale = new Vector3(m_MaxScale.x, m_MaxScale.y, z);
        Vector3 zeroScale = new Vector3(0f, 0f, z);

        // 要望通り：最初は max を初期値として、max -> 0（暗転）
        // 注意: マスクの見た目によって「暗転」が逆に見える場合があるので、
        // m_MaxScale を設定した状態で挙動を確認してください。

        // 安全のため確実に max から開始
        m_MaskTransform.localScale = maxScale;

        // フェードイン（暗転）: max -> 0
        yield return StartCoroutine(LerpScale(m_MaskTransform, maxScale, zeroScale, interval));

        // シーン切替
        SceneManager.LoadScene(scene);

        // フレーム待ち（ロード後の初期化を待つ）
        yield return null;

        // フェードアウト（復帰）: 0 -> max
        yield return StartCoroutine(LerpScale(m_MaskTransform, zeroScale, maxScale, interval));

        isFading = false;
    }

    /// <summary>
    /// 指定の Transform を from -> to に duration 秒で補間するヘルパー（SmoothStep 使用）
    /// </summary>
    private IEnumerator LerpScale(Transform t, Vector3 from, Vector3 to, float duration)
    {
        if (t == null)
            yield break;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            float u = Mathf.Clamp01(elapsed / duration);
            float eva = Mathf.SmoothStep(0f, 1f, u);
            t.localScale = Vector3.LerpUnclamped(from, to, eva);
            elapsed += Time.deltaTime;
            yield return null;
        }
        t.localScale = to;
    }

    /// <summary>
    /// 公開メソッド: 「スケール最大値を初期値として、徐々に 0 にして一定時間経ったら最大へ戻す」
    /// （一時暗転用）
    /// - toZeroDuration: max -> 0 にするまでの時間
    /// - waitTime: 0 になってから待つ時間（秒）
    /// - toMaxDuration: 0 -> max に戻す時間
    /// </summary>
    public void StartTemporaryDark(float toZeroDuration, float waitTime, float toMaxDuration)
    {
        if (isFading)
        {
            Debug.Log("FadeManager: currently fading, StartTemporaryDark ignored.");
            return;
        }
        if (m_MaskTransform == null)
        {
            Debug.LogWarning("Mask Transform is not assigned. StartTemporaryDark ignored.");
            return;
        }

        float z = m_MaskTransform.localScale.z;
        Vector3 maxScale = new Vector3(m_MaxScale.x, m_MaxScale.y, z);
        Vector3 zeroScale = new Vector3(0f, 0f, z);

        // 要望どおり初期を max にセットしてから max->0 -> wait -> 0->max を実行
        m_MaskTransform.localScale = maxScale;
        StartCoroutine(TemporaryDarkCoroutine(maxScale, zeroScale, toZeroDuration, waitTime, toMaxDuration));
    }

    private IEnumerator TemporaryDarkCoroutine(Vector3 maxScale, Vector3 zeroScale, float toZeroDuration, float waitTime, float toMaxDuration)
    {
        isFading = true;

        // max -> 0
        yield return StartCoroutine(LerpScale(m_MaskTransform, maxScale, zeroScale, Mathf.Max(0.0001f, toZeroDuration)));

        // wait
        if (waitTime > 0f)
        {
            float waited = 0f;
            while (waited < waitTime)
            {
                waited += Time.deltaTime;
                yield return null;
            }
        }
       
        // 0 -> max
        yield return StartCoroutine(LerpScale(m_MaskTransform, zeroScale, maxScale, Mathf.Max(0.0001f, toMaxDuration)));

        isFading = false;
    }
}