using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Mixer / Groups")]
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private AudioMixerGroup bgmGroup;
    [SerializeField] private AudioMixerGroup sfxGroup;

    [Header("Sources")]
    [SerializeField] private AudioSource bgmSourceA;
    [SerializeField] private AudioSource bgmSourceB;
    [SerializeField] private int sfxPoolSize = 8;
    private readonly List<AudioSource> sfxPool = new();

    [Header("Default Volumes (0-1)")]
    [Range(0f, 1f)] public float defaultMaster = 1f;
    [Range(0f, 1f)] public float defaultBGM = 0.8f;
    [Range(0f, 1f)] public float defaultSFX = 0.9f;

    private const string KEY_MASTER = "VOL_MASTER";
    private const string KEY_BGM = "VOL_BGM";
    private const string KEY_SFX = "VOL_SFX";

    private bool usingA = true;
    private CancellationTokenSource fadeCts;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Setup
        if (bgmSourceA == null) bgmSourceA = gameObject.AddComponent<AudioSource>();
        if (bgmSourceB == null) bgmSourceB = gameObject.AddComponent<AudioSource>();
        SetupBgmSource(bgmSourceA);
        SetupBgmSource(bgmSourceB);

        for (int i = 0; i < sfxPoolSize; i++)
        {
            var s = new GameObject($"SFX_{i}").AddComponent<AudioSource>();
            s.transform.SetParent(transform);
            s.playOnAwake = false;
            s.outputAudioMixerGroup = sfxGroup;
            s.spatialBlend = 0f;
            sfxPool.Add(s);
        }

        LoadVolumes();
        Debug.Log(GetBGMVolume());
        Debug.Log(GetMasterVolume());
        Debug.Log(GetSFXVolume());
    }

    private void SetupBgmSource(AudioSource src)
    {
        src.playOnAwake = false;
        src.loop = true;
        src.outputAudioMixerGroup = bgmGroup;
        src.spatialBlend = 0f;
    }

    //  Volume Control
    public void SetMasterVolume(float linear, bool save = true)
        => SetMixerLinear(mixer, "MasterVolume", linear, KEY_MASTER, save);

    public void SetBGMVolume(float linear, bool save = true)
        => SetMixerLinear(mixer, "BGMVolume", linear, KEY_BGM, save);

    public void SetSFXVolume(float linear, bool save = true)
        => SetMixerLinear(mixer, "SFXVolume", linear, KEY_SFX, save);

    private void SetMixerLinear(AudioMixer mix, string param, float linear, string key, bool save)
    {
        linear = Mathf.Clamp01(linear);
        float dB = (linear <= 0.0001f) ? -80f : Mathf.Log10(linear) * 20f;
        mix.SetFloat(param, dB);
        if (save)
        {
            PlayerPrefs.SetFloat(key, linear);
            PlayerPrefs.Save();
        }
    }

    private void LoadVolumes()
    {
        SetMasterVolume(PlayerPrefs.GetFloat(KEY_MASTER, defaultMaster), false);
        SetBGMVolume(PlayerPrefs.GetFloat(KEY_BGM, defaultBGM), false);
        SetSFXVolume(PlayerPrefs.GetFloat(KEY_SFX, defaultSFX), false);
    }

    //  BGM
    public async UniTask PlayBGM(AudioClip clip, float fadeTime = 0.75f, bool loop = true, float pitch = 1f)
    {
        if (clip == null) return;

        var cur = usingA ? bgmSourceA : bgmSourceB;
        var next = usingA ? bgmSourceB : bgmSourceA;

        next.clip = clip;
        next.loop = loop;
        next.pitch = pitch;

        fadeCts?.Cancel();
        fadeCts = new CancellationTokenSource();

        await CrossFadeAsync(cur, next, fadeTime, fadeCts.Token);
        usingA = !usingA;
    }
    public async UniTask StopBGM(float fadeOut = 0.5f)
    {
        var cur = usingA ? bgmSourceA : bgmSourceB;
        fadeCts?.Cancel();
        fadeCts = new CancellationTokenSource();

        await FadeOutAsync(cur, fadeOut, fadeCts.Token);
    }
    private async UniTask CrossFadeAsync(AudioSource from, AudioSource to, float time, CancellationToken token)
    {
        to.volume = 0f;
        to.Play();

        float elapsed = 0f;
        while (elapsed < time)
        {
            if (token.IsCancellationRequested) return;
            elapsed += Time.unscaledDeltaTime;
            float k = time <= 0f ? 1f : elapsed / time;
            from.volume = 1f - k;
            to.volume = k;
            await UniTask.Yield(PlayerLoopTiming.Update, token);
        }

        from.Stop();
        from.volume = 1f;
        to.volume = 1f;
    }

    private async UniTask FadeOutAsync(AudioSource src, float time, CancellationToken token)
    {
        float start = src.volume;
        float elapsed = 0f;

        while (elapsed < time)
        {
            if (token.IsCancellationRequested) return;
            elapsed += Time.unscaledDeltaTime;
            float k = time <= 0f ? 1f : elapsed / time;
            src.volume = Mathf.Lerp(start, 0f, k);
            await UniTask.Yield(PlayerLoopTiming.Update, token);
        }

        src.Stop();
        src.volume = 1f;
    }

    //  SFX
    public void PlaySFX(AudioClip clip, float volumeScale = 1f, float pitch = 1f)
    {
        if (clip == null) return;
        var src = GetFreeSfxSource();
        src.pitch = pitch;
        src.PlayOneShot(clip, Mathf.Clamp01(volumeScale));
    }

    public async UniTask PlaySFXAtAsync(AudioClip clip, Vector3 pos, float volumeScale = 1f, float pitch = 1f)
    {
        if (clip == null) return;

        var go = new GameObject("SFX_OneShot3D");
        go.transform.position = pos;
        var src = go.AddComponent<AudioSource>();
        src.clip = clip;
        src.outputAudioMixerGroup = sfxGroup;
        src.spatialBlend = 1f;
        src.pitch = pitch;
        src.Play();

        await UniTask.Delay(TimeSpan.FromSeconds(clip.length / Mathf.Max(0.01f, pitch)));
        Destroy(go);
    }

    private AudioSource GetFreeSfxSource()
    {
        foreach (var s in sfxPool)
            if (!s.isPlaying) return s;
        return sfxPool[0];
    }

    public float GetMasterVolume() => PlayerPrefs.GetFloat(KEY_MASTER, defaultMaster);
    public float GetBGMVolume() => PlayerPrefs.GetFloat(KEY_BGM, defaultBGM); 
    public float GetSFXVolume() => PlayerPrefs.GetFloat(KEY_SFX, defaultSFX);
}
