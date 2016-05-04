using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class VideoScript : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Text _text;
    [SerializeField] private Text _cloudText;
    [SerializeField] private GameObject _all;
    [SerializeField] private GameObject _audio;
    [SerializeField] private Canvas _interface;

    public static bool _isTraining { get; set; }
    public static bool _isPause { get; set; }
    
    private int _currentNum = 1;
    private bool _sceneChanging;

    // Use this for initialization
    void Start ()
    {
        Time.timeScale = 0;
        _isPause = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isPause)
            if (Time.timeScale > 0.02f)
                Time.timeScale -= 0.01f;
            else
                Time.timeScale = 0;
    }

    public void SceneChange()
    {
        if (!_isTraining)
        {
            if (_currentNum < 4)
            {
                if (!_sceneChanging)
                {
                    _sceneChanging = true;
                    _text.gameObject.SetActive(true);
                    _text.text = StringResources.GetLocalizedString("Training" + _currentNum);
                    _currentNum++;
                }
                else
                {
                    _sceneChanging = false;
                    _text.gameObject.SetActive(false);
                    _image.sprite = Resources.Load<Sprite>(@"Sprites/training/" + _currentNum);
                }
            }
            else
            {
                _text.gameObject.SetActive(false);
                _image.gameObject.SetActive(false);
                _audio.gameObject.SetActive(true);
                _interface.gameObject.SetActive(true);
                _all.gameObject.SetActive(true);
                _isTraining = true;
                Time.timeScale = 1;
                _cloudText.text = StringResources.GetLocalizedString("Training" + _currentNum);
            }
        }
        else
        {
            if (_isPause)
            {
                _isPause = false;
            }
        }
    }
}
