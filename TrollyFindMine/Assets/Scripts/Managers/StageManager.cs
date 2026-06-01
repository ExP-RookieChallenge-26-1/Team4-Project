using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    [Header("Stage Layout")]
    [SerializeField] private GameObject stageButtonPrefab;
    [SerializeField] private Transform stageButtonParent;
    [SerializeField] private GridLayoutGroup gridLayoutGroup;
    [SerializeField] private int stageCount = 12;
    [SerializeField] private int columnCount = 3;

    [Header("Stage Progress")]
    [SerializeField] private bool[] clearedStages;

    [Header("Stage Labels")]
    [SerializeField] private bool setStageNumberLabel = true;

    private void Start()
    {
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

            StageButtonRenderer buttonRenderer = buttonObject.GetComponent<StageButtonRenderer>();
            bool isLocked = !IsStageUnlocked(i);

            if (buttonRenderer != null)
            {
                buttonRenderer.Initialize(i, isLocked);
            }
            else if (setStageNumberLabel)
            {
                SetStageLabel(buttonObject, i + 1);
            }
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

        EnsureClearArraySize();

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

    private void SetStageLabel(GameObject buttonObject, int stageNumber)
    {
        TextMeshProUGUI tmpLabel = buttonObject.GetComponentInChildren<TextMeshProUGUI>(true);
        if (tmpLabel != null)
        {
            tmpLabel.text = stageNumber.ToString();
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

    /// <summary>
    /// clearedStages 배열의 길이를 현재 stageCount에 맞게 조정합니다.
    /// 기존 값은 유지하고, 부족한 칸은 false로 초기화합니다.
    /// </summary>
    private void EnsureClearArraySize()
    {
        if (clearedStages != null && clearedStages.Length == stageCount)
            return;

        bool[] resizedArray = new bool[stageCount];

        if (clearedStages != null)
        {
            int copyLength = Mathf.Min(clearedStages.Length, resizedArray.Length);
            for (int i = 0; i < copyLength; i++)
            {
                resizedArray[i] = clearedStages[i];
            }
        }

        clearedStages = resizedArray;
    }
}
