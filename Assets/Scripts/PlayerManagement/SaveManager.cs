using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.CloudSave;
using UnityEngine;
using UnityEngine.iOS;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    private void Awake() 
    {
        if ( Instance == null )
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public async Task<int> GetPlayerStat(string statName)
    {
        int statValue = 0;

        var playerStat = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string>{statName});

        if ( playerStat.TryGetValue(statName, out var keyName) )
        {
            statValue = keyName.Value.GetAs<int>();
        }

        return statValue;
    }

    /* ===== */

    public void SavePlayerStats(int waveReached, int score)
    {
        SaveIndividualPlayerStat(waveReached, "maxWave");
        SaveIndividualPlayerStat(score, "maxScore");
    }

    private async void SaveIndividualPlayerStat(int scoreToSave, string statToSave)
    {
        int maxScore = await GetPlayerStat(statToSave);

        if ( scoreToSave > maxScore )
        {
            var dataToSave = new Dictionary<string, object>{{statToSave, scoreToSave}};
            
            var result = await CloudSaveService.Instance.Data.Player.SaveAsync(dataToSave);
        }
    }
}