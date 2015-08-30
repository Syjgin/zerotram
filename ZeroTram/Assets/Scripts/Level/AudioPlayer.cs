using System.Collections.Generic;
using Assets;
using Assets.Scripts.Level;
using Assets.Scripts.Math;
using UnityEngine;
using System.Collections;

public class AudioPlayer : MonoBehaviour
{

    [SerializeField] private List<AudioClip> _levelAudios;
    [SerializeField] private AudioClip _gameOverAudio;
    [SerializeField] private AudioSource _levelAudioSource;
    [SerializeField] private AudioSource _doorsOpenAudioSource;
    private bool _isDoorsOpen;
    private bool _isGameOverHandled;

    private const float PauseVolumeLevel = 0.2f;

    private void SelectRandomSong()
    {
        int index = Randomizer.GetInRange(0, _levelAudios.Count);
        _levelAudioSource.clip = _levelAudios[index];
        _levelAudioSource.Play();
    }

    public void SetDoorsOpen(bool open)
    {
        _isDoorsOpen = open;
        if (_isDoorsOpen)
        {
            _levelAudioSource.Pause();
            _doorsOpenAudioSource.Play();
        }
        else
        {
            _levelAudioSource.UnPause();
            _doorsOpenAudioSource.Pause();
        }
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

    public void GameOver()
    {
        _levelAudioSource.Stop();
        _doorsOpenAudioSource.Stop();
        _levelAudioSource.clip = _gameOverAudio;
        _levelAudioSource.Play();
    }

	void Update () {
	    if (!_isDoorsOpen)
	    {
            if (!_levelAudioSource.isPlaying)
            {
                SelectRandomSong();
            }   
	    }
	    if (GameController.GetInstance().IsGameFinished && !_isGameOverHandled)
	    {
	        _isGameOverHandled = true;
            GameOver();
	    }
	}
}
