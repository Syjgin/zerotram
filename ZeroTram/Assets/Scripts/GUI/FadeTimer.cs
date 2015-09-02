using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FadeTimer : MonoBehaviour
{

    [SerializeField] private Animator _fadeAnimator;
    [SerializeField] private Image _logo;
    [SerializeField] private Text _presents;
    [SerializeField] private Text _mainText;
    [SerializeField] private GameObject _zerotramLogo;
    [SerializeField] private AudioSource _source;
    [SerializeField] private Button _clicker;

    private Dictionary<int, String>  phrasesByFrames = new Dictionary<int, string>();

    private int _currentFrame;
    private const int ChangeFramePeriod = 7;
    private const int FirstChangeFramePeriod = 2;
    private float _timeSincePreviousFrame;
	// Use this for initialization
	void Start ()
	{
        _clicker.onClick.AddListener(() => Application.LoadLevelAsync("MainMenu"));
	    _timeSincePreviousFrame = 0;
	    _currentFrame = 0;
        phrasesByFrames.Add(2, "Есть правило: \n самые затаенные желания всегда исполняются неожиданно.");
        phrasesByFrames.Add(3, "Другое правило: \n все путешествия между мирами начинаются с \"Нулевого трамвая\".");
        phrasesByFrames.Add(4, "Твое путешествие начнется сейчас.");
        phrasesByFrames.Add(5, "И тебе не придется платить за проезд. \n Потому что ты - Кондуктор!");
	}
	
	// Update is called once per frame
	void Update ()
	{
        if(_fadeAnimator.GetAnimatorTransitionInfo(0).IsName("fadeAnimation"))
            return;
	    _timeSincePreviousFrame += Time.deltaTime;
	    int currentWaitPeriod = _currentFrame == 0 ? FirstChangeFramePeriod : ChangeFramePeriod;
	    if (_timeSincePreviousFrame > currentWaitPeriod)
	    {
	        _currentFrame++;
	        _timeSincePreviousFrame = 0;
            _fadeAnimator.Play("fadeAnimation");
            return;
	    }
        if(_currentFrame == 0)
            return;
	    if (_currentFrame == 1)
	    {
	        _logo.gameObject.SetActive(false);
	        _presents.gameObject.SetActive(false);
	        _mainText.gameObject.SetActive(true);
	    }
	    else
	    {
	        if (_currentFrame > 5)
	            Application.LoadLevelAsync("MainMenu");
	        else
	        {
	            if (_currentFrame == 3)
	            {
                    _source.Play();
	            }
	            if (_currentFrame == 4)
	            {
	                _zerotramLogo.SetActive(true);
	            }
                _mainText.text = phrasesByFrames[_currentFrame];
	        }
	    }
	}
}
