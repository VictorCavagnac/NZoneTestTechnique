using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Enemy1,
    Enemy2
}

[CreateAssetMenu(fileName = "EnemySettings", menuName = "TowerDefense/Enemy Settings")]
public class EnemySettingsSO : ScriptableObject
{
    [Header("Type")]
    [SerializeField]
    private EnemyType _enemyType = default;

    [Header("Enemy Settings")]
    [SerializeField]
    private string _enemyName = "";

    [SerializeField]
    private float _movementSpeed = 10f;

    [SerializeField]
    private int _startingHealth = 10;

    [SerializeField]
    private int _moneyWhenDefeated = 30;

    [Header("Pool Settings")]
    [SerializeField]
    private EnemyPoolSO _enemyPool = null;

    [SerializeField]
    private int _prewarmNumber = 20;

    public EnemyType EnemyType => _enemyType;

    public string EnemyName => _enemyName;
    public float MovementSpeed => _movementSpeed;
    public int StartingHealth => _startingHealth;
    public int MoneyWhenDefeated => _moneyWhenDefeated;

    public EnemyPoolSO EnemyPool => _enemyPool;
    public int PrewarmNumber => _prewarmNumber;
}