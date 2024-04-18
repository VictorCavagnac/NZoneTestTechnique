using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField]
    private TMP_Text _maxWave = null;

    [SerializeField]
    private TMP_Text _maxScore = null;

    [Header("Main Menu UI")]
    [SerializeField]
    private Button _playButton = null;

    [Header("Broadcasting to..")]
    [SerializeField]
    private LoadEventChannelSO _loadLevel = default;

    [SerializeField] 
    private GameSceneSO _levelToLoad = default;

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

    /* ===== */

    public void StartLevel()
    {
        _playButton.interactable = false;

        _loadLevel.RaiseEvent(_levelToLoad);
    }
}