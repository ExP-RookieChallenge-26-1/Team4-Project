using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PreviousButtonController : MonoBehaviour
{
    #region field

    [SerializeField] private Button previousButton;

    #endregion

    #region Unity Cycle

    private void Start()
    {
        previousButton.onClick.AddListener(PreviousButtonClick);
    }

    #endregion

    #region Public Method

    public void PreviousButtonClick()
    {
        string currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        if(currentSceneName=="StageSelectScene") SceneManager.Instance.GoToMainMenu();
        else if(currentSceneName=="Stage1") SceneManager.Instance.GoToStageSelectScene();
        else if(currentSceneName=="Stage2") SceneManager.Instance.GoToStageSelectScene();
    }

    #endregion
}
