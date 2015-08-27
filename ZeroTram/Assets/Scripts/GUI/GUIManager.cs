using UnityEngine;
using System.Collections;

namespace Assets
{

public class GUIManager : MonoBehaviour {
	public GameObject PauseMenu;
	public GameObject MainUI;
	public bool isPaused = false;
	
	public void StartGame () {
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