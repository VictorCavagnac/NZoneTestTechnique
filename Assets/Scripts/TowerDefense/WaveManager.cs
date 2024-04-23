using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Enemies available")]
    [SerializeField]
    private List<EnemySettingsSO> _allEnemiesAvailable = null;

    [Header("Common Settings")]
    [SerializeField]
    private float _waitTimeWave = 1f;

    [Header("Listening to..")]
    [SerializeField]
    private GameObjectEventChannelSO _onReturnEnemy = default;

    [Header("Broadcasting to..")]
    [SerializeField]
    private VoidEventChannelSO _requestNextWaveEvent = default;

    private Transform _spawnPoint = null;
    private Transform[] _waypoints = null;

    private int _numberOfEnemiesToWait = 0;

    private Dictionary<EnemyType, EnemySettingsSO> _listOfEnemies;

    private void Awake() 
    {
        InitiateAllPools();
    }

    private void OnEnable() 
    {
        _onReturnEnemy.OnEventRaised += OnReturnEnemy;
    }

    private void OnDisable() 
    {
        _onReturnEnemy.OnEventRaised -= OnReturnEnemy;

        StopAllCoroutines();
    }

    private void InitiateAllPools()
    {
        _listOfEnemies = new Dictionary<EnemyType, EnemySettingsSO>();

        for ( int i = 0; i < _allEnemiesAvailable.Count; i++ )
        {
            EnemySettingsSO enemySettings = _allEnemiesAvailable[i];

            _listOfEnemies.Add(enemySettings.EnemyType, enemySettings);

            enemySettings.EnemyPool.Prewarm(enemySettings.PrewarmNumber);
        }
    }

    public void SetSpawnPoint(Transform spawnPoint, Transform[] wp)
    {
        _spawnPoint = spawnPoint;
        _waypoints = wp;
    }

    public void StartNextWave(WaveSettingsSO wave)
    {
        _numberOfEnemiesToWait = GetNumberOfEnemies(wave);

        StartCoroutine(SpawnWaveEnemies(wave));
    }

    private IEnumerator SpawnWaveEnemies(WaveSettingsSO wave)
    {
        yield return new WaitForSeconds(_waitTimeWave);

        for ( int i = 0; i < wave.SpawnGroups.Count; i++ )
        {
            SpawnGroups groupOfEnemies = wave.SpawnGroups[i];

            yield return new WaitForSeconds(groupOfEnemies.InitialSpawnDelay);
            StartCoroutine(SpawnEnemiesInGroup(groupOfEnemies));
        }
    }

    private IEnumerator SpawnEnemiesInGroup(SpawnGroups groupOfEnemies)
    {
        for ( int i = 0; i < groupOfEnemies.NumberOfEnemies; i++ )
        {
            EnemySettingsSO enemySettings = _listOfEnemies[groupOfEnemies.EnemyType];
            EnemyPoolSO enemyPool = enemySettings.EnemyPool;

            Enemy enemy = enemyPool.Request();
            enemy.transform.position = _spawnPoint.position;
            enemy.Initiate(_waypoints, enemySettings);

            yield return new WaitForSeconds(groupOfEnemies.TimeBetweenEnemies);
        }
    }

    private int GetNumberOfEnemies(WaveSettingsSO wave)
    {
        int numberOfEnemies = 0;

        for ( int i = 0; i < wave.SpawnGroups.Count; i++ )
        {
            SpawnGroups groupOfEnemies = wave.SpawnGroups[i];

            numberOfEnemies += groupOfEnemies.NumberOfEnemies;
        }

        return numberOfEnemies;
    }

    private void OnReturnEnemy(GameObject enemy)
    {
        Enemy currentEnemy = enemy.GetComponent<Enemy>();

        _listOfEnemies[currentEnemy.enemySettings.EnemyType].EnemyPool.Return(currentEnemy);

        /* === */

        _numberOfEnemiesToWait -= 1;

        if ( _numberOfEnemiesToWait <= 0 )
        {
            _requestNextWaveEvent.RaiseEvent();
        }
    }
}