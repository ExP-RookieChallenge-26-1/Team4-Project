using UnityEngine;

public class Stage3 : MonoBehaviour
{
    #region Field

    [SerializeField] private MineFieldBackend _mineFieldBackend;
    [SerializeField] private int row = 15;
    [SerializeField] private int col = 15;
    [SerializeField] private int numOfMine = 30;
    private DialogueView dview;
    private ScreenBlinkEffect _screenBlinkEffect;

    #endregion

    #region unity cycle
    private void Start()
    {
        _screenBlinkEffect = gameObject.AddComponent<ScreenBlinkEffect>();
        dview = DialogueManager.Instance.PlayDialogue(DialogueKey.Stage3_Tutorial);
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
        _mineFieldBackend.OnGameMiddle += MiddleSequence;
        _mineFieldBackend.InitGrid(row, col, numOfMine);
        GameManager.Instance.ChangeAtmosphere(); //튜토리얼 종료 후 분위기를 angry로 전환
        _screenBlinkEffect.StartBlinking();
    }

    public void WinSequence()
    {
        GameManager.Instance.StageClear(3);
        _mineFieldBackend.OnGameWin -= WinSequence;
        _mineFieldBackend.OnGameOver -= LoseSequence;
        GameManager.Instance.ChangeAtmosphere(); //게임 종료 후 분위기 복원
        _screenBlinkEffect.StopBlinking();
        dview = DialogueManager.Instance.PlayDialogue(DialogueKey.Stage3_Clear);
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
        GameManager.Instance.ChangeAtmosphere(); //게임 종료 후 분위기 복원
        _screenBlinkEffect.StopBlinking();
        dview = DialogueManager.Instance.PlayDialogue(DialogueKey.Stage3_GameOver);
        dview.OnSequenceFinished += LoseSceneChange;
    }

    public void LoseSceneChange()
    {
        dview.OnSequenceFinished -= LoseSceneChange;
        SceneManager.Instance.GoToStageSelectScene();
    }

    public void MiddleSequence()
    {
        _mineFieldBackend.OnGameMiddle -= MiddleSequence;
        dview = DialogueManager.Instance.PlayDialogue(DialogueKey.Stage3_Hint_01);
    }
    #endregion
}
