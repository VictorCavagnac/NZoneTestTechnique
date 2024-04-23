using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEndLevel : MonoBehaviour
{
    [Header("Return Settings")]
    [SerializeField]
    private ReturnToLoader _returnTo = null;

    [Header("Buttons Settings")]
    [SerializeField]
    private Button _restartButton = null;

    [SerializeField]
    private Button _mainMenuButton = null;

    [Header("Broadcast to..")]
    [SerializeField]
    private VoidEventChannelSO _restartLevelEvent = null;

    private void OnEnable() 
    {
        _restartButton.interactable = true;
        _mainMenuButton.interactable = true;
    }

    public void SetEndPanel(bool state)
    {

    }

    public void RestartLevel()
    {
        _restartLevelEvent.RaiseEvent();

        DisableAllButtons();
    }

    public void ReturnToMainMenu()
    {
        _returnTo.GoToMenu();

        DisableAllButtons();
    }

    private void DisableAllButtons()
    {   
        _restartButton.interactable = false;
        _mainMenuButton.interactable = false;
    }
}