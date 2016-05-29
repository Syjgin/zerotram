using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GUIManager : MonoBehaviour {
	public GameObject PauseMenu;
	public GameObject MainUI;
	public bool isPaused = false;

	[SerializeField] private InputField _userName;
	[SerializeField] private GameObject _loadingText;
	[SerializeField] private Button _loadingButton;
	[SerializeField] private AudioSource _startClip;
	[SerializeField] private Button _tut;
	[SerializeField] private Button _map;
	void Start()
	{
		//PlayerPrefs.DeleteAll();
		if (RecordsManager.GetInstance().IsUsernameWasSet())
		{
			_userName.text = RecordsManager.GetInstance().GetCurrentUserName();
		}
		_tut.onClick.AddListener(() => SceneManager.LoadSceneAsync("tutorial"));
		_map.onClick.AddListener(() => SceneManager.LoadSceneAsync("Map"));
	}

	public void StartGame () {
		RecordsManager.GetInstance().SetCurrentUserName(_userName.text);
		_loadingText.SetActive(true);
		_loadingButton.enabled = false;
		_startClip.Play();
		SceneManager.LoadSceneAsync("Main"); 
	}

	public void Pause () {
		isPaused = true;
		PauseMenu.SetActive (true);
		MainUI.SetActive (false);
	}

	public void Exit(){
		Application.Quit ();
	}

	public void Authors(){
        SceneManager.LoadSceneAsync("Authors");
	}

	public void Records(){
        SceneManager.LoadSceneAsync("Records");
	}

	public void BacktoMenu(){
        SceneManager.LoadSceneAsync("MainMenu");
	}

	public void Retry(){
		isPaused = false;
		PauseMenu.SetActive (false);
		MainUI.SetActive (true);

	}
}