using System.Collections.Generic;
using UnityEngine;

public enum DialogueKey
{
    None,
    Stage1_Tutorial,
    Stage1_Hint_01,
    Stage1_Clear,
    Stage2_Tutorial,
    Stage2_Hint_01,
    Stage2_Clear
}

public class DialogueManager : Singleton<DialogueManager>
{
    [Header("Stage 1")]
    [SerializeField] private DialogueSequenceData stage1Tutorial;
    [SerializeField] private DialogueSequenceData stage1Hint01;
    [SerializeField] private DialogueSequenceData stage1Clear;

    [Header("Stage 2")]
    [SerializeField] private DialogueSequenceData stage2Tutorial;
    [SerializeField] private DialogueSequenceData stage2Hint01;
    [SerializeField] private DialogueSequenceData stage2Clear;

    private Dictionary<DialogueKey, DialogueSequenceData> _sequenceDictionary;

    protected override void Awake()
    {
        base.Awake();

        if (Instance != this)
        {
            return;
        }

        _sequenceDictionary = new Dictionary<DialogueKey, DialogueSequenceData>();

        AddSequence(DialogueKey.Stage1_Tutorial, stage1Tutorial);
        AddSequence(DialogueKey.Stage1_Hint_01, stage1Hint01);
        AddSequence(DialogueKey.Stage1_Clear, stage1Clear);
        AddSequence(DialogueKey.Stage2_Tutorial, stage2Tutorial);
        AddSequence(DialogueKey.Stage2_Hint_01, stage2Hint01);
        AddSequence(DialogueKey.Stage2_Clear, stage2Clear);
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

    private void AddSequence(DialogueKey key, DialogueSequenceData sequence)
    {
        if (key == DialogueKey.None || sequence == null)
        {
            return;
        }

        _sequenceDictionary[key] = sequence;
    }
}
