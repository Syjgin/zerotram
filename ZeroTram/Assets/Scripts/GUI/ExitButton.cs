using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour
{
    [SerializeField] private Button _button;
	// Use this for initialization
	void Start () {
        _button.onClick.AddListener(() => Application.LoadLevel("MainMenu"));	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
