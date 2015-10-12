using UnityEngine;
using System.Collections;
using Assets;
using UnityEngine.UI;

public class StickNote : MonoBehaviour
{
    [SerializeField] private Text _text;

    private float _currentStickPeriod;

    void OnEnable()
    {
        _currentStickPeriod = GameController.GetInstance().GetStickPeriod();
        UpdateText();
    }

    void UpdateText()
    {
        _text.text = ((int)_currentStickPeriod).ToString();
    }

    void FixedUpdate()
    {
        if (_currentStickPeriod > 0)
            _currentStickPeriod -= Time.fixedDeltaTime;
        else
        {
            _currentStickPeriod = 0;
        }
        UpdateText();
        if (_currentStickPeriod == 0)
        {
            GameController.GetInstance().KillStickPassenger();
            DoorsHandler.GetTimer().SetPaused(false);
        }
    }
}
