using System;
using Assets;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Level;
using UnityEngine.UI;

public class StationCount : MonoBehaviour, GameStateNotificationListener
{
    [SerializeField] private TextMesh _text;
    private const string Prefix = "станция: ";

	void Start ()
	{
	    _text.text = Prefix + "1";
        GameController.GetInstance().AddListener(this);
	}

    void OnDestroy()
    {
        GameController.GetInstance().RemoveListener(this);
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
