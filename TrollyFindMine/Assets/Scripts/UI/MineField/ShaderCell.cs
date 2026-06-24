using System;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.UI;

public class ShaderCell : Cell
{
    #region field

    private bool isFlagged = false;
    [SerializeField] private Image image;
    [SerializeField] private Sprite normalFlag;
    [SerializeField] private Sprite angryFlag;

    #endregion

    #region public method

    public void ChangeAtmoSphere(bool atmo)
    {
        Image thisImage = GetComponent<Image>();
        if (isFlagged)
        {
            if (atmo == false)
            {
                image.sprite = normalFlag;    
            }
            else
            {
                image.sprite = angryFlag;
            }
        }

        if (selectedCell)
        {
            if (atmo == false)
            {
                thisImage.sprite = outlineNormal;
            }
            else
            {
                thisImage.sprite = outlineAngry;
            }
        }
        else
        {
            if (atmo == false)
            {
                thisImage.sprite = cellNormal;
            }
            else
            {
                thisImage.sprite = cellAngry;
            }
        }
    }

    #endregion
    
    #region unity cycle

    private void OnEnable()
    {
        GameManager.Instance.OnAtmoChanged += ChangeAtmoSphere;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnAtmoChanged -= ChangeAtmoSphere;
    }

    #endregion
}
