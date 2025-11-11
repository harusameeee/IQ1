using UnityEngine;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour
{
    public enum Target { Master, BGM, SFX, Brightness }
    [SerializeField] private Target target;
    [SerializeField] private Slider slider;


    void Start()
    {
        if (slider == null) slider = GetComponent<Slider>();

        // 音量スライダーの場合
        if (target != Target.Brightness)
        {
            slider.minValue = 0f;
            slider.maxValue = 1f;
        }
        else
        {
            // 明るさスライダーの場合
            slider.minValue = 0f;
            slider.maxValue = 1f; // BrightnessAdjustment側で *10 してるので0-1でOK
        }

        // ===== 初期値をロードして反映 =====
        switch (target)
        {
            case Target.Master:
                slider.value = SoundManager.Instance.GetMasterVolume();
                break;
            case Target.BGM:
                slider.value = SoundManager.Instance.GetBGMVolume();
                break;
            case Target.SFX:
                slider.value = SoundManager.Instance.GetSFXVolume();
                break;
            case Target.Brightness:
                slider.value = BrightnessAdjustment.Instance.GetBrightness();
                break;
        }

        // ===== 変更検知 =====
        slider.onValueChanged.AddListener(OnChanged);
    }

    private void OnChanged(float value)
    {
        switch (target)
        {
            case Target.Master:
                SoundManager.Instance.SetMasterVolume(value);
                break;
            case Target.BGM:
                SoundManager.Instance.SetBGMVolume(value);
                break;
            case Target.SFX:
                SoundManager.Instance.SetSFXVolume(value);
                break;
            case Target.Brightness:
                BrightnessAdjustment.Instance.SetBrightness(value);
                break;
        }
    }
}
