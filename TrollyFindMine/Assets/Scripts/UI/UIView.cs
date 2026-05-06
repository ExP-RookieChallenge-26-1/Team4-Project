using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIView : MonoBehaviour
{
    public static event Action<UIView> OnShowUI;
    public static event Action<UIView> OnHideUI ;
    
    public virtual void Show()
    {
        gameObject.SetActive(true);
        OnShowUI?.Invoke(this);
        // TODO: Animation & Sound
    }
    
    public virtual void Hide()
    {
        gameObject.SetActive(false);
        OnHideUI?.Invoke(this);
        // TODO: Animation & Sound
    }
}
