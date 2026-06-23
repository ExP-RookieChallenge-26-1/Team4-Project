using System;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    protected Outline outline;

    private void Awake()
    {
        outline = GetComponent<Outline>();
    }

    public void HighLightCell()
    {
        outline.enabled = true;
        RectTransform rect = GetComponent<RectTransform>();
        //윤곽선이 보이도록
        rect.SetAsLastSibling();
    } 
    public void DeHighLightCell()
    {
        outline.enabled = false;
    } 
}