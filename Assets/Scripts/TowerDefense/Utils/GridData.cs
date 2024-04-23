using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridData
{
    public Dictionary<Vector3, PlacementData> placedTowers = new();

    public void AddTower(Vector3 gridPosition, GameObject tower)
    {
        PlacementData data = new PlacementData(gridPosition, tower);

        if ( placedTowers.ContainsKey(gridPosition) )
        {
            Debug.Log("GridData already contains this cell position..");
        }
        else
        {
            placedTowers[gridPosition] = data;
        }
    }

    public bool CanPlaceTowerAt(Vector3 gridPosition)
    {
        return !placedTowers.ContainsKey(gridPosition);
    }
}

public class PlacementData
{
    // Can be improved into a list if we need bigger towers (2x2 towers and more)
    public Vector3 occupiedPosition;
    public GameObject tower;

    public PlacementData(Vector3 occupiedPosition, GameObject tower)
    {
        this.occupiedPosition = occupiedPosition;
        this.tower = tower;
    }
}