using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueSequence", menuName = "Dialogue/Sequence Data")]
public class DialogueSequenceData : ScriptableObject
{
    public List<DialogueLineData> lines = new();
}
