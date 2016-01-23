using System;
using Assets;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Math;
using UnityEngine.UI;

public class DoorsTimer : MonoBehaviour {

    private float _moveDuration;
    private float _stopDuration;

    private float _currentMoveDuration;
    private float _currentStopDuration;

    private bool _isDoorsOpen;
    private bool _isPaused;
    
    [SerializeField] private DoorsAnimationController _firstDoor;
    [SerializeField] private DoorsAnimationController _secondDoor;
    [SerializeField] private DoorsAnimationController _thirdDoor;
    [SerializeField] private DoorsAnimationController _forthDoor;
    [SerializeField] private GameObject _stickNote;
    [SerializeField] private List<GameObject> _tramMovableObjects;
    [SerializeField] private BenchCombinationManager _benchCombinationManager;

    private PassengerSM _currentStickPassenger;
    private DoorsAnimationController _currentStickDoor;

    private AudioPlayer _player;
    
    private const float StickSoundDelay = 1;
    private float _currentStickSoundRemainingTime;

    private bool _isLeftDoorsOpened;

    void Awake()
    {
        _moveDuration = ConfigReader.GetConfig().GetField("tram").GetField("MoveDuration").n;
        _stopDuration = ConfigReader.GetConfig().GetField("tram").GetField("StopDuration").n;
        _player = GameObject.Find("AudioPlayer").GetComponent<AudioPlayer>();
    }

    public int GetCurrentRemainingTime()
    {
        if (_isDoorsOpen)
            return (int) (_stopDuration - _currentStopDuration);
        return (int) (_moveDuration - _currentMoveDuration);
    }

    void Start()
    {
        _currentMoveDuration = 0;
        _currentStopDuration = 0;
        _isDoorsOpen = true;
        //
        //_isPaused = true;
        UpdateDoors();
    }

    void MoveObjects(bool isMoving)
    {
        foreach (var tramMovableObject in _tramMovableObjects)
        {
            Animator animator = tramMovableObject.GetComponent<Animator>();
            if (isMoving)
            {
                animator.Play("move");
            }
            else
            {
                animator.Stop();
            }
        }
    }

    void UpdateDoors()
    {        
        if (_isDoorsOpen)
        {
            int reward = _benchCombinationManager.GetCombinationReward();
            Debug.Log(string.Format("reward: {0}", reward));
            _player.SetDoorsOpen(true);
            MoveObjects(false);
            _isLeftDoorsOpened = Randomizer.GetPercentageBasedBoolean(50);
            if (_isLeftDoorsOpened)
            {
                _firstDoor.Open(true);
                _thirdDoor.Open(true);
            }
            else
            {
                _secondDoor.Open(true);
                _forthDoor.Open(true);
            }
            GameController.GetInstance().SetDoorsOpen(true);
        }
        else
        {
            _player.SetDoorsOpen(false);
            MoveObjects(true);
            if (_isLeftDoorsOpened)
            {
                _firstDoor.Close();
                _thirdDoor.Close();
            }
            else
            {
                _secondDoor.Close();
                _forthDoor.Close();
            }
            GameController.GetInstance().SetDoorsOpen(false);
        }
    }

    public void SetPaused(bool paused)
    {
        if(paused == _isPaused)
            return;
        if (paused)
        {
            _isPaused = true;
            _stickNote.SetActive(true);
            _currentStickSoundRemainingTime = StickSoundDelay;
        }
        else
        {
            if (!GameController.GetInstance().IsAnybodyStick())
            {
                _stickNote.SetActive(false);
                if (_currentStickDoor != null)
                {
                    if(_isDoorsOpen)
                        _currentStickDoor.Open(false);
                    else 
                        _currentStickDoor.Close();
                    _currentStickPassenger = null;
                }
                _isPaused = false;
            }
        }
    }

    public bool IsDoorsOpen
    {
        get { return _isDoorsOpen; }
    }

    public void StopNow()
    {
        if (_isDoorsOpen)
        {
            _isDoorsOpen = false;
            UpdateDoors();
        }
        int bonusCount = (int)(_moveDuration - _currentMoveDuration);
        _currentMoveDuration = _moveDuration;
    }

    void FixedUpdate () 
    {
        if (_isPaused)
        {
            _currentStickSoundRemainingTime -= Time.fixedDeltaTime;
            if (_currentStickSoundRemainingTime <= 0)
            {
                _currentStickSoundRemainingTime = StickSoundDelay;
                _player.PlayAudioById("tramdoorstuck");
            }
            if (_currentStickPassenger == null)
            {
                _currentStickPassenger = GameController.GetInstance().GetStickPassenger();
                if (_currentStickPassenger != null)
                {
                    _currentStickDoor = MonobehaviorHandler.GetMonobeharior().GetObject<Floor>("Floor").GetPassengerDoor(_currentStickPassenger)
                        .GetComponent<DoorsAnimationController>();
                }
            }
            if(_currentStickDoor != null)
                _currentStickDoor.Glitch();
            return;
        }
	    if (_isDoorsOpen)
	    {
	        _currentStopDuration += Time.fixedDeltaTime;
	        if (_currentStopDuration > _stopDuration)
	        {
	            _isDoorsOpen = false;
	            _currentStopDuration = 0;
                UpdateDoors();
	        }
	    }
	    else
	    {
	        _currentMoveDuration += Time.fixedDeltaTime;
	        if (_currentMoveDuration > _moveDuration)
	        {
	            _isDoorsOpen = true;
	            _currentMoveDuration = 0;
                GameController.GetInstance().CheckBeforeDoorsOpen();
                UpdateDoors();

	        }
	    }

	}
}
