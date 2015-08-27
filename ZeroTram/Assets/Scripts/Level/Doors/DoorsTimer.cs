using Assets;
using UnityEngine;
using System.Collections;

public class DoorsTimer : MonoBehaviour {

    private const int MoveDuration = 20;
    private const int StopDuration = 5;

    private float _currentMoveDuration;
    private float _currentStopDuration;

    private bool _isDoorsOpen;
    private bool _isPaused;

    [SerializeField] private DoorsAnimationController[] _doorsAnimators;

    public int GetCurrentRemainingTime()
    {
        if (_isDoorsOpen)
            return (int) _currentStopDuration;
        else
            return (int) _currentMoveDuration;
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
        _isPaused = paused;
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
