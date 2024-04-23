using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicProjectile : Projectile
{
    private Enemy _target;
    private TowerController _currentTower;

    private bool _isUsed = false;

    private int _attackDmg = 0;

    private void OnEnable() 
    {
        _target = null;
        _isUsed = false;
    }

    public void Initiate(Enemy enemyToFollow, TowerController currentTower, int attackDmg)
    {
        _target = enemyToFollow;
        _currentTower = currentTower;
        _attackDmg = attackDmg;

        _isUsed = true;
    }

    private void Update() 
    {
        if ( _target != null )
        {
            transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, 15f * Time.deltaTime);
        }

        CheckProjectileIsStillUseful();
    }

    private void CheckProjectileIsStillUseful()
    {
        // Don't forget to check if the projectile is still up but the enemy is already dead
        if ( _isUsed && _target == null || _isUsed && Vector3.Distance(_target.transform.position, transform.position) < 0.1f )
        {
            DisposeProjectile();
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        DisposeProjectile();
    }

    private void DisposeProjectile()
    {
        if ( _target != null )
        {
            _target.HitEnemy(this, _attackDmg);
        }

        _target = null;
        _isUsed = false;

        _currentTower.ProjectileHit(this);
    }
}
