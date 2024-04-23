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

    [SerializeField]
    private VoidEventChannelSO _onRestartLevel = default;

    [Header("Broadcasting to..")]
    [SerializeField]
    private VoidEventChannelSO _requestNextWaveEvent = default;

    private Transform _spawnPoint = null;
    private Transform[] _waypoints = null;

    private int _numberOfEnemiesToWait = 0;

    private Dictionary<EnemyType, EnemySettingsSO> _listOfEnemies;
    private List<Enemy> _activeEnemies;

    private void Awake() 
    {
        InitiateAllPools();
    }

    private void OnEnable() 
    {
        _activeEnemies = new List<Enemy>();

        _onReturnEnemy.OnEventRaised += OnReturnEnemy;
        _onRestartLevel.OnEventRaised += OnRestartLevel;
    }

    private void OnDisable() 
    {
        _onReturnEnemy.OnEventRaised -= OnReturnEnemy;
        _onRestartLevel.OnEventRaised -= OnRestartLevel;

        StopAllCoroutines();
        KillAllPools();
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

    private void KillAllPools()
    {
        for ( int i = 0; i < _allEnemiesAvailable.Count; i++ )
        {
            EnemySettingsSO enemySettings = _allEnemiesAvailable[i];

            enemySettings.EnemyPool.KillPool();
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

            _activeEnemies.Add(enemy);

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

        DisposeOfEnemy(currentEnemy); 

        /* === */

        _numberOfEnemiesToWait -= 1;

        if ( _numberOfEnemiesToWait <= 0 )
        {
            _requestNextWaveEvent.RaiseEvent();
        }
    }

    private void DisposeOfEnemy(Enemy enemy)
    {
        _listOfEnemies[enemy.enemySettings.EnemyType].EnemyPool.Return(enemy);

        _activeEnemies.Remove(enemy);
    }

    /* ===== */

    private void OnRestartLevel()
    {
        StopAllCoroutines();

        int numberOfActiveEnemies = _activeEnemies.Count - 1;

        for ( int i = numberOfActiveEnemies; i >= 0; i-- )
        {
            Enemy enemy = _activeEnemies[i];

            DisposeOfEnemy(enemy);
        }
    }
}