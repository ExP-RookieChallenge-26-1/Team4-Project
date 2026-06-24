using UnityEngine;

public abstract class Frontend : MonoBehaviour
{
    #region Fields

    protected Coord selectedCoord = new Coord(-1, -1);
    [SerializeField] protected MineFieldBackend mineFieldBackend;
    protected GameObject[,] Cells;
    
    #endregion
    
    #region Unity Lifecycle

    private void OnEnable()
    {
        mineFieldBackend.OnSelectedCoordChanged += HighlightCell;
    }

    private void OnDisable()
    {
        mineFieldBackend.OnSelectedCoordChanged -= HighlightCell;
    }
    
    #endregion
    
    #region common methods

    private void HighlightCell(Coord coord)
    {
        
        //이전 셀의 HighLight를 제거
        if(selectedCoord.x!=-1 && selectedCoord.y!=-1&& Cells!=null&& Cells[selectedCoord.x,selectedCoord.y] !=null) 
            Cells[selectedCoord.x,selectedCoord.y].GetComponent<Cell>().DeHighLightCell();    
        //selected coord 갱신
        selectedCoord = coord;
        //현제 셀을 HighLight
        if(Cells!=null && Cells[selectedCoord.x,selectedCoord.y] !=null)
            Cells[selectedCoord.x,selectedCoord.y].GetComponent<Cell>().HighLightCell();
    }
    
    #endregion
}
