using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PauseHandler : MonoBehaviour
{

    [SerializeField] private Button _pauseButton;
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _toMenuButton;
    [SerializeField] private GameObject _pauseMenu;

	void Start () {
	    _pauseButton.onClick.AddListener(() =>
	    {
	        if (!_pauseMenu.activeSelf)
	        {
	            _pauseMenu.SetActive(true);
	            Time.timeScale = 0;
	        }
	    });
        _resumeButton.onClick.AddListener(() =>
        {
            _pauseMenu.SetActive(false);
            Time.timeScale = 1;
        });
        _toMenuButton.onClick.AddListener(() =>
        {
            Application.LoadLevelAsync("MainMenu");
            Time.timeScale = 1;
        });
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
