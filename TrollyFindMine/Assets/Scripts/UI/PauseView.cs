using UnityEngine;
using UnityEngine.UI;

public class PauseView : UIViewStackable
{
    [Header("Layout")]
    [SerializeField] private Vector2 referenceResolution = new Vector2(1440f, 3200f);
    [SerializeField] private float referenceScale = 0.8333333f;

    [Header("Sound UI")]
    [SerializeField] private Image soundOnImage;
    [SerializeField] private Image soundOffImage;
    [SerializeField] private Slider soundSlider;
    [SerializeField] private Graphic[] sliderBarGraphics;

    private bool _isSoundOn = true;
    private Color[] _sliderBarOriginalColors;
    private Vector2 _referenceAnchoredPosition;

    private void Awake()
    {
        CacheReferenceAnchoredPosition();
        CacheOriginalSliderBarColors();
        InitializeSoundSlider();
        _isSoundOn = !SoundManager.Instance.IsMuted;
        ApplySoundStyle();
    }

    public override void Show()
    {
        base.Show();
        ApplyRootScale();
    }

    public void Close()
    {
        Hide();
    }

    public void PlayButtonSound()
    {
        SoundManager.Instance.Play(Define.SFX.fx_00_button);
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

    private void ApplyRootScale()
    {
        if (transform.parent is not RectTransform parentRectTransform)
            return;

        if (referenceResolution.x <= 0f || referenceResolution.y <= 0f)
            return;

        float parentWidth = parentRectTransform.rect.width;
        float parentHeight = parentRectTransform.rect.height;

        if (parentWidth <= 0f || parentHeight <= 0f)
            return;

        float scale = Mathf.Min(parentWidth / referenceResolution.x, parentHeight / referenceResolution.y);
        transform.localScale = Vector3.one * scale * referenceScale;

        if (transform is RectTransform rectTransform)
        {
            rectTransform.anchoredPosition = _referenceAnchoredPosition * scale;
        }
    }

    private void CacheReferenceAnchoredPosition()
    {
        if (transform is RectTransform rectTransform)
        {
            _referenceAnchoredPosition = rectTransform.anchoredPosition;
        }
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
