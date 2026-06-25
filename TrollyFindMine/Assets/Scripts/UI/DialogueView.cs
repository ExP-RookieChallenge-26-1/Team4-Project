using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueView : UIViewStackable
{
    [Header("Layout")]
    [SerializeField] private Vector2 referenceResolution = new Vector2(1440f, 3200f);
    [SerializeField] private float referenceScale = 1f;

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
    private Vector2 _referenceAnchoredPosition;

    public event Action OnSequenceFinished;

    private void Awake()
    {
        CacheReferenceAnchoredPosition();

        if (nextButton != null)
        {
            nextButton.onClick.AddListener(ShowNextLine);
            nextButton.onClick.AddListener(() => SoundManager.Instance.Play(Define.SFX.fx_00_button));
        }

        if (previousButton != null)
        {
            previousButton.onClick.AddListener(ShowPreviousLine);
            previousButton.onClick.AddListener(() => SoundManager.Instance.Play(Define.SFX.fx_00_button));
        }
    }

    public override void Show()
    {
        base.Show();
        ApplyRootScale();
    }

    public void PlaySequence(DialogueSequenceData sequence)
    {
        _currentSequence = sequence;
        _currentLineIndex = -1;
        UpdateButtonStates();

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
        UpdateButtonStates();
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
        UpdateButtonStates();
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

    private void ApplyRootScale()
    {
        if (transform.parent is not RectTransform parentRectTransform)
            return;

        if (referenceResolution.x <= 0f || referenceResolution.y <= 0f)
            return;

        float parentWidth = parentRectTransform.rect.width;
        float parentHeight = parentRectTransform.rect.height;

        if (parentWidth <= 0f || parentHeight <= 0f)
            return;

        float scale = Mathf.Min(parentWidth / referenceResolution.x, parentHeight / referenceResolution.y);
        transform.localScale = Vector3.one * scale * referenceScale;

        if (transform is RectTransform rectTransform)
        {
            rectTransform.anchoredPosition = _referenceAnchoredPosition * scale;
        }
    }

    private void CacheReferenceAnchoredPosition()
    {
        if (transform is RectTransform rectTransform)
        {
            _referenceAnchoredPosition = rectTransform.anchoredPosition;
        }
    }

    private void UpdateButtonStates()
    {
        if (previousButton != null)
        {
            previousButton.interactable = _currentLineIndex > 0;
        }
    }
}
