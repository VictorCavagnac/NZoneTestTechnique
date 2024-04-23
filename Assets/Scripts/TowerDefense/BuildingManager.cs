using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    [Header("Building Settings")]
    [SerializeField]
    private Grid _grid = null;

    [Header("Common Settings")]
    [SerializeField]
    private GameSettingsSO _gameSettings = null;

    [Header("Lisetening to..")]
    [SerializeField]
    private IntEventChannelSO _onTowerRequested = default;

    [SerializeField]
    private Vector3EventChannelSO _onCursorWorldPos = default;

    [SerializeField]
    private VoidEventChannelSO _onSelect = default;

    [Header("Broadcasting to..")]
    [SerializeField]
    private VoidEventChannelSO _towerPlacedEvent = default;

    private GridData _towerGridData;

    private bool _isPlacingTower = false;
    private GameObject _towerInPlacement = null;
    private TowerSettingsSO _towerInPlacementSettings = null;

    private Vector3 _gridPosition = Vector3.zero;

    private Vector3 _cursorWorldPos = Vector3.zero;

    public static BuildingManager Instance;

    private void Awake() {
        Instance = this;
    }

    private void OnEnable() 
    {
        _onTowerRequested.OnEventRaised += SetTowerPreview;
        _onCursorWorldPos.OnEventRaised += SetCursorWorldPosition;

        _towerGridData = new GridData();
    }

    private void OnDisable() 
    {
        _onTowerRequested.OnEventRaised -= SetTowerPreview;
        _onCursorWorldPos.OnEventRaised -= SetCursorWorldPosition;
        _onSelect.OnEventRaised -= TryToBuyTower;
    }

    private void Update()
    {
        if ( _isPlacingTower )
        {
            HandleTowerPreviewMovement();
        }
    }

    /* ===== */

    private void SetTowerPreview(int towerIndex)
    {
        // Destroy the previous preview if needed
        if ( _towerInPlacement != null )
        {
            Destroy(_towerInPlacement);
            
            _towerInPlacementSettings = null;
            _towerInPlacement = null;
        }

        // Tower selection is reset
        if ( towerIndex == -1 )
        {
            _onSelect.OnEventRaised -= TryToBuyTower;

            _towerInPlacementSettings = null;
            _towerInPlacement = null;
            _isPlacingTower = false;
        }
        else
        {
            _towerInPlacementSettings = _gameSettings.TowersAvailable[towerIndex];

            _onSelect.OnEventRaised += TryToBuyTower;

            _towerInPlacement = Instantiate(_towerInPlacementSettings.TowerPrefab);
            _isPlacingTower = true;
        }
    }

    private void SetCursorWorldPosition(Vector3 cursorWorldPos)
    {
        _cursorWorldPos = cursorWorldPos;
    }

    private void HandleTowerPreviewMovement()
    {
        _gridPosition = SnapCoordinateToGrid(_cursorWorldPos);
        _gridPosition.y = 1;

        _towerInPlacement.transform.position = _gridPosition;
    }

    private Vector3 SnapCoordinateToGrid(Vector3 position)
    {
        Vector3Int cellPos = _grid.WorldToCell(position);

        return _grid.GetCellCenterWorld(cellPos);
    }

    private void TryToBuyTower()
    {
        // Check if the player can afford to buy the tower
        if ( _towerInPlacement != null && GameManager.Instance.currentMoney >= _towerInPlacementSettings.Cost )
        {
            bool placementValid = _towerGridData.CanPlaceTowerAt(_gridPosition);

            if ( placementValid )
            {
                GameManager.Instance.BoughtTower(_towerInPlacementSettings.Cost);
            
                _towerPlacedEvent.RaiseEvent();

                _towerGridData.AddTower(_gridPosition);

                _towerInPlacement = null;
                _isPlacingTower = false;
            }
        }
    }
}