using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ExitButton : MonoBehaviour
{
    [SerializeField] private Button _button;
	// Use this for initialization
	void Start () {
		_button.onClick.AddListener(() => SceneManager.LoadScene("MainMenu"));	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
