using UnityEngine;
using UnityEngine.UI;

public class SettingsView : UIViewStackable
{
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Button closeButton;

    private void Awake()
    {
        if (bgmSlider != null)
            bgmSlider.onValueChanged.AddListener(OnBgmVolumeChanged);

        if (sfxSlider != null)
            sfxSlider.onValueChanged.AddListener(OnSfxVolumeChanged);

        if (closeButton != null)
            closeButton.onClick.AddListener(OnCloseButtonClicked);
    }

    private void Start()
    {
        if (SoundManager.Instance == null)
            return;

        if (bgmSlider != null)
            bgmSlider.value = SoundManager.Instance.BGMVolume;

        if (sfxSlider != null)
            sfxSlider.value = SoundManager.Instance.SFXVolume;
    }

    private void OnDestroy()
    {
        if (bgmSlider != null)
            bgmSlider.onValueChanged.RemoveListener(OnBgmVolumeChanged);

        if (sfxSlider != null)
            sfxSlider.onValueChanged.RemoveListener(OnSfxVolumeChanged);

        if (closeButton != null)
            closeButton.onClick.RemoveListener(OnCloseButtonClicked);
    }

    private void OnBgmVolumeChanged(float value)
    {
        if (SoundManager.Instance == null)
            return;

        SoundManager.Instance.BGMVolume = value;
        SoundManager.Instance.SetVolume(Define.Sounds.BGM, value);
    }

    private void OnSfxVolumeChanged(float value)
    {
        if (SoundManager.Instance == null)
            return;

        SoundManager.Instance.SFXVolume = value;
        SoundManager.Instance.SetVolume(Define.Sounds.SFX, value);
    }

    private void OnCloseButtonClicked()
    {
        Hide();
    }
}