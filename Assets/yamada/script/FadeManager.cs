using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// シーン遷移時のフェードイン・アウトを制御するためのクラス .
/// </summary>
public class FadeManager : MonoBehaviour
{
    [Tooltip("シーン遷移でフェードを入れるシーンの名前を入力")]
    [Header("Scene Settings")]
    [Space(10)]

    [SerializeField]
    private string m_SceneName1;

    [SerializeField]
    private string m_SceneName2;

    [SerializeField]
    private string m_SceneName3;

    [SerializeField]
    private string m_SceneName4;

    [SerializeField, Range(0.0f, 1.5f)]
    private float m_FadeTime;

    [Space(5)]
    [SerializeField]
    private Transform m_MaskFadeTrans;

    // 新しく追加：scale を変更するための最大/最小値（Inspectorで設定可）
    [SerializeField, Tooltip("Scale の最大値（例: 30）")]
    private float m_MaxScale = 30f;

    [SerializeField, Tooltip("Scale の最小値（例: 0）")]
    private float m_MinScale = 0f;

    // m_MaskFadeTrans がシーン切り替えで破棄されたときに
    // 再検索するために元のオブジェクト名を保持しておく
    private string m_MaskFadeTransName = "";

    #region Singleton

    private static FadeManager instance;

    public static FadeManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (FadeManager)FindObjectOfType(typeof(FadeManager));

                if (instance == null)
                {
                    Debug.LogError(typeof(FadeManager) + "is nothing");
                }
            }

            return instance;
        }
    }

    #endregion Singleton

    /// <summary>
    /// デバッグモード .
    /// </summary>
    public bool DebugMode = true;
    /// <summary>フェード中の透明度</summary>
   // private float fadeAlpha = 0;
    /// <summary>フェード中かどうか</summary>
    private bool isFading = false;
    /// <summary>フェード色</summary>
   // public Color fadeColor = Color.black;


    public void Awake()
    {
        if (this != Instance)
        {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);

        // m_MaskFadeTrans がセットされていれば名前を保持しておく
        if (m_MaskFadeTrans != null)
        {
            m_MaskFadeTransName = m_MaskFadeTrans.gameObject.name;
        }
    }

    /// <summary>
    ///　デバッグ用GUI
    /// </summary>
    public void OnGUI()
    {
        if (this.DebugMode)
        {
            if (!this.isFading)
            {
                //Scene一覧を作成 .
                //(UnityEditor名前空間を使わないと自動取得できなかったので決めうちで作成) .
                List<string> scenes = new List<string>();
                scenes.Add(m_SceneName1);
                //scenes.Add ("SomeScene2");


                //Sceneが一つもない .
                if (scenes.Count == 0)
                {
                    GUI.Box(new Rect(10, 10, 200, 50), "Fade Manager(Debug Mode)");
                    GUI.Label(new Rect(20, 35, 180, 20), "Scene not found.");
                    return;
                }


                GUI.Box(new Rect(10, 10, 300, 50 + scenes.Count * 25), "Fade Manager(Debug Mode)");
                GUI.Label(new Rect(20, 30, 280, 20), "Current Scene : " + SceneManager.GetActiveScene().name);

                int i = 0;
                foreach (string sceneName in scenes)
                {
                    if (GUI.Button(new Rect(20, 55 + i * 25, 100, 20), "Next Scene"))
                    {
                        LoadScene(1/*m_NextSceneName, 1.0f*/);
                    }
                    GUI.Label(new Rect(125, 55 + i * 25, 1000, 20), sceneName);
                    i++;
                }
            }
        }
    }

    /// <summary>
    /// 画面遷移 .
    /// </summary>
    /// <param name='scene'>シーン名</param>
    /// <param name='interval'>暗転にかかる時間(秒)</param>
    public void LoadScene(int sceneNumber/*string scene, float interval*/)
    {
        switch (sceneNumber)
        {
            case 1:
                {
                    StartCoroutine(TransScene(m_SceneName1, m_FadeTime));
                    break;
                }
            case 2:
                {
                    StartCoroutine(TransScene(m_SceneName2, m_FadeTime));
                    break;
                }
            case 3:
                {
                    StartCoroutine(TransScene(m_SceneName3, m_FadeTime));
                    break;
                }
            case 4:
                {
                    StartCoroutine(TransScene(m_SceneName4, m_FadeTime));
                    break;
                }
            default:
                break;
        }
    }


    /// <summary>
    /// シーン遷移用コルーチン .
    /// </summary>
    /// <param name='scene'>シーン名</param>
    /// <param name='interval'>暗転にかかる時間(秒)</param>
    private IEnumerator TransScene(string scene, float interval)
    {
        this.isFading = true;
        float duration = Mathf.Max(0.01f, interval);

        // --- 暗転（だんだん小さく） ---
        float time = 0;
        if (m_MaskFadeTrans != null)
        {
            float startScale = m_MaxScale;
            float endScale = m_MinScale;
            while (time <= duration)
            {
                if (m_MaskFadeTrans != null && m_MaskFadeTrans.gameObject != null)
                {
                    float t = time / duration;
                    float s = Mathf.Lerp(startScale, endScale, t);
                    m_MaskFadeTrans.localScale = Vector3.one * s;
                }
                time += Time.deltaTime;
                yield return null;
            }
            if (m_MaskFadeTrans != null && m_MaskFadeTrans.gameObject != null)
                m_MaskFadeTrans.localScale = Vector3.one * endScale;
        }

        // --- シーン切替 ---
        SceneManager.LoadScene(scene);

        // --- シーン切替後: 新しいUnMaskのRectTransformを再検索 ---
        m_MaskFadeTrans = null;
        float timeout = 2.0f;
        float elapsed = 0f;
        while (m_MaskFadeTrans == null && elapsed < timeout)
        {
            //GameObject found = GameObject.Find("Canvas/Mask/UnMask"); // 今回の階層名に合わせて
            GameObject found = GameObject.FindWithTag("UnMask");
            if (found != null)
            {
                m_MaskFadeTrans = found.GetComponent<RectTransform>();
                // フェード直前なら初期scaleもこのタイミングで設定
                m_MaskFadeTrans.localScale = Vector3.one * m_MinScale;
                break;
            }
            elapsed += Time.deltaTime;
            yield return null;
        }

        // --- 明転（だんだん大きく） ---
        time = 0;
        if (m_MaskFadeTrans != null && m_MaskFadeTrans.gameObject != null)
        {
            float startScale = m_MinScale;
            float endScale = m_MaxScale;
            while (time <= duration)
            {
                float t = time / duration;
                float s = Mathf.Lerp(startScale, endScale, t);
                m_MaskFadeTrans.localScale = Vector3.one * s;
                time += Time.deltaTime;
                yield return null;
            }
            m_MaskFadeTrans.localScale = Vector3.one * endScale;
        }

        this.isFading = false;
    }
}