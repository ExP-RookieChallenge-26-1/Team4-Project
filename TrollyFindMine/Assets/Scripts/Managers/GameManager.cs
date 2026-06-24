using UnityEngine;
using System;

public class GameManager : Singleton<GameManager>
{
    #region delegate

    public event Action<bool> OnAtmoChanged;

    #endregion
    
    #region field

    [Header("Stage Information")]
    [SerializeField] private int stageCount = 9;
    [SerializeField] private bool[] clearedStages;
    private bool angryAtmosphere = false;

    #endregion

    #region property

    public bool[] ClearedStages => clearedStages; 
    public int StageCount => stageCount;
    public bool AngryAtmosphere => angryAtmosphere;

    #endregion

    #region Public Method

    //stage�� 1���� ���� stageCount�� ������ stage
    public void StageClear(int stageNumber)
    {
        if (stageNumber <= 0) Debug.LogError("0���� �۰ų� ���� �������� Ŭ����");
        else if (stageNumber > stageCount) Debug.LogError("12���� ū �������� Ŭ����");
        else clearedStages[stageNumber - 1] = true;
        
    }

    public void ResetData()
    {
        clearedStages = new bool[stageCount];
    }
    
    //게임 분위기 전환 함수
    public void ChangeAtmosphere()
    {
        bool prevBool = angryAtmosphere;
        angryAtmosphere = !angryAtmosphere;
        OnAtmoChanged?.Invoke(prevBool);
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
