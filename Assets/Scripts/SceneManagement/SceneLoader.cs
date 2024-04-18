using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] 
    private GameSceneSO _gameplayScene = default;

    [Header("Listening to..")]
    [SerializeField] 
    private LoadEventChannelSO _loadLevel = default;

	[SerializeField] 
    private LoadEventChannelSO _loadMenu = default;

	[SerializeField] 
    private LoadEventChannelSO _coldStartup = default;

    [Header("Broadcasting to..")]
    [SerializeField] 
    private BoolEventChannelSO _toggleLoadingScreen = default;

	[SerializeField] 
    private VoidEventChannelSO _onSceneReady = default;

	[SerializeField]
	private FadeChannelSO _fadeRequestChannel = default;

    private AsyncOperationHandle<SceneInstance> _loadingOpHandle;
	private AsyncOperationHandle<SceneInstance> _gameplayManagerLoadingOpHandle;

	private GameSceneSO _sceneToLoad;
	private GameSceneSO _currentlyLoadedScene;
	private bool _showLoadingScreen;

	private SceneInstance _gameplayManagerSceneInstance = new SceneInstance();
	private float _fadeDuration = .5f;
	private bool _isLoading = false; 

    private void OnEnable()
	{
		_loadLevel.OnLoadingRequested += LoadLevel;
		_loadMenu.OnLoadingRequested += LoadMenu;

#if UNITY_EDITOR
		_coldStartup.OnLoadingRequested += ColdStartup;
#endif

	}

	private void OnDisable()
	{
		_loadLevel.OnLoadingRequested -= LoadLevel;
		_loadMenu.OnLoadingRequested -= LoadMenu;

#if UNITY_EDITOR
		_coldStartup.OnLoadingRequested -= ColdStartup;
#endif

	}

	/* ===== */

#if UNITY_EDITOR
	private void ColdStartup(GameSceneSO currentlyOpenedLevel, bool showLoadingScreen, bool fadeScreen)
	{
		_currentlyLoadedScene = currentlyOpenedLevel;

		if (_currentlyLoadedScene.sceneType == GameSceneSO.GameSceneType.Level)
		{
			// Gameplay managers is loaded synchronously
			_gameplayManagerLoadingOpHandle = _gameplayScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
			_gameplayManagerLoadingOpHandle.WaitForCompletion();
			_gameplayManagerSceneInstance = _gameplayManagerLoadingOpHandle.Result;

			StartGameplay();
		}
	}
#endif

	private void LoadLevel(GameSceneSO levelToLoad, bool showLoadingScreen, bool fadeScreen)
	{
		// Prevent a double-loading
		if (_isLoading) return;

		_sceneToLoad = levelToLoad;
		_showLoadingScreen = showLoadingScreen;
		_isLoading = true;

		// In case we are coming from the main menu, we need to load the Gameplay manager scene first
		if ( _gameplayManagerSceneInstance.Scene == null || !_gameplayManagerSceneInstance.Scene.isLoaded )
		{
			_gameplayManagerLoadingOpHandle = _gameplayScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
			_gameplayManagerLoadingOpHandle.Completed += OnGameplayManagersLoaded;
		}
		else
		{
			StartCoroutine(UnloadPreviousScene());
		}
	}

	private void OnGameplayManagersLoaded(AsyncOperationHandle<SceneInstance> obj)
	{
		_gameplayManagerSceneInstance = _gameplayManagerLoadingOpHandle.Result;

		StartCoroutine(UnloadPreviousScene());
	}

    /* ===== */

    /// <summary>
	/// Prepares to load the main menu scene, first removing the Gameplay scene in case the game is coming back from gameplay to menus.
	/// </summary>
	private void LoadMenu(GameSceneSO menuToLoad, bool showLoadingScreen, bool fadeScreen)
	{
		// Prevent a double-loading
		if (_isLoading) return;

		_sceneToLoad = menuToLoad;
		_showLoadingScreen = showLoadingScreen;
		_isLoading = true;

		//In case we are coming from a Level back to the main menu, we need to get rid of the persistent Gameplay manager scene
		if (_gameplayManagerSceneInstance.Scene != null && _gameplayManagerSceneInstance.Scene.isLoaded)
        {
            Addressables.UnloadSceneAsync(_gameplayManagerLoadingOpHandle, true);
        }

		StartCoroutine(UnloadPreviousScene());
	}

    /* ===== */

    /// <summary>
	/// In both Level and Menu loading, this function takes care of removing previously loaded scenes.
	/// </summary>
	private IEnumerator UnloadPreviousScene()
	{
		//_inputReader.DisableAllInput();
		_fadeRequestChannel.FadeOut(_fadeDuration);

		yield return new WaitForSeconds(_fadeDuration);

		if (_currentlyLoadedScene != null) //would be null if the game was started in Initialisation
		{
			if (_currentlyLoadedScene.sceneReference.OperationHandle.IsValid())
			{
				//Unload the scene through its AssetReference, i.e. through the Addressable system
				_currentlyLoadedScene.sceneReference.UnLoadScene();
			}
#if UNITY_EDITOR
			else
			{
				//Only used when, after a "cold start", the player moves to a new scene
				//Since the AsyncOperationHandle has not been used (the scene was already open in the editor),
				//the scene needs to be unloaded using regular SceneManager instead of as an Addressable
				SceneManager.UnloadSceneAsync(_currentlyLoadedScene.sceneReference.editorAsset.name);
			}
#endif
		}

		LoadNewScene();
	}

    /// <summary>
	/// Kicks off the asynchronous loading of a scene, either menu or Level.
	/// </summary>
	private void LoadNewScene()
	{
		if (_showLoadingScreen)
		{
			_toggleLoadingScreen.RaiseEvent(true);
		}

		_loadingOpHandle = _sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true, 0);
		_loadingOpHandle.Completed += OnNewSceneLoaded;
	}

    /* ===== */

    private void OnNewSceneLoaded(AsyncOperationHandle<SceneInstance> obj)
	{
		//Save loaded scenes (to be unloaded at next load request)
		_currentlyLoadedScene = _sceneToLoad;

		Scene s = obj.Result.Scene;
		SceneManager.SetActiveScene(s);

		_isLoading = false;

		if (_showLoadingScreen)
        {
			_toggleLoadingScreen.RaiseEvent(false);
        }

		_fadeRequestChannel.FadeIn(_fadeDuration);

		StartGameplay();
	}

	private void StartGameplay()
	{
		_onSceneReady.RaiseEvent();
	}
}