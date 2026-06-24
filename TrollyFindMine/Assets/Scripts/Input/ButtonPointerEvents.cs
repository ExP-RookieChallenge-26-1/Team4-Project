using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonPointerEvents : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public event Action OnPressed;
    public event Action OnReleased;

    public void OnPointerDown(PointerEventData eventData)
    {
        OnPressed?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnReleased?.Invoke();
    }
}
