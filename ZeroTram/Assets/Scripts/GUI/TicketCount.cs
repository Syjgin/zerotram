using System;
using Assets;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Level;
using UnityEngine.UI;

public class TicketCount : MonoBehaviour, GameStateNotificationListener
{
    private Text _text;

	void Start ()
	{
	    _text = GetComponent<Text>();
        _text.text = "0";
        GameController.GetInstance().AddListener(this);
	}

    void OnDestroy()
    {
        GameController.GetInstance().RemoveListener(this);
    }

    public void UpdateInfo(GameController.StateInformation information)
    {
        _text.text = information.TicketCount.ToString();
    }

    public void GameOver()
    {
        
    }
}
