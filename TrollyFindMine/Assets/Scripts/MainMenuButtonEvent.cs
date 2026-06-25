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
        SoundManager.Instance.Play(Define.BGM.p_theme_v2);

        gameStartButton.onClick.AddListener(SceneManager.Instance.GoToStageSelectScene);
        gameStartButton.onClick.AddListener(() => SoundManager.Instance.Play(Define.SFX.fx_00_button));
        settingsButton.onClick.AddListener(() => SoundManager.Instance.Play(Define.SFX.fx_00_button));
        gameExitButton.onClick.AddListener(() => SoundManager.Instance.Play(Define.SFX.fx_00_button));
        //TODO : settingsButton, gameExitButton 연결
    }

    #endregion
}
