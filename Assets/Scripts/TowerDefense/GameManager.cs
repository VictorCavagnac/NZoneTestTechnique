using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Listening to..")]
    [SerializeField] 
    private VoidEventChannelSO _onSceneReady = default;

    private Transform _enemySpawnPoint = null;
    private Transform[] _listOfWaypoints = null;

    private void Awake()
    {
        if ( Instance == null )
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable() 
    {
        _onSceneReady.OnEventRaised += StartGame;
    }

    private void OnDisable() 
    {
        _onSceneReady.OnEventRaised -= StartGame;
    }

    /* ===== */

    private void StartGame()
    {
        InputReader.Instance.EnableGameplayInput();
    }

    public void InitiateGame(Transform enemySpawnPoint, Transform[] listOfWaypoints)
    {
        _enemySpawnPoint = enemySpawnPoint;
        _listOfWaypoints = listOfWaypoints;
    }
}