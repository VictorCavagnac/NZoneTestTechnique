using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIMainMenu : MonoBehaviour
{
    [Header("Player Score")]
    [SerializeField]
    private TMP_Text _maxWave = null;

    [SerializeField]
    private TMP_Text _maxScore = null;

    private void OnEnable() 
    {
        SetStats();
    }

    private async void SetStats()
    {
        int maxWave  = await SaveManager.Instance.GetPlayerStat("maxWave");
        int maxScore = await SaveManager.Instance.GetPlayerStat("maxScore");

        _maxWave.text  = maxWave.ToString();
        _maxScore.text = maxScore.ToString();
    }
}