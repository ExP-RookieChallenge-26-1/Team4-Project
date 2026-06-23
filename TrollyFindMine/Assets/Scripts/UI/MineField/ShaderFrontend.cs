using UnityEngine;

public class ShaderFrontend : Frontend
{
    #region fields

    private GameObject[,] shaderCells;
    [SerializeField] private GameObject shaderCellPrefab;
    [SerializeField] private GameObject shaderCellAngryPrefab;

    #endregion

    public void InitializeGrid(int row, int col)
    {
        //gridCells 할당
        shaderCells = new GameObject[row, col];
        //highlight를 위한 할당
        Cells = new GameObject[row, col];

        RectTransform thisRectTransform = gameObject.GetComponent<RectTransform>();
        //cell의 부모 오브젝트의 width와 height
        float width = thisRectTransform.localScale.x;
        float height = thisRectTransform.localScale.y;

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
                Vector2 offset = new Vector2(prefabWidth * j, -prefabWidth * i);
                GameObject cell = Instantiate(shaderCellPrefab, thisRectTransform, false);
                //highlight를 위해 배열에 cell reference 저장
                Cells[i, j] = cell;
                //배열에 script저장
                shaderCells[i, j] = cell;

                //cell의 pivot을 좌상단으로 설정
                RectTransform cellRectTransform = cell.GetComponent<RectTransform>();
                cellRectTransform.anchorMax = new Vector2(0, 1);
                cellRectTransform.anchorMin = new Vector2(0, 1);

                //좌표 계산해서 채워넣기
                cellRectTransform.anchoredPosition = instantiateStartPos + offset;
            }
        }
    }
    //클릭했을때 사라지게 하는 메서드 만들기, bfs에서 호출될때도 적용되어야함
    public void ShaderClicked(Coord coord)
    {
        Destroy(Cells[coord.x, coord.y]);
    }
    
    //깃발 꽂는 메서드
    public void SetFlag(Coord coord)
    {
        //TODO : 깃발 꽂는 imageChange 기능 만들기 
    }
}