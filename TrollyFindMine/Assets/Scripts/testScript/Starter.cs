using UnityEngine;

public class MineFieldTestStarter : MonoBehaviour
{
    [SerializeField] private MineFieldBackend mineFieldBackend;
    [SerializeField] private int row = 9;
    [SerializeField] private int col = 9;
    [SerializeField] private int mineCount = 10;

    private void Start()
    {
        if (mineFieldBackend == null)
        {
            Debug.LogError("MineFieldBackend가 연결되지 않았습니다.", this);
            return;
        }

        mineFieldBackend.InitGrid(row, col, mineCount);
    }
}