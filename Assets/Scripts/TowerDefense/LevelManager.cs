using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Level Settings")]
    [SerializeField]
    private Transform _spawnPoint = null;

    [SerializeField]
    private WaypointsWrapper _waypointWrapper = null;

    [Header("Listening to..")]
    [SerializeField] 
    private VoidEventChannelSO _onSceneReady = default;

    private void OnEnable() 
    {
        _onSceneReady.OnEventRaised += SendLevelInfo;
    }

    private void OnDisable() 
    {
        _onSceneReady.OnEventRaised -= SendLevelInfo;
    }

    private void SendLevelInfo()
    {
        GameManager.Instance.InitiateGame(_spawnPoint, _waypointWrapper.GetListOfWaypoints());
    }
}