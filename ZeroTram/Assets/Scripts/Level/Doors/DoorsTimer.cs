using Assets;
using UnityEngine;
using System.Collections;

public class DoorsTimer : MonoBehaviour {

    private const int MoveDuration = 10;
    private const int StopDuration = 5;

    private float _currentMoveDuration;
    private float _currentStopDuration;

    private bool _isDoorsOpen;
    private bool _isPaused;

    [SerializeField] private DoorsAnimationController[] _doorsAnimators;

    public int GetCurrentRemainingTime()
    {
        if (_isDoorsOpen)
            return (int) (StopDuration - _currentStopDuration);
        return (int) (MoveDuration - _currentMoveDuration);
    }

    void Start()
    {
        _currentMoveDuration = 0;
        _currentStopDuration = 0;
        _isDoorsOpen = true;
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
        }
        else
        {
            if (!GameController.GetInstance().IsAnybodyStick())
                _isPaused = false;
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
	        if (_currentStopDuration > StopDuration)
	        {
	            _isDoorsOpen = false;
	            _currentStopDuration = 0;
                UpdateDoors();
	        }
	    }
	    else
	    {
	        _currentMoveDuration += Time.fixedDeltaTime;
	        if (_currentMoveDuration > MoveDuration)
	        {
	            _isDoorsOpen = true;
	            _currentMoveDuration = 0;
                GameController.GetInstance().CheckBeforeDoorsOpen();
                UpdateDoors();
	        }
	    }

	}
}
