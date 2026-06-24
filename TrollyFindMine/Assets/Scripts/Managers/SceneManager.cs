using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : Singleton<SceneManager>
{
    #region Unity Cycle

    protected override void Awake()
    {
        base.Awake();
    }

    #endregion

    #region Public Function

    public void GoToStageSelectScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("StageSelectScene");
    }

    public void GoToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("StartScene");
    }

    public void GoToStage1()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Stage1");
    }

    public void GoToStage2()
    {
        
    }

    #endregion

}
