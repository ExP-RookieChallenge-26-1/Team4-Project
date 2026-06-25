using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 화면 전체를 주기적으로 짧게 깜빡이는 기믹용 컴포넌트.
/// 오버레이는 별도의 최상위 Canvas에 raycastTarget = false로 생성되어
/// 클릭 등의 입력을 절대 가리거나 막지 않는다.
/// </summary>
public class ScreenBlinkEffect : MonoBehaviour
{
    [SerializeField] private float blinkInterval = 3f;
    [SerializeField] private float flashDuration = 0.15f;
    [SerializeField] private Color flashColor = new Color(1f, 1f, 1f, 0.5f);

    private Image _overlayImage;
    private Coroutine _blinkRoutine;

    private void Awake()
    {
        CreateOverlay();
    }

    private void CreateOverlay()
    {
        GameObject canvasObject = new GameObject("ScreenBlinkCanvas", typeof(RectTransform));
        canvasObject.transform.SetParent(transform, false);

        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = short.MaxValue; //다른 UI보다 항상 위에 그려지도록

        GameObject overlayObject = new GameObject("Overlay", typeof(RectTransform), typeof(Image));
        overlayObject.transform.SetParent(canvasObject.transform, false);

        RectTransform rect = overlayObject.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        _overlayImage = overlayObject.GetComponent<Image>();
        _overlayImage.raycastTarget = false; //클릭이 항상 통과되도록
        _overlayImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, 0f);
    }

    public void StartBlinking()
    {
        StopBlinking();
        _blinkRoutine = StartCoroutine(BlinkLoop());
    }

    public void StopBlinking()
    {
        if (_blinkRoutine != null)
        {
            StopCoroutine(_blinkRoutine);
            _blinkRoutine = null;
        }

        SetAlpha(0f);
    }

    private IEnumerator BlinkLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(blinkInterval);
            yield return Flash();
        }
    }

    private IEnumerator Flash()
    {
        float half = flashDuration * 0.5f;

        for (float t = 0f; t < half; t += Time.deltaTime)
        {
            SetAlpha(Mathf.Lerp(0f, flashColor.a, t / half));
            yield return null;
        }

        for (float t = 0f; t < half; t += Time.deltaTime)
        {
            SetAlpha(Mathf.Lerp(flashColor.a, 0f, t / half));
            yield return null;
        }

        SetAlpha(0f);
    }

    private void SetAlpha(float alpha)
    {
        if (_overlayImage != null)
        {
            _overlayImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, alpha);
        }
    }

    private void OnDisable()
    {
        StopBlinking();
    }
}
