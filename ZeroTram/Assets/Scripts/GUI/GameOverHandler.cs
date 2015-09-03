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
    [SerializeField] private Text _countText;
    [SerializeField] private GameObject _stickCaption;

    private const String DeathReason = "Кондуктор погиб";
    private const String HareReason = "Слишком много зайцев";
    private const String KilledPassengersReason = "Слишком много погибших";
    private const int ZeroCount = 6;

    private GameController.StateInformation _stateInfo;

    void Awake()
    {
        _restartButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1;
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
        _stickCaption.SetActive(false);
        if (_stateInfo.Hares > GameController.GetInstance().MaxHaresPercent)
            _reasonText.text = HareReason;
        if (_stateInfo.Killed > GameController.GetInstance().MaxKilledPercent)
            _reasonText.text = KilledPassengersReason;
        if(_stateInfo.IsConductorDied)
            _reasonText.text = DeathReason;
        int leadingZeroCount = ZeroCount - _stateInfo.TicketCount.ToString().Length;
        String countText = String.Empty;
        for (int i = 0; i < leadingZeroCount; i++)
        {
            countText += "0";
        }
        countText += _stateInfo.TicketCount;
        countText = countText.Insert(3, " ");
        _countText.text = countText;
        gameOverMenu.SetActive(true);
        if(_stateInfo.TicketCount > 0)
            RecordsManager.GetInstance().AddRecord(_stateInfo.TicketCount);
    }
}
