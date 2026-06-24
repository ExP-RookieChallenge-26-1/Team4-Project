using System;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    [SerializeField] protected Sprite outlineNormal;
    [SerializeField] protected Sprite outlineAngry;
    [SerializeField] protected Sprite cellNormal;
    [SerializeField] protected Sprite cellAngry;
    protected bool selectedCell = false;

    public virtual void HighLightCell()
    {
        selectedCell = true;
        Image image = GetComponent<Image>();
        if (GameManager.Instance.AngryAtmosphere == false)
        {
            image.sprite = outlineNormal;
            
        }
        else
        {
            image.sprite = outlineAngry;
        }
        RectTransform rect = GetComponent<RectTransform>();
        //윤곽선이 보이도록
        rect.SetAsLastSibling();
    } 
    public virtual void DeHighLightCell()
    {
        selectedCell = false;
        Image image = GetComponent<Image>();
        if (cellNormal == null || cellAngry == null)
        {
            image.sprite = null;
            return;
        }
        if (GameManager.Instance.AngryAtmosphere == false)
        {
            image.sprite = cellNormal;
        }
        else
        {
            image.sprite = cellAngry;
        }
    }
}