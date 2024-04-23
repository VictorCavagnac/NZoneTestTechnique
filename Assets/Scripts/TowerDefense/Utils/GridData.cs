using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridData
{
    Dictionary<Vector3, PlacementData> placedTowers = new();

    public void AddTower(Vector3 gridPosition)
    {
        PlacementData data = new PlacementData(gridPosition);

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

    public PlacementData(Vector3 occupiedPosition)
    {
        this.occupiedPosition = occupiedPosition;
    }
}