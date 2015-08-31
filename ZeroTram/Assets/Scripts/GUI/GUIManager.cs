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
		Application.LoadLevel ("Main"); 
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
		Application.LoadLevel ("Authors");
	}

	public void Records(){
		Application.LoadLevel ("Records");
	}

	public void BacktoMenu(){
		Application.LoadLevel ("MainMenu");
    }

	public void Retry(){
		isPaused = false;
			PauseMenu.SetActive (false);
			MainUI.SetActive (true);

	}
}
}