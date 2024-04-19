using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField]
    private float _movementSpeed = 10f;

    [SerializeField]
    private float _minDistanceToChangeTarget = 0.2f;

    private Transform[] _listOfWaypoints = null;

    private int _currentWaypointIndex = 0;
    private Transform _currentTarget = null;

    [SerializeField]
    private WaypointsWrapper wp;

    private void Start() {
        _listOfWaypoints = wp.GetListOfWaypoints();

        _currentTarget = _listOfWaypoints[0];
    }

    private void Update() 
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector3 currentDirection = _currentTarget.position - transform.position;

        transform.Translate(currentDirection.normalized * _movementSpeed * Time.deltaTime, Space.World);
        transform.LookAt(_currentTarget);

        if ( Vector3.Distance(transform.position, _currentTarget.position) <= _minDistanceToChangeTarget )
        {
            GetNextWaypoint();
        }
    }

    private void GetNextWaypoint()
    {
        if ( _currentWaypointIndex >= _listOfWaypoints.Length - 1 )
        {
            AttackPlayer();
        }
        else
        {
            transform.position = _currentTarget.position;

            _currentTarget = _listOfWaypoints[++_currentWaypointIndex];
        }
    }

    private void AttackPlayer()
    {
        // Attack player hp

        Destroy(gameObject);
    }
}