using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISpinner : MonoBehaviour
{
    [SerializeField]
    private RectTransform _spinningRect = null;

    [SerializeField]
    private float _rotationSpeed = 200f;

    private void Update() 
    {
        _spinningRect.Rotate(0, 0, _rotationSpeed * Time.deltaTime);
    }
}