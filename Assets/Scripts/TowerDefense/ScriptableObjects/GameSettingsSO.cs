using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "GameSettings", menuName = "TowerDefense/Game Settings")]
public class GameSettingsSO : ScriptableObject
{
    [Header("Basic Settings")]
    [SerializeField] 
    private int _startingHealth = 15;

    [SerializeField]
    private int _startingMoney = 100;

    [SerializeField]
    private List<TowerSettingsSO> _towersAvailable = null;

    [Header("Wave Settings")]
    [SerializeField]
    private List<WaveSettingsSO> _listOfWaves = null;

    public int StartingHealth => _startingHealth;
    public int StartingMoney => _startingMoney;
    public List<TowerSettingsSO> TowersAvailable => _towersAvailable;

    public List<WaveSettingsSO> ListOfWaves => _listOfWaves;
}