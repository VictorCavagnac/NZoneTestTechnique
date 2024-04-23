using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITowerDefense : MonoBehaviour
{
    [Header("Life UI Settings")]  
    [SerializeField]
    private TMP_Text _life = null;

    [SerializeField]
    private float _lifeShakeDuration = 0.5f;

    [SerializeField]
    private float _lifeShakeStrength = 10f;

    [SerializeField]
    private int _lifeShakeVibrato = 30;

    [Header("Money UI Settings")]
    [SerializeField]
    private TMP_Text _money = null;

    [SerializeField]
    private float _moneyUpdateDuration = 0.5f;

    [Header("Wave UI Settings")]
    [SerializeField]
    private TMP_Text _wave = null;

    [SerializeField]
    private TMP_Text _waveTitle = null;

    [SerializeField]
    private RectTransform _waveTitleRect = null;

    [SerializeField]
    private CanvasGroup _waveCommonTitleCG = null;

    [SerializeField]
    private CanvasGroup _waveTitleCG = null;

    [SerializeField]
    private float _waveTitleFadeDuration = 1f;
    
    [SerializeField]
    private float _waveTitleHeight = 150;

    [SerializeField]
    private float _waveTitleWaitTime = 1f;

    [Header("Building UI Settings")]
    [SerializeField]
    private RectTransform _towerBuildingPanel = null;

    [SerializeField]
    private UITowerButton _towerButtonPrefab = null;
    
    [Header("EndLevel UI Settings")]
    [SerializeField]
    private UIEndLevel _endLevelUI = null;

    [SerializeField]
    private GameObject _endPanel = null;
    
    [SerializeField]
    private CanvasGroup _endPanelCG = null;

    [SerializeField]
    private float _endPanelFadeDuration = 2f;

    [Header("Pause UI Settings")]
    [SerializeField]
    private UIPauseMenu _pauseUI = null;

    [SerializeField]
    private CanvasGroup _pauseCG = null;

    [Header("Common Settings")]
    [SerializeField]
    private GameSettingsSO _gameSettings = default;

    [Header("Listening to..")]
    [SerializeField]
    private VoidEventChannelSO _onTowerReset = default;

    [SerializeField]
    private IntEventChannelSO _onUpdatePlayerHealth = default;

    [SerializeField]
    private IntEventChannelSO _onUpdatePlayerMoney = default;

    [SerializeField]
    private IntEventChannelSO _onUpdatePlayerWave = default;

    [SerializeField]
    private BoolEventChannelSO _onEndLevel = default;

    [SerializeField]
    private VoidEventChannelSO _onRestartLevel = default;

    [SerializeField]
    private VoidEventChannelSO _onPause = default;

    [Header("Broadcasting to..")]
    [SerializeField]
    private IntEventChannelSO _onTowerRequested = default;

    private bool _isTowerSelected = false;
    private int _currentTowerSelected = -1;

    private void OnEnable()
    {
        _onTowerReset.OnEventRaised += OnTowerReset;

        _onUpdatePlayerHealth.OnEventRaised += SetPlayerLife;
        _onUpdatePlayerMoney.OnEventRaised += SetPlayerMoney;
        _onUpdatePlayerWave.OnEventRaised += SetPlayerWave;

        _onEndLevel.OnEventRaised += OnEndLevel;
        _onRestartLevel.OnEventRaised += OnRestartLevel;

        _onPause.OnEventRaised += OnPause;
    }

    private void OnDisable()
    {
        _onTowerReset.OnEventRaised -= OnTowerReset;

        _onUpdatePlayerHealth.OnEventRaised -= SetPlayerLife;
        _onUpdatePlayerMoney.OnEventRaised -= SetPlayerMoney;
        _onUpdatePlayerWave.OnEventRaised -= SetPlayerWave;

        _onEndLevel.OnEventRaised -= OnEndLevel;
        _onRestartLevel.OnEventRaised -= OnRestartLevel;

        _onPause.OnEventRaised -= OnPause;
    }

    private void Awake() 
    {
        InitiateBuildingButtons();

        ResetUI();
    }

    private void ResetUI()
    {
        _endPanel.SetActive(false);
        _endPanelCG.alpha = 0;

        _pauseUI.gameObject.SetActive(false);
        _pauseCG.alpha = 0;

        OnTowerReset();
        SetPlayerLife(_gameSettings.StartingHealth);
        SetPlayerMoney(_gameSettings.StartingMoney);

        // Simply reset the wave text, don't launch the animation
        // The animation will be launch at the start of the game
        _wave.text = "1";
    }

    private void OnRestartLevel()
    {
        ResetUI();
    }
    
    /* ===== */

    private void SetPlayerLife(int currentLife)
    {
        _life.gameObject.transform.DOShakePosition(_lifeShakeDuration, _lifeShakeStrength, _lifeShakeVibrato);

        _life.text = currentLife.ToString();
    }

    /* ===== */

    private void SetPlayerMoney(int currentMoney)
    {
        int money = int.Parse(_money.text);

        DOTween.To(() => money, x => _money.text = x.ToString(), currentMoney, _moneyUpdateDuration);
    }

    /* ===== */

    private void SetPlayerWave(int currentWave)
    {
        _waveTitle.text = currentWave.ToString();

        StartCoroutine(LaunchWaveAnim(currentWave));
    }

    private IEnumerator LaunchWaveAnim(int currentWave)
    {   
        _waveTitleCG.DOFade(1, _waveTitleFadeDuration);
        _waveCommonTitleCG.DOFade(0, _waveTitleFadeDuration - 0.4f).OnComplete(() => _wave.text = currentWave.ToString());

        _waveTitleRect.anchoredPosition = new Vector3(0, _waveTitleHeight, 0);
        _waveTitleRect.DOAnchorPosY(0, _waveTitleFadeDuration).SetEase(Ease.OutSine);

        yield return new WaitForSeconds(_waveTitleWaitTime);

        _waveTitleCG.DOFade(0, _waveTitleFadeDuration);
        _waveCommonTitleCG.DOFade(1, _waveTitleFadeDuration - 0.4f);

        _waveTitleRect.DOAnchorPosY(-_waveTitleHeight, _waveTitleFadeDuration).SetEase(Ease.InSine); 
    }

    /* ===== */

    private void InitiateBuildingButtons()
    {
        for ( int i = 0; i < _gameSettings.TowersAvailable.Count; i++ )
        {
            int towerIndex = i;
            TowerSettingsSO towerAvailable = _gameSettings.TowersAvailable[towerIndex];

            UITowerButton towerButton = Instantiate(_towerButtonPrefab, _towerBuildingPanel);
            towerButton.Initiate(towerAvailable.Cost, towerAvailable.TowerSprite);

            towerButton.button.onClick.AddListener(delegate{ReadyTowerBuildingButton(towerIndex);});
        }
    }

    private void ReadyTowerBuildingButton(int towerIndex)
    {
        if ( !_isTowerSelected )
        {
            _onTowerRequested.RaiseEvent(towerIndex);

            _currentTowerSelected = towerIndex;
            _isTowerSelected = true;
        }
        else if ( _isTowerSelected && _currentTowerSelected != towerIndex )
        {   
            _onTowerRequested.RaiseEvent(towerIndex);

            _currentTowerSelected = towerIndex;
            _isTowerSelected = true;
        }
        else
        {
            _onTowerRequested.RaiseEvent(-1);

            _currentTowerSelected = -1;
            _isTowerSelected = false;
        } 
    }

    private void OnTowerReset()
    {
        _currentTowerSelected = -1;
        _isTowerSelected = false;
    }

    /* ===== */

    private void OnEndLevel(bool state)
    {
        _endLevelUI.SetEndPanel(state);

        _endPanelCG.alpha = 0;
        _endPanel.SetActive(true);

        _endPanelCG.DOFade(1, _endPanelFadeDuration);
    }

    /* ===== */

    private void OnPause()
    {
        _pauseCG.alpha = 0;
        _pauseCG.DOFade(1, 0.3f).SetUpdate(true);

        _pauseUI.gameObject.SetActive(true);
    }
}