using System.Collections.Generic;
using UnityEngine;

public enum DialogueKey
{
    None,
    Stage1_Tutorial,
    Stage1_Hint_01,
    Stage1_Clear,
    Stage1_GameOver,
    Stage2_Tutorial,
    Stage2_Hint_01,
    Stage2_GameOver,
    Stage2_Clear,
    Stage3_Tutorial,
    Stage3_Hint_01,
    Stage3_Clear,
    Stage3_GameOver,
    Stage4_Tutorial,
    Stage4_FirstG,
    Stage4_Hint_01,
    Stage4_Clear,
    Stage4_GameOver
}

public class DialogueManager : Singleton<DialogueManager>
{
    [Header("Debug")]
    [SerializeField] private Transform dialogueViewParent;

    [Header("Stage 1")]
    [SerializeField] private DialogueSequenceData stage1Tutorial;
    [SerializeField] private DialogueSequenceData stage1Hint01;
    [SerializeField] private DialogueSequenceData stage1Clear;
    [SerializeField] private DialogueSequenceData stage1GameOver;

    [Header("Stage 2")]
    [SerializeField] private DialogueSequenceData stage2Tutorial;
    [SerializeField] private DialogueSequenceData stage2Hint01;
    [SerializeField] private DialogueSequenceData stage2Clear;
    [SerializeField] private DialogueSequenceData stage2GameOver;
    
    [Header("Stage 3")]
    [SerializeField] private DialogueSequenceData stage3Tutorial;
    [SerializeField] private DialogueSequenceData stage3Hint01;
    [SerializeField] private DialogueSequenceData stage3Clear;
    [SerializeField] private DialogueSequenceData stage3GameOver;
    
    [Header("Stage 4")]
    [SerializeField] private DialogueSequenceData stage4Tutorial;
    [SerializeField] private DialogueSequenceData stage4FirstG;
    [SerializeField] private DialogueSequenceData stage4Hint01;
    [SerializeField] private DialogueSequenceData stage4Clear;
    [SerializeField] private DialogueSequenceData stage4GameOver;

    private Dictionary<DialogueKey, DialogueSequenceData> _sequenceDictionary;

    protected override void Awake()
    {
        base.Awake();

        if (Instance != this)
        {
            // 씬마다 dialogueViewParent가 다름. 새 씬의 Parent를 살아있는 싱글톤에 넘기고,
            // 이전 씬과 함께 파괴된 DialogueView의 캐시를 비워 새 Parent 아래에 다시 생성되도록 한다.
            Instance.dialogueViewParent = dialogueViewParent;
            UIManager.Instance.ClearCollections();
            return;
        }

        _sequenceDictionary = new Dictionary<DialogueKey, DialogueSequenceData>();

        AddSequence(DialogueKey.Stage1_Tutorial, stage1Tutorial);
        AddSequence(DialogueKey.Stage1_Hint_01, stage1Hint01);
        AddSequence(DialogueKey.Stage1_Clear, stage1Clear);
        AddSequence(DialogueKey.Stage1_GameOver, stage1GameOver);
        AddSequence(DialogueKey.Stage2_Tutorial, stage2Tutorial);
        AddSequence(DialogueKey.Stage2_Hint_01, stage2Hint01);
        AddSequence(DialogueKey.Stage2_Clear, stage2Clear);
        AddSequence(DialogueKey.Stage2_GameOver, stage2GameOver);
        AddSequence(DialogueKey.Stage3_Tutorial, stage3Tutorial);
        AddSequence(DialogueKey.Stage3_Hint_01, stage3Hint01);
        AddSequence(DialogueKey.Stage3_Clear, stage3Clear);
        AddSequence(DialogueKey.Stage3_GameOver, stage3GameOver);
        AddSequence(DialogueKey.Stage4_Tutorial, stage4Tutorial);
        AddSequence(DialogueKey.Stage4_FirstG, stage4FirstG);
        AddSequence(DialogueKey.Stage4_Hint_01, stage4Hint01);
        AddSequence(DialogueKey.Stage4_Clear, stage4Clear);
        AddSequence(DialogueKey.Stage4_GameOver, stage4GameOver);
    }

    public DialogueSequenceData GetSequence(DialogueKey key)
    {
        if (_sequenceDictionary != null && _sequenceDictionary.TryGetValue(key, out DialogueSequenceData sequence))
        {
            return sequence;
        }

        Debug.LogError($"{key}에 연결된 대화 시퀀스가 없습니다.", this);
        return null;
    }

    public bool HasSequence(DialogueKey key)
    {
        return _sequenceDictionary != null && _sequenceDictionary.ContainsKey(key);
    }

    [ContextMenu("Show Dialogue View")]
    public void ShowDialogueView()
    {
        DialogueView dialogueView = UIManager.Instance.GetUIComponent<DialogueView>(dialogueViewParent);
        if (dialogueView == null)
            return;

        dialogueView.Show();
    }

    public DialogueView PlayDialogue(DialogueKey key)
    {
        if (key == DialogueKey.None)
            return null;

        DialogueSequenceData sequence = GetSequence(key);
        if (sequence == null)
            return null;

        DialogueView dialogueView = UIManager.Instance.GetUIComponent<DialogueView>(dialogueViewParent);
        if (dialogueView == null)
            return null;

        dialogueView.PlaySequence(sequence);
        return dialogueView;
    }

    private void AddSequence(DialogueKey key, DialogueSequenceData sequence)
    {
        if (key == DialogueKey.None || sequence == null)
        {
            return;
        }

        _sequenceDictionary[key] = sequence;
    }
}
