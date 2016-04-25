using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StationCount : MonoBehaviour, GameStateNotificationListener
{
    [SerializeField] private Text _text;

	void Start ()
	{
	    _text.text = "1";
        GameController.GetInstance().AddListener(this);
	}

    void OnDestroy()
    {
        GameController.GetInstance().RemoveListener(this);
    }

    public void UpdateInfo(GameController.StateInformation information)
    {
        int realNum = information.StationNumber + 1;
        _text.text = realNum.ToString();
    }

    public void GameOver()
    {
        
    }
}
