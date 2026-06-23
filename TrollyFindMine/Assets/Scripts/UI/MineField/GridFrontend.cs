using UnityEngine;

public class GridFrontend : Frontend
{
    #region Fields

    [SerializeField] private GameObject gridCellPrefab;
    private GridCell[,] gridCells;
    
    #endregion
    
    #region Properties
    
    #endregion
    
    #region Methods

    public void InitializeGrid(int row, int col, int [,] gridInfo)
    {
        //gridCells 할당
        Cells = new GameObject[row, col];
        gridCells = new GridCell[row, col];
        
        RectTransform thisRectTransform = gameObject.GetComponent<RectTransform>();
        //cell의 부모 오브젝트의 width와 height
        float width = thisRectTransform.rect.width;
        float height = thisRectTransform.rect.height;
        Debug.Log(width);
        Debug.Log(height);
        //instantiate 시작 위치
        Vector2 instantiateStartPos;
        float prefabWidth;
        if (row <= col)
        {
            prefabWidth = width / col;
            instantiateStartPos = new Vector2(0f, -(height - prefabWidth * row) / 2.0f);
        }
        else
        {
            prefabWidth = height / col;
            instantiateStartPos = new Vector2((width - prefabWidth * col) / 2.0f, 0f);
        }

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                Debug.Log(prefabWidth);
                Vector2 offset = new Vector2(prefabWidth*j,-prefabWidth*i);
                GameObject cell = Instantiate(gridCellPrefab, thisRectTransform, false);
                
                //HighLight를 위한 배열에 cell reference저장
                Cells[i, j] = cell;
                //배열에 script저장
                gridCells[i,j] = cell.GetComponent<GridCell>();
                
                //cell의 pivot을 좌상단으로 설정
                RectTransform cellRectTransform = cell.GetComponent<RectTransform>();
                cellRectTransform.anchorMax = new Vector2(0,1);
                cellRectTransform.anchorMin = new Vector2(0, 1);
                cellRectTransform.pivot = new Vector2(0, 1);

                //cell의 크기를 설정
                cellRectTransform.sizeDelta = new Vector3(prefabWidth, prefabWidth, 1);
                
                //좌표 계산해서 채워넣기
                cellRectTransform.anchoredPosition = instantiateStartPos + offset;
                
                //gridCell id대로 변경
                gridCells[i,j].ChangeCellImage(gridInfo[i,j]);
            }
        }
    }
    
    //TODO : highlight 구현 
    
    #endregion
}
