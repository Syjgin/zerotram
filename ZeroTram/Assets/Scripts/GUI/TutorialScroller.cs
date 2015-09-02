using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialScroller : MonoBehaviour
{
    [SerializeField] private Button _leftButton;
    [SerializeField] private Button _rightButton;
    [SerializeField] private List<Sprite> _tutorialImages;
    [SerializeField] private Image _view;
    [SerializeField] private Animator _viewAnimator;
    [SerializeField] private Button _exitButton;
    [SerializeField] private GameObject _loadingText;

    private int _currentImage;
    private const int ImagesCount = 5;
    
	// Use this for initialization
	void Start ()
	{
	    _currentImage = 0;
        _leftButton.onClick.AddListener(() =>
        {
            if (!_viewAnimator.GetCurrentAnimatorStateInfo(0).IsName("idle"))
                return;
            if (_currentImage > 0)
            {
                _viewAnimator.Play("scrollToLeft");
                _currentImage--;
                _view.sprite = _tutorialImages[_currentImage];
                if (_currentImage < ImagesCount - 1)
                {
                    _exitButton.gameObject.SetActive(false);
                }
            }
        });
        _rightButton.onClick.AddListener(() =>
        {
            if (!_viewAnimator.GetCurrentAnimatorStateInfo(0).IsName("idle"))
                return;
            if (_currentImage < ImagesCount - 1)
            {
                _viewAnimator.Play("scrollToRight");
                _currentImage++;
                _view.sprite = _tutorialImages[_currentImage];
                if (_currentImage == ImagesCount - 1)
                {
                    _exitButton.gameObject.SetActive(true);
                }
            }
        });
        _exitButton.onClick.AddListener(() =>
        {
            PlayerPrefs.SetInt("WasTutorialFinished", 1);
            _loadingText.SetActive(true);
            _exitButton.enabled = false;
            Application.LoadLevelAsync("main");
        });
	}
	
}
