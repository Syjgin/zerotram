using System;
using Assets;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Level;
using UnityEngine.UI;

public class KilledPassengers : MonoBehaviour, GameStateNotificationListener
{
    private Text _text;
    private bool _shouldUpdate;

	void Start ()
	{
	    _text = GetComponent<Text>();
        _text.text = "-";
	    _shouldUpdate = true;
        GameController.GetInstance().AddListener(this);
	}

    void OnDestroy()
    {
        GameController.GetInstance().RemoveListener(this);
    }

    public void UpdateInfo(GameController.StateInformation information)
    {
        if(_shouldUpdate)
            _text.text = information.RemainKilled < 0 ? "-" : information.RemainKilled.ToString();
    }

    public void GameOver()
    {
        _shouldUpdate = false;
    }
}
