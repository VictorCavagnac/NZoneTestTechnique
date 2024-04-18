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
        _loginButton.enabled = false;

        await LoginManager.Instance.InitSignIn();
    }

    private void OnSignedIn(string name)
    {
        _usernameText.text = name;

        _loginPanel.SetActive(false);
        _mainPanel.SetActive(true);
    }

    private async void SaveData()
    {
        /*
        string playerName = await AuthenticationService.Instance.GetPlayerNameAsync();

        Debug.Log("Player name : " + playerName);

        await AuthenticationService.Instance.UpdatePlayerNameAsync("Gin");
        */

        /*
        var playerData = new Dictionary<string, object>
        {
            {"username", "TEXT"},
            {"currentLevel", 1}
        };
        ADD DATA
        var result = await CloudSaveService.Instance.Data.Player.SaveAsync(playerData);
        */

        //DELETE await CloudSaveService.Instance.Data.Player.DeleteAsync("currentLevel");

        //Debug.Log("<color=green>== " + $"Saved data {string.Join(',', playerData)} ==</color>");
    }

    private void LogoutButtonPressed()
    {
        GetData();
    }

    private async void GetData()
    {
        /*
       var playerData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string>{"currentLevel"});

       if ( playerData.TryGetValue("currentLevel", out var keyName) )
       {
            Debug.Log($"currentLevel: {keyName.Value.GetAs<int>()}");
       }
       else
       {
            Debug.Log("no");
       }
       */
    }

    private void OnDisable() 
    {
        _loginButton.onClick.RemoveListener(LoginButtonPressed);

        _onSignedIn.OnEventRaised -= OnSignedIn;
    }
}
