using System;
using UnityEngine;

public class Stage1 : MonoBehaviour
{
    
    #region Field

    [SerializeField] private MineFieldBackend _mineFieldBackend;
    [SerializeField] private int row = 5;
    [SerializeField] private int col = 5;
    [SerializeField] private int numOfMine = 8;
    private DialogueView dview;
    
    #endregion
    
    #region unity cycle
    private void Start()
    {
        dview = DialogueManager.Instance.PlayDialogue(DialogueKey.Stage1_Tutorial);
        dview.OnSequenceFinished += GameStart;
    }
    
    #endregion

    #region publicFunction
    // Dialogue View의 onSequenceFinished와 연결 
    public void GameStart()
    {
        dview.OnSequenceFinished -= GameStart;
        _mineFieldBackend.OnGameWin += WinSequence;
        _mineFieldBackend.OnGameOver += LoseSequence;
        _mineFieldBackend.InitGrid(row, col, numOfMine);
    }

    public void WinSequence()
    {
        GameManager.Instance.StageClear(1);
        _mineFieldBackend.OnGameWin -= WinSequence;
        _mineFieldBackend.OnGameOver -= LoseSequence;
        dview = DialogueManager.Instance.PlayDialogue(DialogueKey.Stage1_Clear);
        dview.OnSequenceFinished += WinSceneChange;
    }

    public void WinSceneChange()
    {
        dview.OnSequenceFinished -= WinSceneChange;
        SceneManager.Instance.GoToStageSelectScene();
    }

    public void LoseSequence()
    {
        GameManager.Instance.ResetData();
        _mineFieldBackend.OnGameWin -= WinSequence;
        _mineFieldBackend.OnGameOver -= LoseSequence;
        dview = DialogueManager.Instance.PlayDialogue(DialogueKey.Stage1_GameOver);
        dview.OnSequenceFinished += LoseSceneChange;
    }

    public void LoseSceneChange()
    {
        dview.OnSequenceFinished -= LoseSceneChange;
        SceneManager.Instance.GoToStageSelectScene();
    }

    #endregion

}
