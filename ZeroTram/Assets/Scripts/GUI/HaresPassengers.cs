using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Level;
using UnityEngine.UI;

namespace Assets
{
    public class HaresPassengers : MonoBehaviour, GameStateNotificationListener
{
    private Text _text;
    private const String Prefix = "зайцев: ";

    void Awake()
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
        _text.text = Prefix + information.Hares + "%";
    }

        public void GameOver()
    {

    }
}
}