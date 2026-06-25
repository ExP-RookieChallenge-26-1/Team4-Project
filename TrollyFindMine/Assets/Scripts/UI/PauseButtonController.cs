using UnityEngine;

public class PauseButtonController : MonoBehaviour
{
    [SerializeField] private Transform pauseViewParent;

    public void ShowPauseView()
    {
        PauseView pauseView = UIManager.Instance.GetUIComponent<PauseView>(pauseViewParent);
        if (pauseView == null)
            return;

        pauseView.Show();
    }
}
