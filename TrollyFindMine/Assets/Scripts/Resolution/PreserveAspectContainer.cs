using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
public class PreserveAspectContainer : MonoBehaviour
{
    private RectTransform myRect;
    private RectTransform parentRect;
    private Image parentImage;

    void Awake()
    {
        myRect = GetComponent<RectTransform>();
        if (transform.parent != null)
        {
            parentRect = transform.parent.GetComponent<RectTransform>();
            parentImage = transform.parent.GetComponent<Image>();
        }
    }

    void Update()
    {
        if (parentRect == null || parentImage == null || parentImage.sprite == null) return;

        // 부모 프레임의 실제 시각적 크기를 계산
        Vector2 visualSize = GetParentVisualSize();

        // 이 컨테이너의 크기를 부모의 실제 시각적 크기와 완전히 일치시킴
        myRect.anchorMin = new Vector2(0.5f, 0.5f);
        myRect.anchorMax = new Vector2(0.5f, 0.5f);
        myRect.pivot = new Vector2(0.5f, 0.5f);
        myRect.localPosition = Vector3.zero;
        myRect.sizeDelta = visualSize;
    }

    private Vector2 GetParentVisualSize()
    {
        float parentWidth = parentRect.rect.width;
        float parentHeight = parentRect.rect.height;

        float spriteAspect = parentImage.sprite.rect.width / parentImage.sprite.rect.height;
        float rectAspect = parentWidth / parentHeight;

        Vector2 visualSize = Vector2.zero;

        if (parentImage.preserveAspect)
        {
            if (rectAspect > spriteAspect)
            {
                visualSize.y = parentHeight;
                visualSize.x = parentHeight * spriteAspect;
            }
            else
            {
                visualSize.x = parentWidth;
                visualSize.y = parentWidth / spriteAspect;
            }
        }
        else
        {
            visualSize = new Vector2(parentWidth, parentHeight);
        }

        return visualSize;
    }
}