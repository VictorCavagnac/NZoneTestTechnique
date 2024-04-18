using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Authentication.PlayerAccounts;
using Unity.Services.CloudSave;

public class UILogin : MonoBehaviour
{
    [Header("Listening to..")]
    [SerializeField]
    private StringEventChannelSO _onSignedIn = default;

    [Header("Login UI")]
    [SerializeField]
    private GameObject _loginPanel;

    [SerializeField]
    private Button _loginButton;

    [Header("Main UI")]
    [SerializeField]
    private GameObject _mainPanel;

    [SerializeField]
    private TMP_Text _usernameText;

    private void OnEnable() 
    {
        _loginButton.onClick.AddListener(LoginButtonPressed);

        _onSignedIn.OnEventRaised += OnSignedIn;
    }

    private async void LoginButtonPressed()
    {
        _loginButton.interactable = false;

        await LoginManager.Instance.InitSignIn();
    }

    private void OnSignedIn(string name)
    {
        _usernameText.text = name;

        _loginPanel.SetActive(false);
        _mainPanel.SetActive(true);
    }

    private void OnDisable() 
    {
        _loginButton.onClick.RemoveListener(LoginButtonPressed);

        _onSignedIn.OnEventRaised -= OnSignedIn;
    }
}
