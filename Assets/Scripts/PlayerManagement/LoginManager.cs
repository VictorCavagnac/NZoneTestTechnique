using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Authentication.PlayerAccounts;
using Unity.Services.Core;
using UnityEngine;

public class LoginManager : MonoBehaviour
{
    public static LoginManager Instance;

    [Header("Broadcasting to..")]
    [SerializeField] 
    private StringEventChannelSO _onSignedIn = default;

    private PlayerInfo _playerInfo = null;

    [HideInInspector]
    public string playerUsername = "";

    private async void Awake() 
    {
        if ( Instance == null )
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        /* ===== */

        await UnityServices.InitializeAsync();

        PlayerAccountService.Instance.SignedIn += SignedIn;
    }

    private async void SignedIn()
    {
        try
        {
            var accessToken = PlayerAccountService.Instance.AccessToken;
            await SignInWithUnityAsync(accessToken);
        }
        catch (Exception ex)
        {
            Debug.Log("<color=red>== SignIn failed : " + ex + " ==</color>");
        }
    }

    public async Task InitSignIn()
    {
        await PlayerAccountService.Instance.StartSignInAsync();
    }

    private async Task SignInWithUnityAsync(string accessToken)
    {
        try
        {
            await AuthenticationService.Instance.SignInWithUnityAsync(accessToken);
            Debug.Log("<color=green>== SignIn is successful ==</color>");

            _playerInfo = AuthenticationService.Instance.PlayerInfo;

            playerUsername = await AuthenticationService.Instance.GetPlayerNameAsync();

            _onSignedIn.RaiseEvent(playerUsername);
        }
        catch (AuthenticationException ex)
        {
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            Debug.LogException(ex);
        }
    }

    private async Task SignInAnonymouslyAsync()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Sign in anonymously succeeded!");

            _playerInfo = AuthenticationService.Instance.PlayerInfo;

            var name = await AuthenticationService.Instance.GetPlayerNameAsync();

            _onSignedIn.RaiseEvent(name);
        }
        catch (AuthenticationException ex)
        {
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            Debug.LogException(ex);
        }
    }

    private void OnDestroy() 
    {
        PlayerAccountService.Instance.SignedIn -= SignedIn;
    }
}