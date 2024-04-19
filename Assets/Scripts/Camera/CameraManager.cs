using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField]
    private float _cameraSpeed = 30f;

    private void LateUpdate()
    {
        HandleCameraMovement();
    }

    private void HandleCameraMovement()
    {
        if ( !InputReader.Instance.cameraIsMoving ) return;

        Vector2 delta = InputReader.Instance.cameraDelta;

        Vector3 nextPosition = transform.right * (delta.x * -_cameraSpeed);
                nextPosition += transform.forward * (delta.y * -_cameraSpeed);

        nextPosition.y = 0;
        
        transform.position += nextPosition * Time.deltaTime;
    }
}