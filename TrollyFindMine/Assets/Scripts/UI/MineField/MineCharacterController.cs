using UnityEngine;
using UnityEngine.UI;

public class MineCharacterController : MonoBehaviour
{
    #region field

    [SerializeField] private Image characterImage;
    [SerializeField] private MineFieldBackend mineFieldBackend;

    [SerializeField] private Sprite mineCharacterHappy;
    [SerializeField] private Sprite mineCharacterAngry;
    [SerializeField] private Sprite mineCharacterSuperAngry;
    [SerializeField] private Sprite mineCharacterSad;

    private bool isGameOver = false;

    #endregion

    #region Public Methods

    public void ShowAngry()
    {
        if (isGameOver) return;
        SetExpression(mineCharacterAngry);
    }

    public void ShowSuperAngry()
    {
        if (isGameOver) return;
        SetExpression(mineCharacterSuperAngry);
    }

    public void ResetExpression()
    {
        if (isGameOver) return;
        SetExpression(mineCharacterHappy);
    }

    #endregion

    #region Private Methods

    private void SetExpression(Sprite sprite)
    {
        if (characterImage == null) return;
        characterImage.sprite = sprite;
    }

    private void HandleGameOver()
    {
        isGameOver = true;
        SetExpression(mineCharacterSad);
    }

    #endregion

    #region unity cycle

    private void Start()
    {
        SetExpression(mineCharacterHappy);
    }

    private void OnEnable()
    {
        if (mineFieldBackend != null) mineFieldBackend.OnGameOver += HandleGameOver;
    }

    private void OnDisable()
    {
        if (mineFieldBackend != null) mineFieldBackend.OnGameOver -= HandleGameOver;
    }

    #endregion
}
