using System;
using Assets;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Level;
using UnityEngine.UI;

public class StationCount : MonoBehaviour, GameStateNotificationListener
{
    private Text _text;
    private const String Prefix = "№ станции: ";

	void Start ()
	{
	    _text = GetComponent<Text>();
	    _text.text = Prefix + "1";
        GameController.GetInstance().AddListener(this);
	}

    public void UpdateInfo(GameController.StateInformation information)
    {
        int realNum = information.StationNumber + 1;
        _text.text = Prefix + realNum;
    }

    public void GameOver()
    {
        
    }
}
