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
    private ButtonPointerEvents _clickButtonPointerEvents;
    private ButtonPointerEvents _flagButtonPointerEvents;
    private bool _inputActive;

    #region unity cycle

    private void Start()
    {
        //clickButton의 Event 연결
        clickButton.onClick.AddListener(mineFieldBackend.OpenCellWithLeftClick);
        clickButton.onClick.AddListener(() => SoundManager.Instance.Play(Define.SFX.fx_00_button));
        //flagButton의 Event도 연결
        flagButton.onClick.AddListener(mineFieldBackend.FlagCellWithRightClick);
        flagButton.onClick.AddListener(() => SoundManager.Instance.Play(Define.SFX.fx_00_button));

        //캐릭터 표정 연결 : 버튼을 누르는 동안만 SuperAngry, 조이스틱을 누르는 동안만 Angry
        if (mineCharacterController == null)
        {
            Debug.LogWarning("MineFieldInputController에 mineCharacterController가 연결되어 있지 않습니다.");
        }
        else
        {
            _clickButtonPointerEvents = SetupPressExpression(clickButton.gameObject, mineCharacterController.ShowSuperAngry, mineCharacterController.ResetExpression);
            _flagButtonPointerEvents = SetupPressExpression(flagButton.gameObject, mineCharacterController.ShowSuperAngry, mineCharacterController.ResetExpression);

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

        //InitGrid 완료 전까지는 입력 비활성화, 완료되면 활성화, 게임이 끝나면 다시 비활성화
        SetInputActive(false);
        mineFieldBackend.OnGridInitialized += () => SetInputActive(true);
        mineFieldBackend.OnGameOver += () => SetInputActive(false);
        mineFieldBackend.OnGameWin += () => SetInputActive(false);
    }

    private ButtonPointerEvents SetupPressExpression(GameObject target, System.Action onPressed, System.Action onReleased)
    {
        ButtonPointerEvents pointerEvents = target.GetComponent<ButtonPointerEvents>();
        if (pointerEvents == null) pointerEvents = target.AddComponent<ButtonPointerEvents>();
        pointerEvents.OnPressed += onPressed;
        pointerEvents.OnReleased += onReleased;
        return pointerEvents;
    }

    public void SetInputActive(bool active)
    {
        _inputActive = active;
        clickButton.interactable = active;
        flagButton.interactable = active;
        if (virtualJoystick != null) virtualJoystick.enabled = active;
        if (_clickButtonPointerEvents != null) _clickButtonPointerEvents.enabled = active;
        if (_flagButtonPointerEvents != null) _flagButtonPointerEvents.enabled = active;
        if (!active) ResetRepeatState();
    }

    private void Update()
    {
        if (!_inputActive || mineFieldBackend == null || virtualJoystick == null)
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
