using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueView : UIViewStackable
{
    [Header("UI References")]
    [SerializeField] private Image characterImage;
    [SerializeField] private Image bubbleImage;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button previousButton;

    [Header("Sprites")]
    [SerializeField] private Sprite[] expressionSprites;
    [SerializeField] private Sprite[] bubbleSprites;

    private DialogueSequenceData _currentSequence;
    private int _currentLineIndex = -1;

    public event Action OnSequenceFinished;

    private void Awake()
    {
        if (nextButton != null)
        {
            nextButton.onClick.AddListener(ShowNextLine);
        }

        if (previousButton != null)
        {
            previousButton.onClick.AddListener(ShowPreviousLine);
        }
    }

    public void PlaySequence(DialogueSequenceData sequence)
    {
        _currentSequence = sequence;
        _currentLineIndex = -1;

        if (_currentSequence == null || _currentSequence.lines == null || _currentSequence.lines.Count == 0)
        {
            FinishSequence();
            return;
        }

        Show();
        ShowNextLine();
    }

    public void ShowNextLine()
    {
        if (_currentSequence == null || _currentSequence.lines == null)
            return;

        int nextIndex = _currentLineIndex + 1;
        if (nextIndex >= _currentSequence.lines.Count)
        {
            FinishSequence();
            return;
        }

        ShowLine(nextIndex);
    }

    public void ShowPreviousLine()
    {
        if (_currentSequence == null || _currentSequence.lines == null)
            return;

        int previousIndex = _currentLineIndex - 1;
        if (previousIndex < 0)
            return;

        ShowLine(previousIndex);
    }

    public void ShowLine(int lineIndex)
    {
        if (_currentSequence == null || _currentSequence.lines == null)
            return;

        if (lineIndex < 0 || lineIndex >= _currentSequence.lines.Count)
            return;

        _currentLineIndex = lineIndex;
        ApplyLine(_currentSequence.lines[_currentLineIndex]);
    }

    public void ApplyLine(DialogueLineData line)
    {
        if (line == null)
            return;

        SetCharacterExpression(line.expression);
        SetBubbleColor(line.bubbleColor);
        SetText(line.text);
        ExecuteDialogueEvent(line.eventType);
    }

    public void SetText(string text)
    {
        if (dialogueText != null)
        {
            dialogueText.text = text;
        }
    }

    public void SetCharacterExpression(DialogueCharacterExpression expression)
    {
        SetImageSprite(characterImage, expressionSprites, (int)expression);
    }

    public void SetBubbleColor(DialogueBubbleColor bubbleColor)
    {
        SetImageSprite(bubbleImage, bubbleSprites, (int)bubbleColor);
    }

    public void FinishSequence()
    {
        Hide();
        OnSequenceFinished?.Invoke();
    }

    private void ExecuteDialogueEvent(DialogueEventType eventType)
    {
        switch (eventType)
        {
            case DialogueEventType.None:
                break;
            case DialogueEventType.ButtonHighlight:
                Debug.Log("Dialogue event requested: ButtonHighlight", this);
                break;
        }
    }

    private void SetImageSprite(Image targetImage, Sprite[] sprites, int spriteIndex)
    {
        if (targetImage == null)
            return;

        if (sprites == null || spriteIndex < 0 || spriteIndex >= sprites.Length || sprites[spriteIndex] == null)
        {
            targetImage.enabled = false;
            return;
        }

        targetImage.sprite = sprites[spriteIndex];
        targetImage.enabled = true;
    }
}
