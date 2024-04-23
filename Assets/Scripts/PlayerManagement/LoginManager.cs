using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Authentication.PlayerAccounts;
using Unity.Services.Core;
using UnityEngine;


/// <summary>
/// This class manages the first login of the player
/// </summary>
public class LoginManager : MonoBehaviour
{
    public static LoginManager Instance;

    [Header("Broadcasting to..")]
    [SerializeField] 
    private StringEventChannelSO _onSignedIn = default;

    private PlayerInfo _playerInfo = null;

    [HideInInspector]
    public string playerUsername = "";

    public bool isSignedIn = false;

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

    public async void SignedIn()
    {
        try
        {
            /*
            var accessToken = PlayerAccountService.Instance.AccessToken;
            await SignInWithUnityAsync(accessToken);
            */

            Debug.Log("Try sign in anon");

            await SignInAnonymouslyAsync();
        }
        catch (Exception ex)
        {
            Debug.Log("<color=red>== SignIn failed : " + ex + " ==</color>");
        }
    }

    /// <summary>
	/// Called by the first button through the Login UI, launching the correct event
	/// </summary>
    public async Task InitSignIn()
    {
        await PlayerAccountService.Instance.StartSignInAsync();
    }

    /// <summary>
	/// Launch the Unity Auth services and gets back the player infos used by the SaveManager
	/// </summary>
    public async Task SignInWithUnityAsync(string accessToken)
    {
        try
        {
            await AuthenticationService.Instance.SignInWithUnityAsync(accessToken);
            Debug.Log("<color=green>== SignIn is successful ==</color>");

            _playerInfo = AuthenticationService.Instance.PlayerInfo;
            isSignedIn = true;

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

    public async Task SignInAnonymouslyAsync()
    {
        try
        {
            Debug.Log("Before..");

            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Sign in anonymously succeeded!");

            _playerInfo = AuthenticationService.Instance.PlayerInfo;
            isSignedIn = true;

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