using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Assets
{

public class GUIManager : MonoBehaviour {
	public GameObject PauseMenu;
	public GameObject MainUI;
	public bool isPaused = false;

    [SerializeField] private InputField _userName;
    [SerializeField] private GameObject _loadingText;
    [SerializeField] private Button _loadingButton;
    [SerializeField] private AudioSource _startClip;
    [SerializeField] private Button _tut;
    void Start()
    {
        //PlayerPrefs.DeleteAll();
        if (RecordsManager.GetInstance().IsUsernameWasSet())
        {
            _userName.text = RecordsManager.GetInstance().GetCurrentUserName();
        }
        _tut.onClick.AddListener(() => Application.LoadLevelAsync("tutorial"));
    }

	public void StartGame () {
        RecordsManager.GetInstance().SetCurrentUserName(_userName.text);
        _loadingText.SetActive(true);
	    _loadingButton.enabled = false;
        _startClip.Play();
    	Application.LoadLevelAsync("Main"); 
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
		Application.LoadLevelAsync("Authors");
	}

	public void Records(){
		Application.LoadLevelAsync("Records");
	}

	public void BacktoMenu(){
		Application.LoadLevelAsync("MainMenu");
    }

	public void Retry(){
		isPaused = false;
			PauseMenu.SetActive (false);
			MainUI.SetActive (true);

	}
}
}