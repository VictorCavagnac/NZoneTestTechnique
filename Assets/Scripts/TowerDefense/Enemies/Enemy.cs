using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField]
    private float _minDistanceToChangeTarget = 0.2f;

    [Header("Broadcasting to..")]
    [SerializeField]
    private GameObjectEventChannelSO _returnEnemyEvent = default;

    [SerializeField]
    private IntEventChannelSO _enemyDefeatedEvent = default;

    [SerializeField]
    private VoidEventChannelSO _enemyAttackPlayerEvent = default;

    private Transform[] _listOfWaypoints = null;

    [HideInInspector]
    public int currentWaypointIndex = 0;
    [HideInInspector]
    public float distanceToNextWaypoint = 0f;
    [HideInInspector]
    public EnemySettingsSO enemySettings = null;

    private Transform _currentTarget = null;
    private bool _isDefeated = false;

    private int _health = 10;

    public void Initiate(Transform[] waypoints, EnemySettingsSO enemySettings)
    {
        this.enemySettings = enemySettings;
        _listOfWaypoints = waypoints;

        _isDefeated = false;
        currentWaypointIndex = 0;
        distanceToNextWaypoint = 0f;
        _currentTarget = _listOfWaypoints[0];

        SetEnemySettings();
    }

    private void SetEnemySettings()
    {
        _health = enemySettings.StartingHealth;
    }

    /* ===== */

    private void Update() 
    {
        if ( _currentTarget == null ) return;
        
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector3 currentDirection = _currentTarget.position - transform.position;

        transform.Translate(currentDirection.normalized * enemySettings.MovementSpeed * Time.deltaTime, Space.World);
        transform.LookAt(_currentTarget);

        distanceToNextWaypoint = Vector3.Distance(transform.position, _currentTarget.position);

        if ( distanceToNextWaypoint <= _minDistanceToChangeTarget )
        {
            GetNextWaypoint();
        }
    }

    private void GetNextWaypoint()
    {
        if ( currentWaypointIndex >= _listOfWaypoints.Length - 1 )
        {
            AttackPlayer();
        }
        else
        {
            transform.position = _currentTarget.position;

            _currentTarget = _listOfWaypoints[++currentWaypointIndex];
        }
    }

    /* ===== */

    public void HitEnemy(Projectile projectile)
    {
        _health -= 5;

        if ( !_isDefeated && _health <= 0 )
        {
            EnemyDefeated();
        }
    }

    private void EnemyDefeated()
    {
        _isDefeated = true;
        _enemyDefeatedEvent.RaiseEvent(enemySettings.MoneyWhenDefeated);

        DisposeEnemy();
    }

    private void AttackPlayer()
    {
        _enemyAttackPlayerEvent.RaiseEvent();

        DisposeEnemy();
    }

    private void DisposeEnemy()
    {
        _currentTarget = null;

        _returnEnemyEvent.RaiseEvent(gameObject);
    }
}