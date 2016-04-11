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
    private GameObject _stopcrane_unpressed;
    private GameObject _stopcrane_pressed;

    void Awake()
    {
        _player = GameObject.Find("AudioPlayer").GetComponent<AudioPlayer>();
        _stopcrane_unpressed = GameObject.Find("Stopcrane_unpressed");
        _stopcrane_pressed = GameObject.Find("Stopcrane_pressed");
        _stopcrane_pressed.SetActive(false);
    }

	void Start () {
	    _pauseButton.onClick.AddListener(() =>
	    {
	        if (!_pauseMenu.activeSelf)
	        {
                _player.HandlePauseMenu(true);
	            _pauseMenu.SetActive(true);
	            _pauseButton.enabled = false;
                _stopcrane_unpressed.SetActive(false);
                _stopcrane_pressed.SetActive(true);
                Time.timeScale = 0;
	        }
	    });
        _resumeButton.onClick.AddListener(() =>
        {
            _player.HandlePauseMenu(false);
            _pauseMenu.SetActive(false);
            _pauseButton.enabled = true;
            _stopcrane_unpressed.SetActive(true);
            _stopcrane_pressed.SetActive(false);
            Time.timeScale = 1;
        });
        _toMenuButton.onClick.AddListener(() =>
        {
            Application.LoadLevelAsync("MainMenu");
            _toMenuButton.enabled = false;
            Time.timeScale = 1;
        });
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
