using System;
using Assets;
using Assets.Scripts.Level;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameOverHandler : MonoBehaviour, GameStateNotificationListener
{
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private Text _reasonText;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _exitButton;
    private const String DeathReason = "Кондуктор погиб";
    private const String HareReason = "Слишком много зайцев";
    private const String KilledPassengersReason = "Слишком много погибших пассажиров с билетами";

    private GameController.StateInformation _stateInfo;

    void Awake()
    {
        _restartButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1;
            GameController.GetInstance().Init();
            Application.LoadLevel("main");
        });
        _exitButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1;
            Application.LoadLevel("MainMenu");
        });
        GameController.GetInstance().AddListener(this);
    }

    void OnDestroy()
    {
        GameController.GetInstance().RemoveListener(this);
    }

    public void UpdateInfo(GameController.StateInformation information)
    {
        _stateInfo = information;
    }

    public void GameOver()
    {
        if (_stateInfo.Hares > GameController.GetInstance().MaxHaresPercent)
            _reasonText.text = HareReason;
        if (_stateInfo.Killed > GameController.GetInstance().MaxKilledPercent)
            _reasonText.text = KilledPassengersReason;
        if(_stateInfo.IsConductorDied)
            _reasonText.text = DeathReason;
        gameOverMenu.SetActive(true);
    }
}
