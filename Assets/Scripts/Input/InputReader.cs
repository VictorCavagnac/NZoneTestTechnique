using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    public static InputReader Instance;

    public Vector2 cameraDelta = Vector2.zero;
    public bool cameraIsMoving = false;

    private GameControls _gameControls = null;

    private void Awake()
    {
        if ( Instance == null )
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable() 
    {
        if ( _gameControls == null )
        {
            _gameControls = new GameControls();

            _gameControls.Gameplay.Drag.performed     += i => cameraDelta = i.ReadValue<Vector2>();

            _gameControls.Gameplay.MoveCamera.performed += i => cameraIsMoving = true;
            _gameControls.Gameplay.MoveCamera.canceled  += i => cameraIsMoving = false;
        }
    }

    private void OnDisable() 
    {
        DisableAllInput();
    }

    /* ===== */

    public void EnableGameplayInput()
    {
        _gameControls.Gameplay.Enable();
    }

    public void DisableAllInput()
    {
        _gameControls.Gameplay.Disable();
    }
}
