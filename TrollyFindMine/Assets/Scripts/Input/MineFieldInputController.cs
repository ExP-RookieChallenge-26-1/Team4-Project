using UnityEngine;
using UnityEngine.UI;

public class MineFieldInputController : MonoBehaviour
{
    [SerializeField] private MineFieldBackend mineFieldBackend;
    [SerializeField] private VirtualJoystick virtualJoystick;
    [SerializeField] private MineCharacterController mineCharacterController;
    //Button 설정
    [SerializeField] private Button clickButton;
    [SerializeField] private Button flagButton;
    [SerializeField] private float firstRepeatDelay = 0.35f;
    [SerializeField] private float repeatInterval = 0.18f;

    private Vector2 _lastDirection;
    private float _nextMoveTime;
    private bool _waitingFirstRepeat;

    #region unity cycle

    private void Start()
    {
        //clickButton의 Event 연결
        clickButton.onClick.AddListener(mineFieldBackend.OpenCellWithLeftClick);
        //flagButton의 Event도 연결
        flagButton.onClick.AddListener(mineFieldBackend.FlagCellWithRightClick);

        //캐릭터 표정 연결 : 버튼을 누르는 동안만 SuperAngry, 조이스틱을 누르는 동안만 Angry
        if (mineCharacterController == null)
        {
            Debug.LogWarning("MineFieldInputController에 mineCharacterController가 연결되어 있지 않습니다.");
        }
        else
        {
            SetupPressExpression(clickButton.gameObject, mineCharacterController.ShowSuperAngry, mineCharacterController.ResetExpression);
            SetupPressExpression(flagButton.gameObject, mineCharacterController.ShowSuperAngry, mineCharacterController.ResetExpression);

            if (virtualJoystick == null)
            {
                Debug.LogWarning("MineFieldInputController에 virtualJoystick이 연결되어 있지 않습니다.");
            }
            else
            {
                virtualJoystick.OnPressed += mineCharacterController.ShowAngry;
                virtualJoystick.OnReleased += mineCharacterController.ResetExpression;
            }
        }
    }

    private void SetupPressExpression(GameObject target, System.Action onPressed, System.Action onReleased)
    {
        ButtonPointerEvents pointerEvents = target.GetComponent<ButtonPointerEvents>();
        if (pointerEvents == null) pointerEvents = target.AddComponent<ButtonPointerEvents>();
        pointerEvents.OnPressed += onPressed;
        pointerEvents.OnReleased += onReleased;
    }

    private void Update()
    {
        if (mineFieldBackend == null || virtualJoystick == null)
            return;

        Vector2 direction = virtualJoystick.CurrentDirection;
        if (direction == Vector2.zero)
        {
            ResetRepeatState();
            return;
        }

        if (direction != _lastDirection)
        {
            TriggerMove(direction);
            _lastDirection = direction;
            _waitingFirstRepeat = true;
            _nextMoveTime = Time.unscaledTime + firstRepeatDelay;
            return;
        }

        if (Time.unscaledTime >= _nextMoveTime)
        {
            TriggerMove(direction);
            _waitingFirstRepeat = false;
            _nextMoveTime = Time.unscaledTime + repeatInterval;
        }
    }

    #endregion
    

    public void OnOpenPressed()
    {
        if (mineFieldBackend == null)
            return;

        mineFieldBackend.OpenCellWithLeftClick();
    }

    public void OnFlagPressed()
    {
        if (mineFieldBackend == null)
            return;

        mineFieldBackend.FlagCellWithRightClick();
    }

    private void TriggerMove(Vector2 direction)
    {
        mineFieldBackend.SelectedCellMove((int)direction.x, (int)direction.y);
    }

    private void ResetRepeatState()
    {
        _lastDirection = Vector2.zero;
        _waitingFirstRepeat = false;
        _nextMoveTime = 0f;
    }
}
