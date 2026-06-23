using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    #region field

    [Header("Stage Information")]
    [SerializeField] private int stageCount = 12;
    [SerializeField] private bool[] clearedStages;

    #endregion

    #region property

    public bool[] ClearedStages => clearedStages; 
    public int StageCount => stageCount;

    #endregion

    #region Public Method

    //stage는 1부터 시작 stageCount가 마지막 stage
    public void stageClear(int stageNumber)
    {
        if (stageNumber <= 0) Debug.LogError("0보다 작거나 같은 스테이지 클리어");
        else if (stageNumber > stageCount) Debug.LogError("12보다 큰 스테이지 클리어");
        else clearedStages[stageNumber - 1] = true;
        
    }

    public void ResetData()
    {
        clearedStages = new bool[stageCount];
    }

    #endregion

    #region unity cycle
    protected override void Awake()
    {
        base.Awake();
        clearedStages = new bool[stageCount];
    }

    #endregion
}
