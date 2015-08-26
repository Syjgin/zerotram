using Assets;
using UnityEngine;
using System.Collections;

public class DoorsTimer : MonoBehaviour {

    private const int MoveDuration = 60;
    private const int StopDuration = 10;

    private float _currentMoveDuration;
    private float _currentStopDuration;

    private bool _isDoorsOpen;

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

	void FixedUpdate () 
    {
	    if (_isDoorsOpen)
	    {
	        _currentStopDuration += Time.fixedDeltaTime;
	        if (_currentStopDuration > StopDuration)
	        {
	            _isDoorsOpen = false;
	            _currentMoveDuration = 0;
                GameController.GetInstance().CheckStats();
                UpdateDoors();
	        }
	    }
	    else
	    {
	        _currentMoveDuration += Time.fixedDeltaTime;
	        if (_currentMoveDuration > MoveDuration)
	        {
	            _isDoorsOpen = true;
	            _currentStopDuration = 0;
                UpdateDoors();
	        }
	    }

	}
}
