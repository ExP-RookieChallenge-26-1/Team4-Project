using System;
using UnityEngine;

[Serializable]
public class DialogueLineData
{
    public DialogueCharacterExpression expression;
    public DialogueBubbleColor bubbleColor;
    [TextArea(2, 5)] public string text;
    public DialogueEventType eventType;
}
