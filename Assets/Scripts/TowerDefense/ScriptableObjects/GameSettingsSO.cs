using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "TowerDefense/Game Settings")]
public class GameSettingsSO : ScriptableObject
{
    [Header("Basic Settings")]
    [SerializeField] 
    private int _startingHealth = 15;

    [SerializeField]
    private int _startingMoney = 100;

    [Header("Wave Settings")]
    [SerializeField]
    private WaveSettingsSO _waveSettings = null;

    public int StartingHealth => _startingHealth;
    public int StartingMoney  => _startingMoney;

    public WaveSettingsSO WaveSettings => _waveSettings;
}