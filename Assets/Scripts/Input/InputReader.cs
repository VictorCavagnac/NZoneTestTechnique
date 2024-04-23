using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, GameControls.IGameplayActions
{
    [Header("Common Settings")]
    [SerializeField]
    private Camera _camera = null;

    [SerializeField]
    private LayerMask _placementLayerMask = default;

    [Header("Listening to..")]
    [SerializeField]
    private VoidEventChannelSO _activateGameplayInput = default;

    [SerializeField]
    private VoidEventChannelSO _disableAllInput = default;

    [Header("Broadcasting to..")]
    [SerializeField]
    private Vector2EventChannelSO _dragEvent = default;

    [SerializeField]
    private BoolEventChannelSO _moveCameraEvent = default;

    [SerializeField]
    private Vector3EventChannelSO _cursorWorldPosEvent = default;

    [SerializeField]
    private VoidEventChannelSO _selectEvent = default;

    [SerializeField]
    private FloatEventChannelSO _zoomEvent = default;

    private GameControls _gameControls = null;

    private Vector3 lastPositionHit = Vector3.zero;

    private void OnEnable() 
    {
        if ( _gameControls == null )
        {
            _gameControls = new GameControls();

            _gameControls.Gameplay.SetCallbacks(this);
        }

        _activateGameplayInput.OnEventRaised += EnableGameplayInput;
        _disableAllInput.OnEventRaised += DisableAllInput;
    }

    private void OnDisable() 
    {
        DisableAllInput();

        _activateGameplayInput.OnEventRaised -= EnableGameplayInput;
        _disableAllInput.OnEventRaised -= DisableAllInput;
    }

    public void EnableGameplayInput()
    {
        _gameControls.Gameplay.Enable();
    }

    public void DisableAllInput()
    {
        _gameControls.Gameplay.Disable();
    }

    /* ===== */

    public void OnDrag(InputAction.CallbackContext context)
    {
        if ( context.phase == InputActionPhase.Performed )
        {
            _dragEvent.RaiseEvent(context.ReadValue<Vector2>());
        }
    }

    public void OnMoveCamera(InputAction.CallbackContext context)
    {
        if ( context.phase == InputActionPhase.Performed )
        {
            _moveCameraEvent.RaiseEvent(true);
        }
        else if ( context.phase == InputActionPhase.Canceled )
        {
            _moveCameraEvent.RaiseEvent(false);
        }
    }

    public void OnPosition(InputAction.CallbackContext context)
    {
        if ( context.phase == InputActionPhase.Performed )
        {
            _cursorWorldPosEvent.RaiseEvent(GetCursorWorldPosition(context.ReadValue<Vector2>()));
        }
    }

    public Vector3 GetCursorWorldPosition(Vector2 cursorPosition)
    {
        Ray ray = _camera.ScreenPointToRay(cursorPosition);
        RaycastHit hit;

        if ( Physics.Raycast(ray, out hit, 100, _placementLayerMask) )
        {
            lastPositionHit = hit.point;
        }

        return lastPositionHit;
    }

    public void OnSelect(InputAction.CallbackContext context)
    {
        if ( context.phase == InputActionPhase.Canceled )
        {
            _selectEvent.RaiseEvent();
        }
    }

    public void OnZoom(InputAction.CallbackContext context)
    {
        _zoomEvent.RaiseEvent(context.ReadValue<float>());
    }
}
