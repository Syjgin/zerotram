using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CurrentBonusTimer : MonoBehaviour
{
    [SerializeField] private List<Sprite> _bonusSprites;
    [SerializeField] private Text _textField;
    [SerializeField] private Image _currentBonusImage;
    [SerializeField] private CanvasGroup _group;

    private IBonus _bonus;

    public void Activate(IBonus bonus)
    {
        _bonus = bonus;
        GameController.BonusTypes bonusType = bonus.GetBonusType();
        _currentBonusImage.sprite = _bonusSprites[(int) bonusType];
        _textField.text = _bonus.GetTTL().ToString();
        _group.alpha = 1;
    }

	void FixedUpdate () {
	    if(_bonus == null)
            return;
        _textField.text = _bonus.GetTTL().ToString();
	    if (_bonus.GetTTL() <= 0)
	    {
            _bonus = null;
	        _group.alpha = 0;
	    }
	}
}
