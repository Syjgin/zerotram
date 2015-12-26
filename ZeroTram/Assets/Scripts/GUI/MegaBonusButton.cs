using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MegaBonusButton : MonoBehaviour
{
    [SerializeField] private Button _megaBonusButton;
    [SerializeField] private List<Sprite> _bonusSprites;
    [SerializeField] private Image _image;
    [SerializeField] private GameObject _megaIconPrefab;
    [SerializeField] private GameObject _background;

    private IBonus _megaBonus;
    
    public void SetMegaBonus(IBonus megaBonus)
    {
        _megaBonus = megaBonus;
        _image.sprite = _bonusSprites[(int) megaBonus.GetBonusType()];
    }

    public void OnMouseDown()
    {
        GameObject icon = Instantiate(_megaIconPrefab);
        MegaBonusIcon iconObject = icon.GetComponent<MegaBonusIcon>();
        iconObject.SetBonus(_megaBonus, _image.sprite);
        _background.gameObject.SetActive(false);
        _megaBonusButton.gameObject.SetActive(false);
    }

    public void SetVisible(bool isVisible)
    {
        if (isVisible)
        {
            _background.gameObject.SetActive(isVisible);
            _megaBonusButton.gameObject.SetActive(isVisible);
        }
    }
}