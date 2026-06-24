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

    #endregion

    #region unity cycle
    

    #endregion    
    
}
