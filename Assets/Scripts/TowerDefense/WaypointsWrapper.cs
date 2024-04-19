using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointsWrapper : MonoBehaviour
{
    private Transform[] _listOfWaypoints = null;

    private void Awake() 
    {
        _listOfWaypoints = new Transform[transform.childCount];

        for ( int i = 0; i < _listOfWaypoints.Length; i++ )
        {
            _listOfWaypoints[i] = transform.GetChild(i);
        }
    }

    public Transform[] GetListOfWaypoints()
    {
        return _listOfWaypoints;
    }
}