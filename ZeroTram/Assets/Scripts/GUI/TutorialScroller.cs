using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialScroller : MonoBehaviour
{
    [SerializeField] private Button _exitButton;
    [SerializeField] private GameObject _loadingText;

	// Use this for initialization
	void Start ()
	{
        _exitButton.onClick.AddListener(() =>
        {
            PlayerPrefs.SetInt("WasTutorialFinished", 1);
            _loadingText.SetActive(true);
            _exitButton.enabled = false;
            Application.LoadLevelAsync("main");
        });
	}
	
}
