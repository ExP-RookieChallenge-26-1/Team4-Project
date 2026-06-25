using UnityEngine;
using UnityEngine.UI;

public class PauseView : UIViewStackable
{
    [Header("Sound UI")]
    [SerializeField] private Image soundOnImage;
    [SerializeField] private Image soundOffImage;
    [SerializeField] private Slider soundSlider;
    [SerializeField] private Graphic[] sliderBarGraphics;

    private bool _isSoundOn = true;
    private Color[] _sliderBarOriginalColors;

    private void Awake()
    {
        CacheOriginalSliderBarColors();
        InitializeSoundSlider();
        _isSoundOn = !SoundManager.Instance.IsMuted;
        ApplySoundStyle();
    }

    public void Close()
    {
        Hide();
    }

    public void ToggleSoundStyle()
    {
        SoundManager.Instance.ToggleMute();
        _isSoundOn = !SoundManager.Instance.IsMuted;
        ApplySoundStyle();
    }

    private void InitializeSoundSlider()
    {
        if (soundSlider == null)
            return;

        soundSlider.SetValueWithoutNotify(SoundManager.Instance.BGMVolume);
        soundSlider.onValueChanged.AddListener(SetSoundVolume);
    }

    private void SetSoundVolume(float volume)
    {
        float clampedVolume = Mathf.Clamp01(volume);

        SoundManager.Instance.BGMVolume = clampedVolume;
        SoundManager.Instance.SFXVolume = clampedVolume;
        SoundManager.Instance.ApplyVolume();
    }

    private void ApplySoundStyle()
    {
        if (soundOnImage != null)
        {
            soundOnImage.gameObject.SetActive(_isSoundOn);
        }

        if (soundOffImage != null)
        {
            soundOffImage.gameObject.SetActive(!_isSoundOn);
        }

        if (soundSlider != null)
        {
            soundSlider.interactable = _isSoundOn;
        }

        ApplySliderBarStyle();
    }

    private void CacheOriginalSliderBarColors()
    {
        if (sliderBarGraphics == null)
            return;

        _sliderBarOriginalColors = new Color[sliderBarGraphics.Length];

        for (int i = 0; i < sliderBarGraphics.Length; i++)
        {
            if (sliderBarGraphics[i] != null)
            {
                _sliderBarOriginalColors[i] = sliderBarGraphics[i].color;
            }
        }
    }

    private void ApplySliderBarStyle()
    {
        if (sliderBarGraphics == null)
            return;

        for (int i = 0; i < sliderBarGraphics.Length; i++)
        {
            Graphic graphic = sliderBarGraphics[i];
            if (graphic == null)
                continue;

            if (_isSoundOn && _sliderBarOriginalColors != null && i < _sliderBarOriginalColors.Length)
            {
                graphic.color = _sliderBarOriginalColors[i];
                continue;
            }

            Color disabledColor = graphic.color;
            disabledColor.a = 0.4f;
            graphic.color = disabledColor;
        }
    }
}
