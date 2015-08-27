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

    void Start()
    {
        _text = GetComponent<Text>();
        _text.text = Prefix + "0%";
        GameController.GetInstance().AddListener(this);
    }

    public void UpdatePercentage(int killedPercent, int haresPercent)
    {
        _text.text = Prefix + haresPercent + "%";
    }

    public void GameOver()
    {

    }
}
}