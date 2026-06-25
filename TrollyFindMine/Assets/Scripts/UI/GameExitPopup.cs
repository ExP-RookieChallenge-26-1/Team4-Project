using UnityEngine;

public class GameExitPopup : UIViewStackable
{
    [Header("Layout")]
    [SerializeField] private Vector2 referenceResolution = new Vector2(1440f, 3200f);
    [SerializeField] private float referenceScale = 1f;

    private Vector2 _referenceAnchoredPosition;

    private void Awake()
    {
        CacheReferenceAnchoredPosition();
    }

    public override void Show()
    {
        base.Show();
        ApplyRootScale();
    }

    public void ConfirmExit()
    {
        SoundManager.Instance.Play(Define.SFX.fx_00_button);
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void Cancel()
    {
        SoundManager.Instance.Play(Define.SFX.fx_00_button);
        Hide();
    }

    private void ApplyRootScale()
    {
        if (referenceResolution.x <= 0f || referenceResolution.y <= 0f)
            return;

        if (Screen.width <= 0 || Screen.height <= 0)
            return;

        float scale = Mathf.Min(Screen.width / referenceResolution.x, Screen.height / referenceResolution.y);
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
}
