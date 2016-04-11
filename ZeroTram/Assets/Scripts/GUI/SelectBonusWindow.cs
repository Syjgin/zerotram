using System;
using UnityEngine;
using System.Collections.Generic;
using Assets;
using UnityEngine.UI;

public class SelectBonusWindow : MonoBehaviour
{

    [SerializeField] private GameObject _bonusPanelPrefab;
    [SerializeField] private List<Sprite> _bonusSprites;
    [SerializeField] private RectTransform _scrollContent;
    [SerializeField] private MegaBonusButton _megaBonusButton;
    [SerializeField] private BonusTimer _bonusTimer;

    private const int ItemHeight = 165;
	// Use this for initialization
	void Start ()
	{
	    Time.timeScale = 0;
	    int index = 0;
        _megaBonusButton.SetVisible(false);
	    foreach (GameController.BonusTypes currentType in Enum.GetValues(typeof(GameController.BonusTypes)))
	    {
	        Sprite currentSprite = GetSpriteByBonus(currentType);
	        if (currentSprite != null)
	        {
	            GameObject instantiated = Instantiate(_bonusPanelPrefab);
	            RectTransform instRect = instantiated.GetComponent<RectTransform>();
                instRect.SetParent(_scrollContent, false);
                instRect.transform.localPosition = new Vector3(0, -index*ItemHeight);
	            Image image = instantiated.GetComponentInChildren<Image>();
	            image.sprite = currentSprite;
                string description = GetBonusDescription(currentType);
	            Text textField = instantiated.GetComponentInChildren<Text>();
	            textField.text = description;
	            Button but = instantiated.GetComponent<Button>();
	            var copyType = currentType;
	            but.onClick.AddListener(() =>
                {
                    _bonusTimer.SetMegaBonus(copyType);
                    _megaBonusButton.SetVisible(true);
                    Time.timeScale = 1;
                    gameObject.SetActive(false);
                });
                index++;
	        }
	    }
        //ConfigReader.LoadConfigFromServer();
        _scrollContent.sizeDelta = new Vector2(_scrollContent.sizeDelta.x, ItemHeight*index);
	}

    private string GetBonusDescription(GameController.BonusTypes bonusType)
    {
        return ConfigReader.GetConfig().GetField("descriptions").GetField(bonusType.ToString()).str;
    }

    private Sprite GetSpriteByBonus(GameController.BonusTypes bonusType)
    {
        switch (bonusType)
        {
            case GameController.BonusTypes.Wheel:
                return _bonusSprites[0];
            case GameController.BonusTypes.Ticket:
                return _bonusSprites[1];
            case GameController.BonusTypes.Boot:
                return _bonusSprites[2];
            case GameController.BonusTypes.Magnet:
                return _bonusSprites[3];
            case GameController.BonusTypes.Smile:
                return _bonusSprites[4];
            case GameController.BonusTypes.AntiHare:
                return _bonusSprites[5];
            case GameController.BonusTypes.SandGlass:
                return _bonusSprites[6];
            case GameController.BonusTypes.Vortex:
                return _bonusSprites[7];
            case GameController.BonusTypes.Snow:
                return _bonusSprites[8];
            case GameController.BonusTypes.Wrench:
                return null;
            case GameController.BonusTypes.Cogwheel:
                return null;
            case GameController.BonusTypes.Heal:
                return _bonusSprites[9];
            case GameController.BonusTypes.Clew:
                return _bonusSprites[10];
            default:
                throw new ArgumentOutOfRangeException("bonusType");
        }
    }
}
