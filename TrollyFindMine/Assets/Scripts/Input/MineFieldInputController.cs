using UnityEngine;

public class MineFieldInputController : MonoBehaviour
{
    [SerializeField] private MineFieldBackend mineFieldBackend;
    [SerializeField] private VirtualJoystick virtualJoystick;
    [SerializeField] private float firstRepeatDelay = 0.35f;
    [SerializeField] private float repeatInterval = 0.18f;

    private Vector2 _lastDirection;
    private float _nextMoveTime;
    private bool _waitingFirstRepeat;

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
