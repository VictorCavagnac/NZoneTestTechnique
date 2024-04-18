using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

/// <summary>
/// Allows a "cold start" in the editor, ensuring everything is loaded when playing another scene.
/// </summary> 
public class EditorColdStartup : MonoBehaviour
{
#if UNITY_EDITOR

    [SerializeField] 
    private GameSceneSO _persistentManagersSO = default;

	[SerializeField] 
    private AssetReference _notifyColdStartupChannel = default;

	[SerializeField] 
    private VoidEventChannelSO _onSceneReadyChannel = default;

    private bool _isColdStartNeeded = false;

    private void Awake() 
    {
        // Check if the persistant scene is loaded
        if( !SceneManager.GetSceneByName(_persistentManagersSO.sceneReference.editorAsset.name).isLoaded )
        {
            _isColdStartNeeded = true;
        }
    }

    private void Start() 
    {
        if ( _isColdStartNeeded )
        {
            _persistentManagersSO.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true).Completed += OnSceneReady;
        }
    }

    private void OnSceneReady(AsyncOperationHandle<SceneInstance> obj)
    {
        _notifyColdStartupChannel.LoadAssetAsync<LoadEventChannelSO>().Completed += OnNotifyChannelLoaded;
    }

    private void OnNotifyChannelLoaded(AsyncOperationHandle<LoadEventChannelSO> obj)
	{
        _onSceneReadyChannel.RaiseEvent();
    }

#endif
}