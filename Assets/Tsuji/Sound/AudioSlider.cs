using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour
{
    public enum Target { Master, BGM, SFX }
    [SerializeField] private Target target;
    [SerializeField] private Slider slider;

    void Awake()
    {
        if (slider == null) slider = GetComponent<Slider>();
        slider.minValue = 0f;
        slider.maxValue = 1f;

        // ‰Šú’l‚ğƒ[ƒh‚µ‚Ä”½‰f
        switch (target)
        {
            case Target.Master: slider.value = SoundManager.Instance.GetMasterVolume(); break;
            case Target.BGM: slider.value = SoundManager.Instance.GetBGMVolume(); break;
            case Target.SFX: slider.value = SoundManager.Instance.GetSFXVolume(); break;
        }

        slider.onValueChanged.AddListener(OnChanged);
    }

    void OnChanged(float v)
    {
        switch (target)
        {
            case Target.Master: SoundManager.Instance.SetMasterVolume(v); break;
            case Target.BGM: SoundManager.Instance.SetBGMVolume(v); break;
            case Target.SFX: SoundManager.Instance.SetSFXVolume(v); break;
        }
    }
}
