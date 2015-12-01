using Assets;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DoorsTimer : MonoBehaviour {

    private float _moveDuration;
    private float _stopDuration;

    private float _currentMoveDuration;
    private float _currentStopDuration;

    private bool _isDoorsOpen;
    private bool _isPaused;

    [SerializeField] private Animator[] _wheels;
    [SerializeField] private DoorsAnimationController[] _doorsAnimators;
    [SerializeField] private GameObject _stickNote;
    [SerializeField] private Parallax _parallax;

    private PassengerSM _currentStickPassenger;
    private DoorsAnimationController _currentStickDoor;

    private AudioPlayer _player;
    
    private const float StickSoundDelay = 1;
    private float _currentStickSoundRemainingTime;

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

    void UpdateDoors()
    {
        foreach (var animator in _wheels)
        {
            if(_isDoorsOpen)
                animator.Play("idle");
            else 
                animator.Play("wheel");
        }
        
        if (_isDoorsOpen)
        {
            _player.SetDoorsOpen(true);
            _parallax.SetEnabled(false);
            foreach (var doorsAnimationController in _doorsAnimators)
            {
                doorsAnimationController.Open(true);
            }
            GameController.GetInstance().SetDoorsOpen(true);
        }
        else
        {
            _player.SetDoorsOpen(false);
            _parallax.SetEnabled(true);
            foreach (var doorsAnimationController in _doorsAnimators)
            {
                doorsAnimationController.Close();
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
        if (!IsDoorsOpen)
        {
            int bonusCount = (int)(_moveDuration - _currentMoveDuration);
            _currentMoveDuration = _moveDuration;
        }
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
