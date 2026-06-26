using System.Collections;
using UnityEngine;

public class Stage4 : MonoBehaviour
{
    #region Field

    [SerializeField] private MineFieldBackend _mineFieldBackend;
    [SerializeField] private int row = 15;
    [SerializeField] private int col = 15;
    [SerializeField] private int numOfMine = 30;
    private DialogueView dview;
    private bool _firstGimmickFired = false;
    private Coroutine _slideGimmickCoroutine;

    // 4방향: 우, 좌, 상, 하 (SelectedCellMove의 dx/dy 좌표계 기준)
    private static readonly int[] _slideDx = { 1, -1, 0, 0 };
    private static readonly int[] _slideDy = { 0, 0, 1, -1 };

    #endregion

    #region Unity Cycle

    private void Start()
    {
        dview = DialogueManager.Instance.PlayDialogue(DialogueKey.Stage4_Tutorial);
        dview.OnSequenceFinished += GameStart;
    }

    #endregion

    #region Public Methods

    public void GameStart()
    {
        dview.OnSequenceFinished -= GameStart;
        _mineFieldBackend.OnGameWin += WinSequence;
        _mineFieldBackend.OnGameOver += LoseSequence;
        _mineFieldBackend.OnGameMiddle += MiddleSequence;
        _mineFieldBackend.InitGrid(row, col, numOfMine);
        _slideGimmickCoroutine = StartCoroutine(SlideGimmickCoroutine());
    }

    public void WinSequence()
    {
        GameManager.Instance.StageClear(4);
        _mineFieldBackend.OnGameWin -= WinSequence;
        _mineFieldBackend.OnGameOver -= LoseSequence;
        StopSlideGimmick();
        if (_firstGimmickFired)
            GameManager.Instance.ChangeAtmosphere();
        dview = DialogueManager.Instance.PlayDialogue(DialogueKey.Stage4_Clear);
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
        StopSlideGimmick();
        if (_firstGimmickFired)
            GameManager.Instance.ChangeAtmosphere();
        dview = DialogueManager.Instance.PlayDialogue(DialogueKey.Stage4_GameOver);
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
        dview = DialogueManager.Instance.PlayDialogue(DialogueKey.Stage4_Hint_01);
    }

    #endregion

    #region Private Methods

    private void StopSlideGimmick()
    {
        if (_slideGimmickCoroutine != null)
        {
            StopCoroutine(_slideGimmickCoroutine);
            _slideGimmickCoroutine = null;
        }
    }

    // 2~3초마다 선택된 셀을 랜덤 방향으로 한 칸 미끄러뜨리는 Stage4 전용 기믹.
    // 범위 체크 및 Highlight는 SelectedCellMove 내부에서 처리됨.
    private IEnumerator SlideGimmickCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(2f, 3f));

            int dir = Random.Range(0, 4);
            _mineFieldBackend.SelectedCellMove(_slideDx[dir], _slideDy[dir]);

            if (!_firstGimmickFired)
            {
                _firstGimmickFired = true;
                GameManager.Instance.ChangeAtmosphere();
                dview = DialogueManager.Instance.PlayDialogue(DialogueKey.Stage4_FirstG);
            }
        }
    }

    #endregion
}
