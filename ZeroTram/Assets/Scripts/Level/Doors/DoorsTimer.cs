using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Assets.Scripts.Client;
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
    private float _currentStationTotalMoveDuration;
    private float _stopDuration;

    private float _currentMoveDuration;
    private float _currentStopDuration;

    private bool _isDoorsOpen;
    private bool _isPaused;
    
    [SerializeField] private DoorsAnimationController[] _doors;
    [SerializeField] private Spawner _spawner;
    [SerializeField] private List<GameObject> _tramMovableObjects;
    [SerializeField] private BenchCombinationManager _benchCombinationManager;
    [SerializeField] private TrainingHandler _trainingHandler;
    [SerializeField] private Client _webClient;
    [SerializeField] private MyMessageScript _messageScript;

    private PassengerSM _currentStickPassenger;
    private DoorsAnimationController _currentStickDoor;

    private AudioPlayer _player;
    
    private const float StickSoundDelay = 1;
    private const int DoorsCount = 4;
    private float _currentStickSoundRemainingTime;

    private bool[] _doorOpened;

    private bool _isTrainingMode;
    private bool _isMovementTimeLocked;
    private int _countToFinish = -1;
    private bool _isSpawnEnabled;

    void Awake()
    {
        _isSpawnEnabled = true;
        _moveDuration = ConfigReader.GetConfig().GetField("tram").GetField("MoveDuration").n;
        _stopDuration = ConfigReader.GetConfig().GetField("tram").GetField("StopDuration").n;
        _player = GameObject.Find("AudioPlayer").GetComponent<AudioPlayer>();
        _doorOpened = new bool[DoorsCount];
    }

    public void DisableSpawn()
    {
        _isSpawnEnabled = false;
    }

    private void UpdateMoveDuration()
    {
        if(!_isTrainingMode)
            _currentStationTotalMoveDuration = _moveDuration*GameController.GetInstance().GetPassengersCount();
    }

    public void OpenDoors()
    {
        _isDoorsOpen = true;
        UpdateDoors();
    }

    public void SetMoveAndStopDuration(float moveDuration, float stopDuration)
    {
        _currentMoveDuration = 0;
        _currentStationTotalMoveDuration = moveDuration;
        _stopDuration = stopDuration;
        _isTrainingMode = true;
    }

    public void DisableTrainingMode()
    {
        _isTrainingMode = false;
    }

    public int GetCurrentRemainingTime()
    {
        if (_isDoorsOpen)
            return (int) (_stopDuration - _currentStopDuration);
        return (int) (_currentStationTotalMoveDuration - _currentMoveDuration);
    }

    void Start()
    {
        _currentMoveDuration = 0;
        _currentStopDuration = 0;
        _isDoorsOpen = true;
        if(TrainingHandler.IsTrainingFinished())
            UpdateDoors();
    }

    public void SetMovementLocked(bool locked)
    {
        _isMovementTimeLocked = locked;
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

    public int GetOpenedDoorsCount()
    {
        switch (MapManager.GetInstance().GetCurrentStationInfo().DoorsOpenMode)
        {
            case DoorsOpenMode.single:
                return 1;
            case DoorsOpenMode.tween:
                return DoorsCount/2;
            case DoorsOpenMode.all:
                return DoorsCount;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void SetStationCountListener(int count)
    {
        _countToFinish = count;
    }

    void UpdateDoors()
    {        
        if (_isDoorsOpen)
        {
            _player.SetDoorsOpen(true);
            MoveObjects(false);
            _spawner.PrepareToSpawn();
            if (!_isTrainingMode)
            {
                _benchCombinationManager.CalculateCurrent();
                switch (MapManager.GetInstance().GetCurrentStationInfo().DoorsOpenMode)
                {
                    case DoorsOpenMode.single:
                        CalculateOpenProbabilities(false);
                        for (int i = 0; i < DoorsCount; i++)
                        {
                            if (_doorOpened[i])
                            {
                                _doors[i].Open(_isSpawnEnabled);
                            }
                        }
                        break;
                    case DoorsOpenMode.tween:
                        CalculateOpenProbabilities(true);
                        for (int i = 0; i < DoorsCount; i++)
                        {
                            if (_doorOpened[i])
                            {
                                _doors[i].Open(_isSpawnEnabled);
                            }
                        }
                        break;
                    case DoorsOpenMode.all:
                        for (int i = 0; i < DoorsCount; i++)
                        {
                            _doors[i].Open(_isSpawnEnabled);
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            GameController.GetInstance().SetDoorsOpen(true);
        }
        else
        {
            _player.SetDoorsOpen(false);
            MoveObjects(true);
            UpdateMoveDuration();
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

    public void Unstick()
    {
        if (_currentStickDoor != null)
        {
            if (_isDoorsOpen)
                _currentStickDoor.Open(false);
            else
                _currentStickDoor.Close();
            _currentStickPassenger = null;
        }
        _isPaused = false;
    }

    public void SetPaused(bool paused)
    {
        if(paused == _isPaused)
            return;
        if (paused)
        {
            _isPaused = true;
            _currentStickSoundRemainingTime = StickSoundDelay;
        }
        else
        {
            Unstick();
        }
    }

    public bool IsDoorOpenedByNumber(int number)
    {
        return _doors[number].IsOpened();
    }

    public void StopNow()
    {
        int bonusCount = (int)(_currentStationTotalMoveDuration - _currentMoveDuration);
        if (bonusCount > 0)
        {
            _webClient.SendDoorBonusTime(bonusCount, o =>
            {
                if(!o.HasField("error"))
                {
                    string rewardCaption = String.Format(StringResources.GetLocalizedString("newDoorsTimeRecord"), bonusCount);
                    MessageSender.SendRewardMessage(o, _messageScript, rewardCaption);
                }
            });
        }
        if (_isDoorsOpen)
        {
            _isDoorsOpen = false;
            UpdateDoors();
        }
        _currentMoveDuration = _currentStationTotalMoveDuration;
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
            if(!_isMovementTimeLocked)
                _currentMoveDuration += Time.fixedDeltaTime;
            if (_currentMoveDuration > _currentStationTotalMoveDuration)
            {
                _isDoorsOpen = true;
                _currentMoveDuration = 0;
                GameController.GetInstance().CheckBeforeDoorsOpen();
                UpdateDoors();
                GameController.GetInstance().IncrementStationNumberForPassengers();
                if (_countToFinish > 0)
                {
                    _countToFinish--;
                    if (_countToFinish == 0)
                    {
                        MonobehaviorHandler.GetMonobeharior()
                        .GetObject<TrainingHandler>("TrainingHandler").ShowNext();
                    }
                }
            }
        }

	}
}
