using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPauseMenu : MonoBehaviour
{
    [Header("Return Settings")]
    [SerializeField]
    private ReturnToLoader _returnTo = null;

    [Header("Buttons Settings")]
    [SerializeField]
    private Button _closeButton = null;

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
        _closeButton.interactable = true;

        Time.timeScale = 0;
    }

    private void OnDisable() 
    {
        RestartTime();
    }

    public void ClosePauseMenu()
    {
        gameObject.SetActive(false);

        DisableAllButtons();
    }

    public void RestartLevel()
    {
        RestartTime();
        _restartLevelEvent.RaiseEvent();

        DisableAllButtons();
    }

    public void ReturnToMainMenu()
    {
        RestartTime();
        _returnTo.GoToMenu();

        DisableAllButtons();
    }

    private void RestartTime()
    {
        Time.timeScale = 1;
    }

    private void DisableAllButtons()
    {   
        _restartButton.interactable = false;
        _mainMenuButton.interactable = false;
        _closeButton.interactable = false;
    }
}
