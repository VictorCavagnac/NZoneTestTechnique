using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerSettings", menuName = "TowerDefense/Tower Settings")]
public class TowerSettingsSO : ScriptableObject
{
    [SerializeField]
    private string _towerName = "";

    [SerializeField]
    private TowerTargetType _towerType = default;

    [SerializeField]
    private int _cost = 100;

    [SerializeField]
    private float _attackCooldown = 1f;

    [SerializeField]
    private float _attackRange = 4f;

    [SerializeField]
    private Sprite _towerSprite = null;

    [SerializeField]
    private GameObject _towerPrefab = null;

    public string TowerName => _towerName;
    public TowerTargetType TowerType => _towerType;
    public int Cost => _cost;
    public float AttackCooldown => _attackCooldown;
    public float AttackRange => _attackRange;
    public Sprite TowerSprite => _towerSprite;
    public GameObject TowerPrefab => _towerPrefab;
}