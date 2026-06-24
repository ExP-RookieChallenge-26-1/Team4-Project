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

    #endregion

}
