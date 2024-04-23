using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnGroups
{
    [SerializeField]
    private EnemyType _enemyType = default;

    /// <summary>
    /// Don't forget to properly setup the initial delay for each spawn group
    /// It's the delay before the spawn group starts its first enemy spawn
    /// </summary>
    [SerializeField]
    private float _initialSpawnDelay = 0f;

    [SerializeField]
    private int _numberOfEnemies = 10;

    [SerializeField]
    private float _timeBetweenEnemies = 0.5f;

    public EnemyType EnemyType => _enemyType;
    public float InitialSpawnDelay => _initialSpawnDelay;
    public int NumberOfEnemies => _numberOfEnemies;
    public float TimeBetweenEnemies => _timeBetweenEnemies;
}

[CreateAssetMenu(fileName = "WaveSettings", menuName = "TowerDefense/Wave Settings")]
public class WaveSettingsSO : ScriptableObject
{
    [Header("Basic Settings")]
    [SerializeField]
    private List<SpawnGroups> _spawnGroups = default;

    public List<SpawnGroups> SpawnGroups => _spawnGroups;
}