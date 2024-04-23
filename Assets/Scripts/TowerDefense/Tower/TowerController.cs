using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum TowerTargetType
{
    First,
    Last,
    Closest
}

public enum TowerState
{
    Placing,
    Idle,
    Cooldown
}

public class TowerController : MonoBehaviour
{
    [Header("Tower Settings")]
    [SerializeField]
    private TowerSettingsSO _towerSettings = null;

    [SerializeField]
    private ProjectilePoolSO _basicProjectilePool = null;

    [SerializeField]
    private CapsuleCollider _towerRangeCollider = null;

    [SerializeField]
    private GameObject _towerRangeVisual = null;

    [Header("Listening to..")]
    [SerializeField]
    private VoidEventChannelSO _onTowerPlaced = null;

    [SerializeField]
    private GameObjectEventChannelSO _enemyDefeatedEvent = default; 

    private List<Enemy> _listOfEnemies = null;

    private TowerState _towerState = TowerState.Placing;

    private void Awake() 
    {
        _listOfEnemies = new List<Enemy>();

        _basicProjectilePool.Prewarm(4);
    }

    private void OnEnable() 
    {
        _onTowerPlaced.OnEventRaised += OnTowerPlaced;
        _enemyDefeatedEvent.OnEventRaised += OnEnemyDefeated;

        SetTowerSettings();
    }

    private void OnDisable() 
    {
        _onTowerPlaced.OnEventRaised -= OnTowerPlaced;
        _enemyDefeatedEvent.OnEventRaised -= OnEnemyDefeated;
    }

    private void SetTowerSettings()
    {
        _towerRangeCollider.radius = _towerSettings.AttackRange;
        _towerRangeVisual.transform.localScale = new Vector3(_towerSettings.AttackRange * 2, 0.005f, _towerSettings.AttackRange * 2);
    }

    private void OnTowerPlaced()
    {
        _towerState = TowerState.Idle;

        // Don't listen to the event since it's used for one tower at a time
        _onTowerPlaced.OnEventRaised -= OnTowerPlaced;

        // Immediately try to attack any enemy
        TryAttack();
    }
    
    private void TryAttack()
    {
        // The tower is not ready to attack (cooldown still running)
        if ( _towerState != TowerState.Idle ) return;

        // No enemies to attack
        if ( _listOfEnemies.Count <= 0 ) return;

        Enemy enemyToAttack = DetermineTarget();
        AttackEnemy(enemyToAttack);

        StartCoroutine(AttackCooldownTimer());
    }

    IEnumerator AttackCooldownTimer()
    {
        _towerState = TowerState.Cooldown;

        yield return new WaitForSeconds(_towerSettings.AttackCooldown);

        _towerState = TowerState.Idle;

        TryAttack();
    }

    private void AttackEnemy(Enemy enemy)
    {
        BasicProjectile projectile = (BasicProjectile)_basicProjectilePool.Request();

        projectile.transform.position = transform.position;
        projectile.Initiate(enemy, this);
    }

    public void ProjectileHit(Projectile projectile)
    {
        _basicProjectilePool.Return(projectile);
    }

    /* ===== */

    private Enemy DetermineTarget()
    {
        switch ( _towerSettings.TowerType )
        {
            case TowerTargetType.First:
                return GetFirstEnemy(); 
            case TowerTargetType.Last:
                return GetLastEnemy();
            case TowerTargetType.Closest:
                return GetClosestEnemy();
            default:
                return null;
        }
    }

    private Enemy GetFirstEnemy()
    {
        Enemy furthestEnemy = _listOfEnemies[0];

        for ( int i = 1; i < _listOfEnemies.Count; i++ )
        {
            furthestEnemy = CompareGreaterPathProgress(furthestEnemy, _listOfEnemies[i]);
        }

        return furthestEnemy;
    }

    private Enemy GetLastEnemy()
    {
        Enemy lastEnemy = _listOfEnemies[0];

        for ( int i = 1; i < _listOfEnemies.Count; i++ )
        {
            lastEnemy = CompareLeastPathProgress(lastEnemy, _listOfEnemies[i]);
        }

        return lastEnemy;
    }

    private Enemy GetClosestEnemy()
    {
        Enemy closestEnemy = null;
        float closestDistanceSqr = Mathf.Infinity;
        
        foreach ( Enemy potentialEnemy in _listOfEnemies )
        {
            // More efficient to calculate distance squared
            // Remove the sqr root calculations from Vector.Distance
            Vector3 directionToTarget = potentialEnemy.transform.position - transform.position;
            float distanceSqrToTarget = directionToTarget.sqrMagnitude;

            if ( distanceSqrToTarget < closestDistanceSqr )
            {
                closestDistanceSqr = distanceSqrToTarget;
                closestEnemy = potentialEnemy;
            }
        }

        return closestEnemy;
    }

    private Enemy CompareGreaterPathProgress(Enemy enemy1, Enemy enemy2)
    {
        if ( enemy1.currentWaypointIndex > enemy2.currentWaypointIndex )
        {
            return enemy1;
        }
        else if ( enemy1.currentWaypointIndex < enemy2.currentWaypointIndex )
        {
            return enemy2;
        }

        if ( enemy1.distanceToNextWaypoint > enemy2.distanceToNextWaypoint )
        {
            return enemy2;
        }
        else if ( enemy1.distanceToNextWaypoint < enemy2.distanceToNextWaypoint )
        {
            return enemy1;
        }
        else
        {
            return enemy1;
        }
    }

    private Enemy CompareLeastPathProgress(Enemy enemy1, Enemy enemy2)
    {
        if ( enemy1.currentWaypointIndex < enemy2.currentWaypointIndex )
        {
            return enemy1;
        }
        else if ( enemy1.currentWaypointIndex > enemy2.currentWaypointIndex )
        {
            return enemy2;
        }

        if ( enemy1.distanceToNextWaypoint < enemy2.distanceToNextWaypoint )
        {
            return enemy2;
        }
        else if ( enemy1.distanceToNextWaypoint > enemy2.distanceToNextWaypoint )
        {
            return enemy1;
        }
        else
        {
            return enemy1;
        }
    }

    /* ===== */

    private void OnTriggerEnter(Collider other) 
    {
        Enemy enteredEnemy = other.gameObject.GetComponent<Enemy>();

        _listOfEnemies.Add(enteredEnemy);

        TryAttack();
    }

    private void OnTriggerExit(Collider other) 
    {
        _listOfEnemies.Remove(other.gameObject.GetComponent<Enemy>());
    }

    private void OnEnemyDefeated(GameObject enemy)
    {
        Enemy defeatedEnemy = enemy.GetComponent<Enemy>();

        if ( _listOfEnemies.Contains(defeatedEnemy) )
        {
            _listOfEnemies.Remove(defeatedEnemy);
        }
    }
}
