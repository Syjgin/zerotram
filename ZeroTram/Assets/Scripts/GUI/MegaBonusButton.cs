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
	[SerializeField] private Client _client;

    private IBonus _megaBonus;
    
    public void SetMegaBonus(IBonus megaBonus)
    {
        _megaBonus = megaBonus;
        _image.sprite = _bonusSprites[(int) megaBonus.GetBonusType()];
    }

    public void OnMouseDown()
    {
		_client.UseBonus (_megaBonus.GetBonusType ().ToString (), (JSONObject result) => {
			Debug.Log (result.ToString ());
			//TODO: uncomment when shop will be implemented
			/*if(!result.HasField ("error")) {
				GameObject icon = Instantiate(_megaIconPrefab);
				MegaBonusIcon iconObject = icon.GetComponent<MegaBonusIcon>();
				iconObject.SetBonus(_megaBonus, _image.sprite);
				_megaBonusButton.gameObject.SetActive(false);		
			}*/
		});
		GameObject icon = Instantiate(_megaIconPrefab);
		MegaBonusIcon iconObject = icon.GetComponent<MegaBonusIcon>();
		iconObject.SetBonus(_megaBonus, _image.sprite);
		_megaBonusButton.gameObject.SetActive(false);
    }

    public void SetVisible(bool isVisible)
    {
        if (isVisible)
        {
           // _background.gameObject.SetActive(isVisible);
            _megaBonusButton.gameObject.SetActive(isVisible);
        }
    }
}