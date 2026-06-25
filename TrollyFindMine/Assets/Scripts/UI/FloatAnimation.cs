using DG.Tweening;
using UnityEngine;

public class FloatAnimation : MonoBehaviour
{
    [SerializeField] private float floatHeight = 20f;  // 위아래 이동 거리 (px)
    [SerializeField] private float duration = 1.2f;    // 한쪽 방향 이동 시간 (초)

    private Tween _tween;

    private void Start()
    {
        RectTransform rect = GetComponent<RectTransform>();
        float baseY = rect.anchoredPosition.y;

        _tween = rect
            .DOAnchorPosY(baseY + floatHeight, duration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    private void OnDestroy()
    {
        _tween?.Kill();
    }
}
