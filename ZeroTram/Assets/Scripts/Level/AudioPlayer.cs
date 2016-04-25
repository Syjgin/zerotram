using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class AudioPlayer : MonoBehaviour, GameStateNotificationListener
{

    [SerializeField] private List<AudioClip> _levelAudios;
    [SerializeField] private AudioClip _gameOverAudio;
    [SerializeField] private AudioSource _levelAudioSource;
    [SerializeField] private AudioSource _doorsOpenAudioSource;
    [SerializeField] private List<AudioClip> _sounds;
    [SerializeField] private AudioSource _soundsSource;
    private bool _isDoorsOpen;
    private bool _isPlayerJustPaused;

    private const float PauseVolumeLevel = 0.2f;
    private const float VolumeIncrementCount = 0.01f;
    private const float VolumeBarrier = 0.1f;
    private const float NormalSoundVolume = 0.5f;

    void Start()
    {
        GameController.GetInstance().AddListener(this);
        _levelAudioSource.volume = NormalSoundVolume;
        _doorsOpenAudioSource.volume = NormalSoundVolume;
    }

    void Destroy()
    {
        GameController.GetInstance().RemoveListener(this);
    }

    public void PlayAudioById(string id)
    {
        foreach (var audioClip in _sounds)
        {
            if (audioClip.name == id)
            {
                _soundsSource.PlayOneShot(audioClip);
                return;
            }
        }
    }

    private void SelectRandomSong()
    {
        int index = Randomizer.GetInRange(0, _levelAudios.Count);
        _levelAudioSource.clip = _levelAudios[index];
        _levelAudioSource.Play();
    }

    public void SetDoorsOpen(bool open)
    {
        PlayAudioById("tramdoor");
        _isDoorsOpen = open;
    }

    public void HandlePauseMenu(bool pause)
    {
        float currentLevel = pause ? PauseVolumeLevel : NormalSoundVolume;
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
	    if (Time.timeScale == 0)
	    {
	        return;
	    }
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
            if(_doorsOpenAudioSource.volume < NormalSoundVolume)
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
            if (_levelAudioSource.volume < NormalSoundVolume)
                _levelAudioSource.volume += VolumeIncrementCount;
        }
	}
}
