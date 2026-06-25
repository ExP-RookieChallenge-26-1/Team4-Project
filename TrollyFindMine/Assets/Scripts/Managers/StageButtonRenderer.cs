using UnityEngine;
using UnityEngine.UI;

public class StageButtonRenderer : MonoBehaviour
{
    [Header("Stage Layout")]
    [SerializeField] private GameObject stageButtonPrefab;
    [SerializeField] private Transform stageButtonParent;
    [SerializeField] private GridLayoutGroup gridLayoutGroup;
    [SerializeField] private int columnCount = 3;
    private int stageCount = 9;
    private bool[] clearedStages;

    private void Start()
    {
        stageCount = GameManager.Instance.StageCount;
        clearedStages = GameManager.Instance.ClearedStages;
        GenerateStageButtons();
    }

    [ContextMenu("Generate Stage Buttons")]
    public void GenerateStageButtons()
    {
        if (!ValidateReferences())
            return;

        ClearExistingButtons();
        ApplyGridSettings();

        for (int i = 0; i < stageCount; i++)
        {
            GameObject buttonObject = Instantiate(stageButtonPrefab, stageButtonParent);
            buttonObject.name = $"StageButton_{i + 1}";

            StageButtonImageController buttonRenderer = buttonObject.GetComponent<StageButtonImageController>();
            bool isLocked = !IsStageUnlocked(i);
            int stageNumber = i + 1;

            if (buttonRenderer != null)
            {
                buttonRenderer.Initialize(i, isLocked);
            }

            Button button = buttonRenderer != null && buttonRenderer.StageButton != null
                ? buttonRenderer.StageButton
                : buttonObject.GetComponent<Button>();
            if (button != null && !isLocked)
            {
                button.onClick.RemoveAllListeners();
                AddStageButtonClickEvent(button, stageNumber);
            }
        }
    }

    private void AddStageButtonClickEvent(Button button, int stageNumber)
    {
        switch (stageNumber)
        {
            case 1:
                button.onClick.AddListener(SceneManager.Instance.GoToStage1);
                break;
            case 2:
                button.onClick.AddListener(SceneManager.Instance.GoToStage2);
                break;
            case 3:
                button.onClick.AddListener(SceneManager.Instance.GoToStage3);
                break;
            case 4:
                button.onClick.AddListener(SceneManager.Instance.GoToStage4);
                break;
            default:
                button.onClick.AddListener(() => Debug.LogWarning($"Stage {stageNumber} scene is not connected yet.", this));
                break;
        }
    }

    private bool ValidateReferences()
    {
        if (stageButtonPrefab == null)
        {
            Debug.LogError("StageManager: stageButtonPrefab is not assigned.", this);
            return false;
        }

        if (stageButtonParent == null)
        {
            Debug.LogError("StageManager: stageButtonParent is not assigned.", this);
            return false;
        }

        if (gridLayoutGroup == null)
        {
            Debug.LogError("StageManager: gridLayoutGroup is not assigned.", this);
            return false;
        }

        if (stageCount < 0)
        {
            Debug.LogError("StageManager: stageCount cannot be negative.", this);
            return false;
        }

        if (columnCount <= 0)
        {
            Debug.LogError("StageManager: columnCount must be at least 1.", this);
            return false;
        }

        return true;
    }

    private void ApplyGridSettings()
    {
        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayoutGroup.constraintCount = columnCount;
    }

    /// <summary>
    /// 버튼을 다시 생성하기 전에 부모 오브젝트 아래의 기존 스테이지 버튼들을 모두 제거합니다.
    /// </summary>
    private void ClearExistingButtons()
    {
        for (int i = stageButtonParent.childCount - 1; i >= 0; i--)
        {
            Transform child = stageButtonParent.GetChild(i);

            if (Application.isPlaying)
                Destroy(child.gameObject);
            else
                DestroyImmediate(child.gameObject);
        }
    }

    private bool IsStageUnlocked(int stageIndex)
    {
        if (stageIndex <= 0)
            return true;

        if (stageIndex - 1 >= clearedStages.Length)
            return false;

        return clearedStages[stageIndex - 1];
    }

}
