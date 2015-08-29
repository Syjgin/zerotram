using System;
using Assets;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Level;
using UnityEngine.UI;

public class KilledPassengers : MonoBehaviour, GameStateNotificationListener
{
    private Text _text;
    private const String Prefix = "не доехавших: ";

	void Start ()
	{
	    _text = GetComponent<Text>();
	    _text.text = Prefix + "0%";
        GameController.GetInstance().AddListener(this);
	}

    void OnDestroy()
    {
        GameController.GetInstance().RemoveListener(this);
    }

    public void UpdateInfo(GameController.StateInformation information)
    {
        _text.text = Prefix + information.Killed + "%";
    }

    public void GameOver()
    {
        
    }
}
