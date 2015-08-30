using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PauseHandler : MonoBehaviour
{

    [SerializeField] private Button _pauseButton;
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _toMenuButton;
    [SerializeField] private GameObject _pauseMenu;

    private AudioPlayer _player;

    void Awake()
    {
        _player = GameObject.Find("AudioPlayer").GetComponent<AudioPlayer>();
    }

	void Start () {
	    _pauseButton.onClick.AddListener(() =>
	    {
	        if (!_pauseMenu.activeSelf)
	        {
                _player.HandlePauseMenu(true);
	            _pauseMenu.SetActive(true);
	            _pauseButton.enabled = false;
	            Time.timeScale = 0;
	        }
	    });
        _resumeButton.onClick.AddListener(() =>
        {
            _player.HandlePauseMenu(false);
            _pauseMenu.SetActive(false);
            _pauseButton.enabled = true;
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
