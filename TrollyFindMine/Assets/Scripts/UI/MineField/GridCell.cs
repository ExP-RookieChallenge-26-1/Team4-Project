using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class GridCell : Cell
{
    #region field

    [SerializeField] private List<Sprite> numbers;
    [SerializeField] private Sprite mine;
    [SerializeField] Image image;

    #endregion

    #region public Method

    public override void HighLightCell()
    {
        base.HighLightCell();
        Image thisImage = GetComponent<Image>();
        thisImage.color = new Color(1f, 1f, 1f, 1f);
    }

    public override void DeHighLightCell()
    {
        base.DeHighLightCell();
        Image thisImage = GetComponent<Image>();
        thisImage.color = new Color(1f, 1f, 1f, 0f);
    }

    public void ChangeCellImage(int id)
    {
        
        if (image == null)
        {
            Debug.LogError("Image가 없습니다!");
            return;
        }
        
        //GridCell ID 대로 변경
        if (id == -1)
        {
            image.sprite = mine;
            image.color = new Color(1f, 1f, 1f, 1f);
        }
        else if (id == 0) //빈칸은 투명하게
        {
            image.sprite = numbers[id];
            image.color = new Color(1f, 1f, 1f, 0f);
        }
        else if (0 < id && id < 9)
        {
            image.sprite = numbers[id];
            image.color = new Color(1f, 1f, 1f, 1f);
        }
        else
        {
            Debug.Log($"알맞지 않은 Grid ID입니다: {id}");
        }
    }

    public void ChangeAtmosphere(bool atmo)
    {
        Image thisImage = GetComponent<Image>();
        if (selectedCell)
        {
            thisImage.sprite = atmo ? outlineAngry : outlineNormal;
        }
        else
        {
            if (cellNormal == null || cellAngry == null) return;
            thisImage.sprite = atmo ? cellAngry : cellNormal;
        }
    }

    #endregion

    #region unity cycle

    private void OnEnable()
    {
        GameManager.Instance.OnAtmoChanged += ChangeAtmosphere;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnAtmoChanged -= ChangeAtmosphere;
    }

    #endregion    
    
}
