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

    [SerializeField] private DoorsAnimationController[] _doorsAnimators;
    [SerializeField] private GameObject _stickNote;

    void Awake()
    {
        _moveDuration = ConfigReader.GetConfig().GetField("tram").GetField("MoveDuration").n;
        _stopDuration = ConfigReader.GetConfig().GetField("tram").GetField("StopDuration").n;
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
        foreach (var doorsAnimationController in _doorsAnimators)
        {
            if(_isDoorsOpen)
                doorsAnimationController.Open();
            else
                doorsAnimationController.Close();
        }
    }

    public void SetPaused(bool paused)
    {
        if (paused)
        {
            _isPaused = true;
            _stickNote.SetActive(true);
        }
        else
        {
            if (!GameController.GetInstance().IsAnybodyStick())
            {
                _stickNote.SetActive(false);
                _isPaused = false;
            }
        }
    }

    public bool IsDoorsOpen
    {
        get { return _isDoorsOpen; }
    }

    void FixedUpdate () 
    {
        if(_isPaused)
            return;
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
