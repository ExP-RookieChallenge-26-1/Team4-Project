using UnityEngine;

public class GameExitPopupButtonController : MonoBehaviour
{
    [SerializeField] private Transform popupParent;

    public void ShowGameExitPopup()
    {
        GameExitPopup popup = UIManager.Instance.GetUIComponent<GameExitPopup>(popupParent);

        if (popup == null)
            return;

        popup.Show();
    }
}
