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

    void Start()
    {
        //PlayerPrefs.DeleteAll();
        if (RecordsManager.GetInstance().IsUsernameWasSet())
        {
            _userName.text = RecordsManager.GetInstance().GetCurrentUserName();
        }
    }

	public void StartGame () {
        RecordsManager.GetInstance().SetCurrentUserName(_userName.text);
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