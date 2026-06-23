using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform joystickBase;
    [SerializeField] private float maxRadius = 240f;
    [SerializeField] private float deadZone = 60f;

    private Vector2 _inputVector;
    private Vector2 _currentDirection;
    private Canvas _parentCanvas;
    private Camera _uiCamera;

    public Vector2 CurrentDirection => _currentDirection;
    public bool HasDirection => _currentDirection != Vector2.zero;

    private void Awake()
    {
        _parentCanvas = GetComponentInParent<Canvas>();
        if (_parentCanvas != null && _parentCanvas.renderMode != RenderMode.ScreenSpaceOverlay)
        {
            _uiCamera = _parentCanvas.worldCamera;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        UpdateJoystick(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        UpdateJoystick(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _inputVector = Vector2.zero;
        _currentDirection = Vector2.zero;
    }

    private void UpdateJoystick(PointerEventData eventData)
    {
        if (joystickBase == null)
            return;

        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                joystickBase,
                eventData.position,
                _uiCamera,
                out Vector2 localPoint))
        {
            return;
        }

        Vector2 clampedPoint = Vector2.ClampMagnitude(localPoint, maxRadius);

        if (clampedPoint.magnitude < deadZone)
        {
            _inputVector = Vector2.zero;
            _currentDirection = Vector2.zero;
            return;
        }

        _inputVector = clampedPoint / maxRadius;
        _currentDirection = QuantizeDirection(_inputVector);
    }

    private Vector2 QuantizeDirection(Vector2 input)
    {
        if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
        {
            return new Vector2(Mathf.Sign(input.x), 0f);
        }

        return new Vector2(0f, Mathf.Sign(input.y));
    }
}
