using UnityEngine;

public class GameExitPopup : UIViewStackable
{
    public void ConfirmExit()
    {
        SoundManager.Instance.Play(Define.SFX.fx_00_button);
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void Cancel()
    {
        SoundManager.Instance.Play(Define.SFX.fx_00_button);
        Hide();
    }
}
