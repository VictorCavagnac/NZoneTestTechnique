using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreenManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _loadingScreen = default;

    [Header("Listening to..")]
    [SerializeField]
    private BoolEventChannelSO _toggleLoadingScreen = default;

    private void OnEnable() 
    {
        _toggleLoadingScreen.OnEventRaised += ToggleLoadingScreen;
    }

    private void OnDisable() 
    {
        _toggleLoadingScreen.OnEventRaised -= ToggleLoadingScreen;
    }

    private void ToggleLoadingScreen(bool state)
    {
        _loadingScreen.SetActive(state);
    }
}