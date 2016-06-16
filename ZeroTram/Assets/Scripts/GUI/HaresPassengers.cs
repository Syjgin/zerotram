using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HaresPassengers : MonoBehaviour, GameStateNotificationListener
{
	private bool _shouldUpdate;

    [SerializeField] private Image _indicator;
    [SerializeField] private Sprite _normalSprite;
    [SerializeField] private Sprite _redSprite;
    [SerializeField] private GameObject _arrow;

    private int _savedHaresPercent = -1;
    private float _targetRotation;
    private const float Epsilon = 0.1f;
    private const float RotationSpeed = 1;
    private bool _isAnimationFinished;

    void Awake()
	{
		_shouldUpdate = true;
		GameController.GetInstance().AddListener(this);
	}

	void OnDestroy()
	{
		GameController.GetInstance().RemoveListener(this);
	}

    void Update()
    {
        if(_isAnimationFinished)
            return;
        float currentRotation = _arrow.transform.localRotation.eulerAngles.z;
        if (currentRotation > 180)
        {
            currentRotation = currentRotation - 360;
        }
        if (Math.Abs(currentRotation - _targetRotation) > Epsilon)
        {
            
            bool increment = currentRotation < _targetRotation;
            _arrow.transform.localEulerAngles = new Vector3(0, 0, increment ? currentRotation + RotationSpeed : currentRotation - RotationSpeed);
            /*float degreesByModule = currentRotation - 360;
            if (Math.Abs(degreesByModule - _targetRotation) < Epsilon)
            {
                _isAnimationFinished = true;
            }*/
        }
        else
        {
            _isAnimationFinished = true;
        }
    }

	public void UpdateInfo(GameController.StateInformation information)
	{
	    if (_shouldUpdate)
	    {
	        if (_savedHaresPercent == -1)
	        {
	            _savedHaresPercent = information.Hares;
                _indicator.sprite = _savedHaresPercent > GameController.GetInstance().MaxHaresPercent ? _redSprite : _normalSprite;
	            _targetRotation = PercentToDegree(_savedHaresPercent);
	            _isAnimationFinished = false;
	        }
	        else
	        {
	            int newHaresPercent = information.Hares;
	            if (newHaresPercent != _savedHaresPercent)
	            {
	                _savedHaresPercent = newHaresPercent;
	                StartCoroutine(Blink());
                    _targetRotation = PercentToDegree(_savedHaresPercent);
                    _isAnimationFinished = false;
                }
            }
	    }
	}

    private float PercentToDegree(int percent)
    {
        return 90 - (percent*1.8f);
    }

    private IEnumerator Blink()
    {
        for (int i = 0; i < 5; i++)
        {
            _indicator.sprite = i%2 == 0 ? _normalSprite : _redSprite;
            yield return new WaitForSeconds(0.2f);
        }
        _indicator.sprite = _savedHaresPercent > GameController.GetInstance().MaxHaresPercent ? _redSprite : _normalSprite;
    }

	public void GameOver()
	{
		_shouldUpdate = false;
	}
}