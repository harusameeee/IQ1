using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    // ===== Singleton =====
    public static SoundManager Instance { get; private set; }

    [Header("Mixer / Groups")]
    [SerializeField] private AudioMixer mixer;                 // Masterミキサー
    [SerializeField] private AudioMixerGroup bgmGroup;         // BGM用グループ
    [SerializeField] private AudioMixerGroup sfxGroup;         // SFX用グループ

    [Header("Sources")]
    [SerializeField] private AudioSource bgmSourceA;           // クロスフェード用2系統
    [SerializeField] private AudioSource bgmSourceB;
    [SerializeField] private int sfxPoolSize = 8;              // 同時再生数
    private readonly List<AudioSource> sfxPool = new();

    [Header("Default Volumes (0-1)")]
    [Range(0f, 1f)] public float defaultMaster = 1f;
    [Range(0f, 1f)] public float defaultBGM = 0.8f;
    [Range(0f, 1f)] public float defaultSFX = 0.9f;

    // PlayerPrefs Keys
    const string KEY_MASTER = "VOL_MASTER";
    const string KEY_BGM = "VOL_BGM";
    const string KEY_SFX = "VOL_SFX";

    // State
    private bool usingA = true; // 今鳴っているBGMソース
    private Coroutine bgmFadeCo;

    void Awake()
    {
        // --- Singleton & 永続化 ---
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // --- オーディオソース準備 ---
        if (bgmSourceA == null) bgmSourceA = gameObject.AddComponent<AudioSource>();
        if (bgmSourceB == null) bgmSourceB = gameObject.AddComponent<AudioSource>();
        SetupBgmSource(bgmSourceA);
        SetupBgmSource(bgmSourceB);

        // SFXプール
        for (int i = 0; i < sfxPoolSize; i++)
        {
            var s = new GameObject($"SFX_{i}").AddComponent<AudioSource>();
            s.transform.SetParent(transform);
            s.playOnAwake = false;
            s.outputAudioMixerGroup = sfxGroup;
            s.spatialBlend = 0f; // UI/2D想定
            sfxPool.Add(s);
        }

        // --- 音量ロード ---
        float master = PlayerPrefs.GetFloat(KEY_MASTER, defaultMaster);
        float bgm = PlayerPrefs.GetFloat(KEY_BGM, defaultBGM);
        float sfx = PlayerPrefs.GetFloat(KEY_SFX, defaultSFX);

        SetMasterVolume(master, save: false);
        SetBGMVolume(bgm, save: false);
        SetSFXVolume(sfx, save: false);
    }

    private void SetupBgmSource(AudioSource src)
    {
        src.playOnAwake = false;
        src.loop = true;
        src.outputAudioMixerGroup = bgmGroup;
        src.spatialBlend = 0f; // 2D
    }

    // ====== Volume API (0.0〜1.0) ======
    public void SetMasterVolume(float linear, bool save = true)
        => SetMixerLinear(mixer, "MasterVolume", linear, KEY_MASTER, save);

    public void SetBGMVolume(float linear, bool save = true)
        => SetMixerLinear(mixer, "BGMVolume", linear, KEY_BGM, save);

    public void SetSFXVolume(float linear, bool save = true)
        => SetMixerLinear(mixer, "SFXVolume", linear, KEY_SFX, save);

    private void SetMixerLinear(AudioMixer mix, string param, float linear, string saveKey, bool save)
    {
        linear = Mathf.Clamp01(linear);
        // 線形(0-1) → dB(-80〜0)
        float dB = (linear <= 0.0001f) ? -80f : Mathf.Log10(linear) * 20f;
        mix.SetFloat(param, dB);
        if (save)
        {
            PlayerPrefs.SetFloat(saveKey, linear);
            PlayerPrefs.Save();
        }
    }

    public float GetMasterVolume() => PlayerPrefs.GetFloat(KEY_MASTER, defaultMaster);
    public float GetBGMVolume() => PlayerPrefs.GetFloat(KEY_BGM, defaultBGM);
    public float GetSFXVolume() => PlayerPrefs.GetFloat(KEY_SFX, defaultSFX);

    // ====== BGM ======
    public void PlayBGM(AudioClip clip, float fadeTime = 0.75f, bool loop = true, float pitch = 1f)
    {
        if (clip == null) return;

        var cur = usingA ? bgmSourceA : bgmSourceB;
        var next = usingA ? bgmSourceB : bgmSourceA;

        next.clip = clip;
        next.loop = loop;
        next.pitch = pitch;

        if (bgmFadeCo != null) StopCoroutine(bgmFadeCo);
        bgmFadeCo = StartCoroutine(CrossFade(cur, next, fadeTime));
        usingA = !usingA;
    }

    public void StopBGM(float fadeOut = 0.5f)
    {
        var cur = usingA ? bgmSourceA : bgmSourceB;
        if (bgmFadeCo != null) StopCoroutine(bgmFadeCo);
        bgmFadeCo = StartCoroutine(FadeOutAndStop(cur, fadeOut));
    }

    private IEnumerator CrossFade(AudioSource from, AudioSource to, float time)
    {
        to.volume = 0f;
        to.Play();

        float t = 0f;
        while (t < time)
        {
            t += Time.unscaledDeltaTime;
            float k = time <= 0f ? 1f : t / time;
            from.volume = 1f - k;
            to.volume = k;
            yield return null;
        }
        from.Stop();
        from.volume = 1f;
        to.volume = 1f;
    }

    private IEnumerator FadeOutAndStop(AudioSource src, float time)
    {
        float start = src.volume;
        float t = 0f;
        while (t < time)
        {
            t += Time.unscaledDeltaTime;
            float k = time <= 0f ? 1f : t / time;
            src.volume = Mathf.Lerp(start, 0f, k);
            yield return null;
        }
        src.Stop();
        src.volume = 1f;
    }

    // ====== SFX ======
    public void PlaySFX(AudioClip clip, float volumeScale = 1f, float pitch = 1f)
    {
        if (clip == null) return;
        var src = GetFreeSfxSource();
        src.pitch = pitch;
        src.PlayOneShot(clip, Mathf.Clamp01(volumeScale));
    }

    public void PlaySFXAt(AudioClip clip, Vector3 worldPos, float volumeScale = 1f, float pitch = 1f)
    {
        if (clip == null) return;
        var go = new GameObject("SFX_OneShot3D");
        go.transform.position = worldPos;
        var src = go.AddComponent<AudioSource>();
        src.clip = clip;
        src.spatialBlend = 1f; // 3D
        src.outputAudioMixerGroup = sfxGroup;
        src.pitch = pitch;
        src.Play();
        Destroy(go, clip.length / Mathf.Max(0.01f, pitch));
    }

    private AudioSource GetFreeSfxSource()
    {
        foreach (var s in sfxPool)
            if (!s.isPlaying) return s;
        // 全部埋まってたら先頭を再利用
        return sfxPool[0];
    }
}
