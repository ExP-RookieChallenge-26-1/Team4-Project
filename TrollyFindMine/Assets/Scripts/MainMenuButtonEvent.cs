using System;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuButtonEvent : MonoBehaviour
{
    #region field

    [SerializeField] private Button settingsButton;
    [SerializeField] private Button gameStartButton;
    [SerializeField] private Button gameExitButton;
    
    #endregion

    #region unity cycle

    private void Start()
    {
        gameStartButton.onClick.AddListener(SceneManager.Instance.GoToStageSelectScene);
        //TODO : settingsButton, gameExitButton 연결
    }

    #endregion
}
