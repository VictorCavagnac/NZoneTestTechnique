using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField]
    private float _cameraSpeed = 30f;

    [SerializeField]
    private float _zoomRate = 40f;

    [SerializeField]
    private float _zoomDampening = 5.0f; 

    [SerializeField]
    private float _maxDistanceZoom = 20f;

    [SerializeField]
    private float _minDistanceZoom = 0.6f; 	

    [Header("Listening to..")]
    [SerializeField]
    private Vector2EventChannelSO _dragEvent = null;

    [SerializeField]
    private FloatEventChannelSO _zoomEvent = null;

    [SerializeField]
    private BoolEventChannelSO _moveCameraEvent = null;

    private bool _cameraIsMoving = false;
    private Vector2 _cameraDelta = Vector2.zero;

    private float _zoomDirection = 0f;
    private float desiredDistance = 0f; 

    private float _zoomLevel = 0f;
    private float _zoomPosition = 0;

    private void OnEnable() 
    {
        _dragEvent.OnEventRaised += OnDragInput;
        _zoomEvent.OnEventRaised += OnZoomInput;
        _moveCameraEvent.OnEventRaised += OnMoveCameraInput;
    }

    private void OnDisable() 
    {
        _dragEvent.OnEventRaised -= OnDragInput;
        _zoomEvent.OnEventRaised -= OnZoomInput;
        _moveCameraEvent.OnEventRaised -= OnMoveCameraInput;
    }

    private void LateUpdate()
    {
        HandleCameraMovement();
        HandleCameraZoom();
    }

    private void HandleCameraMovement()
    {
        if ( !_cameraIsMoving ) return;

        Vector2 delta = _cameraDelta;

        Vector3 nextPosition = transform.right * (delta.x * -_cameraSpeed);
                nextPosition += transform.forward * (delta.y * -_cameraSpeed);

        nextPosition.y = 0;
        
        transform.position += nextPosition * Time.deltaTime;
    }

    private void HandleCameraZoom()
    {
        
    }

    /* ===== */

    private void OnDragInput(Vector2 input)
    {
        _cameraDelta = input;
    }

    private void OnMoveCameraInput(bool input)
    {
        _cameraIsMoving = input;
    }

    private void OnZoomInput(float input)
    {
        _zoomDirection = input;
    }
}