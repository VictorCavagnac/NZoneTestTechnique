using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Common Settings")]
    [SerializeField]
    private WaveManager _waveManager = default;

    [SerializeField]
    private GameSettingsSO _gameSettings = null;

    [SerializeField]
    private float _waitTimeBetweenWaves = 2f;

    [SerializeField]
    private float _fadeDuration = 0.3f;

    [Header("Listening to..")]
    [SerializeField] 
    private VoidEventChannelSO _onSceneReady = default;

    [SerializeField]
    private IntEventChannelSO _onEnemyDefeated = default;

    [SerializeField]
    private VoidEventChannelSO _onEnemyAttackPlayer = default;

    [SerializeField]
    private VoidEventChannelSO _onRequestNextWave = default;

    [SerializeField]
    private VoidEventChannelSO _onRequestRestartLevel = default;

    [Header("Broadcasting to..")]
    [SerializeField]
    private VoidEventChannelSO _activateGameplayInputEvent = default;

    [SerializeField]
    private VoidEventChannelSO _disableAllInputsEvent = default;

    [SerializeField]
    private IntEventChannelSO _updatePlayerHealthEvent = default;

    [SerializeField]
    private IntEventChannelSO _updatePlayerMoneyEvent = default;

    [SerializeField]
    private IntEventChannelSO _updatePlayerWaveEvent = default;

    [SerializeField]
    private BoolEventChannelSO _endEvent = default;

    [SerializeField]
    private VoidEventChannelSO _resetLevelEvent = default;

    [SerializeField] 
    private BoolEventChannelSO _toggleLoadingScreen = default;

	[SerializeField]
	private FadeChannelSO _fadeRequestChannel = default;

    [SerializeField]
    private AudioCueEventChannelSO _sfxEvent = default;

    [Header("Audio")]
    [SerializeField]
    private AudioConfigurationSO _sfxConfig = null;

    [SerializeField]
    private AudioCueSO _enemyDefeatSFX = null;

    private Transform _enemySpawnPoint = null;
    private Transform[] _listOfWaypoints = null;

    private int _currentWaveIndex = 0;
    private int _currentHealth = 0;
    [HideInInspector]
    public int currentMoney = 0;

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

    private void OnEnable() 
    {
        _onSceneReady.OnEventRaised += StartGame;

        _onEnemyDefeated.OnEventRaised += OnEnemyDefated;
        _onEnemyAttackPlayer.OnEventRaised += OnEnemyAttackPlayer;

        _onRequestNextWave.OnEventRaised += OnRequestNextWave;

        _onRequestRestartLevel.OnEventRaised += OnRequestRestartLevel;
    }

    private void OnDisable() 
    {
        _onSceneReady.OnEventRaised -= StartGame;

        _onEnemyDefeated.OnEventRaised -= OnEnemyDefated;
        _onEnemyAttackPlayer.OnEventRaised -= OnEnemyAttackPlayer;

        _onRequestNextWave.OnEventRaised -= OnRequestNextWave;

        _onRequestRestartLevel.OnEventRaised -= OnRequestRestartLevel;

        StopAllCoroutines();
    }

    /* ===== */

    public void InitiateGame(Transform enemySpawnPoint, Transform[] listOfWaypoints)
    {
        _enemySpawnPoint = enemySpawnPoint;
        _listOfWaypoints = listOfWaypoints;

        _waveManager.SetSpawnPoint(_enemySpawnPoint, _listOfWaypoints);
    }

    private void StartGame()
    {
        _activateGameplayInputEvent.RaiseEvent();

        _currentWaveIndex = 0;
        _currentHealth = _gameSettings.StartingHealth;
        currentMoney = _gameSettings.StartingMoney;

        StartCoroutine(WaitTimeBetweenWaves());
    }

    private void BeginNextWave()
    {
        WaveSettingsSO currentWave = _gameSettings.ListOfWaves[_currentWaveIndex];

        _waveManager.StartNextWave(currentWave);
        _updatePlayerWaveEvent.RaiseEvent(_currentWaveIndex + 1);
    }

    private void OnRequestNextWave()
    {
        _currentWaveIndex += 1;

        if ( _currentWaveIndex < _gameSettings.ListOfWaves.Count )
        {  
            StartCoroutine(WaitTimeBetweenWaves());
        }
        else
        {
            SaveManager.Instance.SavePlayerStats(_currentWaveIndex, 999);

            _endEvent.RaiseEvent(true);
            _disableAllInputsEvent.RaiseEvent();
        }
    }

    private IEnumerator WaitTimeBetweenWaves()
    {
        yield return new WaitForSeconds(_waitTimeBetweenWaves);

        BeginNextWave();
    }

    private void OnRequestRestartLevel()
    {
        StartCoroutine(ResetLevel());
    }

    private IEnumerator ResetLevel()
    {
        _fadeRequestChannel.FadeOut(_fadeDuration);

        yield return new WaitForSeconds(_fadeDuration);

        _toggleLoadingScreen.RaiseEvent(true);

        _resetLevelEvent.RaiseEvent();

        yield return new WaitForSeconds(2);

        _toggleLoadingScreen.RaiseEvent(false);
        _fadeRequestChannel.FadeIn(_fadeDuration);

        StartGame();
    }

    /* ===== */

    public void BoughtTower(int cost)
    {
        currentMoney -= cost;

        _updatePlayerMoneyEvent.RaiseEvent(currentMoney);
    }

    /* ===== */

    private void OnEnemyDefated(int money)
    {
        currentMoney += money;

        _updatePlayerMoneyEvent.RaiseEvent(currentMoney);
        _sfxEvent.RaisePlayEvent(_enemyDefeatSFX, _sfxConfig);
    }

    private void OnEnemyAttackPlayer()
    {
        _currentHealth -= 1;

        _updatePlayerHealthEvent.RaiseEvent(_currentHealth);

        if ( _currentHealth <= 0 )
        {
            SaveManager.Instance.SavePlayerStats(_currentWaveIndex, 0);

            _endEvent.RaiseEvent(false);
            _disableAllInputsEvent.RaiseEvent();
        }
    }
}