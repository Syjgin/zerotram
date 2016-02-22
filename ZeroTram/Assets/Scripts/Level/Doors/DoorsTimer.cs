using System;
using Assets;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Math;
using UnityEngine.UI;

public class DoorsTimer : MonoBehaviour
{
    public enum DoorsOpenMode
    {
        single,
        tween,
        all
    };

    private float _moveDuration;
    private float _stopDuration;

    private float _currentMoveDuration;
    private float _currentStopDuration;

    private bool _isDoorsOpen;
    private bool _isPaused;
    
    [SerializeField] private DoorsAnimationController[] _doors;
    [SerializeField] private GameObject _stickNote;
    [SerializeField] private List<GameObject> _tramMovableObjects;
    [SerializeField] private BenchCombinationManager _benchCombinationManager;

    private PassengerSM _currentStickPassenger;
    private DoorsAnimationController _currentStickDoor;

    private AudioPlayer _player;
    
    private const float StickSoundDelay = 1;
    private const int DoorsCount = 4;
    private float _currentStickSoundRemainingTime;

    private bool[] _doorOpened;

    void Awake()
    {
        _moveDuration = ConfigReader.GetConfig().GetField("tram").GetField("MoveDuration").n;
        _stopDuration = ConfigReader.GetConfig().GetField("tram").GetField("StopDuration").n;
        _player = GameObject.Find("AudioPlayer").GetComponent<AudioPlayer>();
        _doorOpened = new bool[DoorsCount];
    }

    public int GetCurrentRemainingTime()
    {
        if (_isDoorsOpen)
            return (int) (_stopDuration - _currentStopDuration);
        return (int) (_moveDuration * GameController.GetInstance().GetCurrentSpawnCount() - _currentMoveDuration);
    }

    void Start()
    {
        _currentMoveDuration = 0;
        _currentStopDuration = 0;
        _isDoorsOpen = true;
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
                animator.Play("stop");
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
            switch (MapManager.GetInstance().GetCurrentStationInfo().DoorsOpenMode)
            {
                case DoorsOpenMode.single:
                    CalculateOpenProbabilities(false);
                    for (int i = 0; i < DoorsCount; i++)
                    {
                        if (_doorOpened[i])
                        {
                            _doors[i].Open(true);
                        }    
                    }
                    break;
                case DoorsOpenMode.tween:
                    CalculateOpenProbabilities(true);
                    for (int i = 0; i < DoorsCount; i++)
                    {
                        if (_doorOpened[i])
                        {
                            _doors[i].Open(true);
                        }
                    }
                    break;
                case DoorsOpenMode.all:
                    for (int i = 0; i < DoorsCount; i++)
                    {
                        _doors[i].Open(true);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            GameController.GetInstance().SetDoorsOpen(true);
        }
        else
        {
            _player.SetDoorsOpen(false);
            MoveObjects(true);
            foreach (var doorsAnimationController in _doors)
            {
                if(doorsAnimationController.IsOpened())
                    doorsAnimationController.Close();
            }
            GameController.GetInstance().SetDoorsOpen(false);
        }
    }

    private void CalculateOpenProbabilities(bool tween)
    {
        bool isAnyDoorOpened = false;
        for (int i = 0; i < DoorsCount; i++)
        {
            _doorOpened[i] = false;
        }
        if (tween)
        {
            bool isLeft = Randomizer.GetPercentageBasedBoolean(50);
            for (int i = 0; i < DoorsCount; i++)
            {
                bool odd = ((i + 1) % 2 == 1);
                if (odd == isLeft)
                {
                    _doorOpened[i] = true;
                }
            }
        }
        else
        {
            for (int i = 0; i < DoorsCount; i++)
            {
                _doorOpened[i] = Randomizer.GetPercentageBasedBoolean(100 / DoorsCount);
                if (_doorOpened[i])
                    isAnyDoorOpened = true;
            }
            if (!isAnyDoorOpened)
            {
                int index = Randomizer.GetInRange(0, DoorsCount);
                _doorOpened[index] = true;
            }
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

    public bool IsDoorOpenedByNumber(int number)
    {
        return _doors[number].IsOpened();
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
