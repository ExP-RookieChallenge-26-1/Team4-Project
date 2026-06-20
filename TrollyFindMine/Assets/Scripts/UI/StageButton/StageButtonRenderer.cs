using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageButtonRenderer : MonoBehaviour
{
    [SerializeField] private Button stageButton;
    [SerializeField] private GameObject lockIcon;
    [SerializeField] private TextMeshProUGUI stageNumberLabel;

    private int _stageIndex;
    private bool _isLocked;

    public int StageIndex => _stageIndex;
    public bool IsLocked => _isLocked;

    public void Initialize(int stageIndex, bool isLocked)
    {
        _stageIndex = stageIndex;
        SetStageNumber(stageIndex + 1);
        SetLocked(isLocked);
    }

    public void SetStageNumber(int stageNumber)
    {
        if (stageNumberLabel != null)
        {
            stageNumberLabel.text = stageNumber.ToString();
        }
    }

    public void SetLocked(bool isLocked)
    {
        _isLocked = isLocked;

        if (lockIcon != null)
        {
            lockIcon.SetActive(isLocked);
        }

        if (stageButton != null)
        {
            stageButton.interactable = !isLocked;
        }
    }
}
