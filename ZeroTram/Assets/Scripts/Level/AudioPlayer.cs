using System.Collections.Generic;
using Assets;
using Assets.Scripts.Level;
using Assets.Scripts.Math;
using UnityEngine;
using System.Collections;

public class AudioPlayer : MonoBehaviour, GameStateNotificationListener
{

    [SerializeField] private List<AudioClip> _levelAudios;
    [SerializeField] private AudioClip _gameOverAudio;
    [SerializeField] private AudioSource _levelAudioSource;
    [SerializeField] private AudioSource _doorsOpenAudioSource;
    private bool _isDoorsOpen;
    private bool _isPlayerJustPaused;

    private const float PauseVolumeLevel = 0.2f;
    private const float VolumeIncrementCount = 0.01f;
    private const float VolumeBarrier = 0.1f;

    void Start()
    {
        GameController.GetInstance().AddListener(this);
    }

    void Destroy()
    {
        GameController.GetInstance().RemoveListener(this);
    }

    private void SelectRandomSong()
    {
        int index = Randomizer.GetInRange(0, _levelAudios.Count);
        _levelAudioSource.clip = _levelAudios[index];
        _levelAudioSource.Play();
    }

    public void SetDoorsOpen(bool open)
    {
        _isDoorsOpen = open;
    }

    public void HandlePauseMenu(bool pause)
    {
        float currentLevel = pause ? PauseVolumeLevel : 1;
        if (_isDoorsOpen)
        {
            _doorsOpenAudioSource.volume = currentLevel;
        }
        else
        {
            _levelAudioSource.volume = currentLevel;
        }
    }

    public void UpdateInfo(GameController.StateInformation information)
    {
        
    }

    public void GameOver()
    {
        if (_levelAudioSource != null && _doorsOpenAudioSource != null)
        {
            _levelAudioSource.Stop();
            _doorsOpenAudioSource.Stop();
            _levelAudioSource.clip = _gameOverAudio;
            _levelAudioSource.volume = 1;
            _levelAudioSource.Play();   
        }
    }

	void Update () {
        if (_isDoorsOpen)
        {
            _levelAudioSource.volume -= VolumeIncrementCount;
            if (_levelAudioSource.volume < VolumeBarrier)
            {
                _levelAudioSource.Pause();
                _isPlayerJustPaused = true;
            }
            if (!_doorsOpenAudioSource.isPlaying)
            {
                _doorsOpenAudioSource.UnPause();
                _doorsOpenAudioSource.volume = VolumeBarrier;
            }
            if(_doorsOpenAudioSource.volume < 1)
                _doorsOpenAudioSource.volume += VolumeIncrementCount;
        }
        else
        {
            if (!_levelAudioSource.isPlaying && !_isPlayerJustPaused)
            {
                SelectRandomSong();
            }
            _doorsOpenAudioSource.volume -= VolumeIncrementCount;
            if (_doorsOpenAudioSource.volume < VolumeBarrier)
                _doorsOpenAudioSource.Pause();
            if (!_levelAudioSource.isPlaying)
            {
                _levelAudioSource.UnPause();
                _levelAudioSource.volume = VolumeBarrier;
                _isPlayerJustPaused = false;
            }
            if (_levelAudioSource.volume < 1)
                _levelAudioSource.volume += VolumeIncrementCount;
        }
	}
}
