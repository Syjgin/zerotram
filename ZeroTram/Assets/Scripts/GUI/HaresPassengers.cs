using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HaresPassengers : MonoBehaviour, GameStateNotificationListener
{
	private Text _text;
	private bool _shouldUpdate;

	void Awake()
	{
		_text = GetComponent<Text>();
		_text.text = "0%" + "/" + GameController.GetInstance().MaxHaresPercent;
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
			_text.text = information.Hares + "/" + GameController.GetInstance().MaxHaresPercent + "%";
	}

	public void GameOver()
	{
		_shouldUpdate = false;
	}
}