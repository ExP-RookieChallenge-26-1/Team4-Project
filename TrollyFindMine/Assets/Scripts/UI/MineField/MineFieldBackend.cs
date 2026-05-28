using UnityEngine;
using System.Collections.Generic;


public class MineFieldBackend : MonoBehaviour
{
    #region Field

    [SerializeField] private ButtonFrontend buttonController;
    [SerializeField] private GridFrontend gridController;
    
    private int[,] grid; //-1이 지뢰 0은 빈칸 나머지 [1-8]는 숫자를 의미 -2는 초기값
    private bool[,] openedCells;
    private int row;
    private int col;
    private int leftMineNumber;
    private int openedCellNumber = 0;
    private bool mapGeneratedFlag = false;
    private static readonly int[] dx = { 0, 0, -1, 1, 1, 1, -1, -1 };
    private static readonly int[] dy = { 1, -1, 0, 0, 1, -1, 1, -1 };

    #endregion
    
    #region Property

    public int LeftMineNumber => leftMineNumber;
    public int OpenedCellNumber => openedCellNumber;

    #endregion
    
    #region Private Methods

    private bool IsInRange(Coord coord)
    {
        return (0 <= coord.x && coord.x < row && 0 <= coord.y && coord.y < col);
    }
    
    private void GenerateMap(Coord startPoint)
    {
        grid[startPoint.x, startPoint.y] = 0;
        for (int i = 0; i < 8; i++)
        {
            int nx = startPoint.x + dx[i];
            int ny = startPoint.y + dy[i];
            Coord nc = new Coord(nx, ny);
            if (IsInRange(nc)) grid[nc.x, nc.y] = 0;
        }

        int toGenerateMine = leftMineNumber;
        while (toGenerateMine > 0) //지뢰 배열하기
        {
            int nx = Random.Range(0, row);
            int ny = Random.Range(0, col);
            if (grid[nx, ny] != -2) continue;
            else
            {
                grid[nx, ny] = -1;
                toGenerateMine--;
            }
        }
        
        //지뢰 배열 기반으로 숫자 배열
        for (int i = 0; i < row; i++) 
        {
            for (int j = 0; j < col; j++)
            {
                if (grid[i, j] == -2 || grid[i, j] == 0)
                {
                    int mineCount = 0;
                    for (int k = 0; k < 8; k++)
                    {
                        int nx = i + dx[k];
                        int ny = j + dy[k];
                        Coord nc = new Coord(nx, ny);

                        if (IsInRange(nc) && grid[nc.x, nc.y] == -2)
                        {
                            mineCount++;
                        }
                    }

                    grid[i, j] = mineCount;
                }
            }
        }
        
        //TODO : Frontend에 변경사항 적용하기
    }
    
    #endregion
    
    #region Public Methods
    
    public void InitGrid(int n, int m, int numOfMine)
    {
        if (numOfMine > n * m) //지뢰의 개수가 grid의 개수보다 많을 때
        {
            Debug.LogError("올바르지 않은 생성입니다.");
        }
        row = n;
        col = m;
        grid = new int[row, col];
        openedCells = new bool[row, col];
        for(int i = 0; i < row; i++) for (int j = 0; j < col; j++) grid[i, j] = -2; 
        
        //TODO: buttonFrontend에서 N,M초기화
        //TODO: GridFrontend에서 N,M초기화
        leftMineNumber = numOfMine;
    }

    public void OpenCell(Coord coord)
    {
        if (mapGeneratedFlag)
        {
            if (grid[coord.x, coord.y] == -2) //지뢰를 클릭했을때
            {
                //TODO : 게임 오버 로직
                //TODO : 프론트 엔드 변화
            }
            else
            {
                if (grid[coord.x, coord.y] == 0) //bfs 필요
                {
                    //TODO : BFS로 cell open
                }
                else //그 숫자 하나만 열면 됨
                {
                    //TODO : cell 하나 open
                }
            }
            
            //TODO : Frontend Update
        }
        else //아직 맵이 생성되지 않았을때 -> map을 generate해야함 
        {
            GenerateMap(coord);
            mapGeneratedFlag = true;
            OpenCell(coord);
        }
    }

    #endregion
}
