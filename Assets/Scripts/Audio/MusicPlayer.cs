using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [Header("Music Settings")]
    [SerializeField]
    private AudioCueEventChannelSO _playMusic = default;

    [SerializeField] 
    private AudioCueSO _MusicAudio = default;

    [SerializeField] 
    private AudioConfigurationSO _audioConfig = default;

    [Header("Listening to..")]
    [SerializeField]
    private VoidEventChannelSO _onSceneReady = default;

    private void OnEnable() 
    {
        _onSceneReady.OnEventRaised += OnSceneReady;
    }

    private void OnDisable() 
    {
        _onSceneReady.OnEventRaised -= OnSceneReady;
    }

    private void OnSceneReady()
    {
        _playMusic.RaisePlayEvent(_MusicAudio, _audioConfig);
    }
}