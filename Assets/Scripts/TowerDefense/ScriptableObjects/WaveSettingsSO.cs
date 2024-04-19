using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveSettings", menuName = "TowerDefense/Wave Settings")]
public class WaveSettingsSO : ScriptableObject
{
    [Header("Basic Settings")]
    [SerializeField]
    private int _numberOfWaves = 5;

    public int NumberOfWaves => _numberOfWaves;
}