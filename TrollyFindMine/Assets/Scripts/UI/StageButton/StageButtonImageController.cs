using UnityEngine;
using UnityEngine.UI;

public class StageButtonImageController : MonoBehaviour
{
    [SerializeField] private Button stageButton;
    [SerializeField] private GameObject lockIcon;
    [SerializeField] private Image stageNumberImage;
    [SerializeField] private Sprite[] stageNumberSprites;

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
        if (stageNumberImage == null)
        {
            return;
        }

        int spriteIndex = stageNumber - 1;
        if (stageNumberSprites == null || spriteIndex < 0 || spriteIndex >= stageNumberSprites.Length)
        {
            Debug.LogWarning($"Stage number sprite is not assigned for stage {stageNumber}.", this);
            stageNumberImage.sprite = null;
            stageNumberImage.enabled = false;
            return;
        }

        stageNumberImage.sprite = stageNumberSprites[spriteIndex];
        stageNumberImage.enabled = true;
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
