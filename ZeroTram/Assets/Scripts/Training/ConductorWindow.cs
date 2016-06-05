using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ConductorWindow : MonoBehaviour
{

    [SerializeField]
    private Text _replica;
    [SerializeField]
    private Image _trainingPicture;

    [SerializeField] private GameObject _window;

    [SerializeField] private Image _background;

    private Sprite _baseSprite;
    private bool _hideAfterClick;
    private const float MinimalShowTime = 0.2f;
    private bool _canBeHidden;
    private bool _showNextAfterClick;

    public bool Hide()
    {
        if (!_canBeHidden)
            return false;
        if(_hideAfterClick)
            _window.SetActive(false);
        return true;
    }

    private void Awake()
    {
        _baseSprite = _trainingPicture.sprite;
        _hideAfterClick = true;
    }

    public bool ShowNextAfterClick()
    {
        return _showNextAfterClick;
    }

    public void DisplayText(string text, bool hideAfterClick, bool showNextAfterClick=true)
    {
        _showNextAfterClick = showNextAfterClick;
        _canBeHidden = false;
        _window.SetActive(true);
        _background.gameObject.SetActive(false);
        if (_trainingPicture.sprite.name != _baseSprite.name)
        {
            _trainingPicture.sprite = _baseSprite;
        }
        _replica.text = text;
        _hideAfterClick = hideAfterClick;
        StartCoroutine(WaitAndUnlock());
    }

    public void DisplayTextWithImage(string text, Sprite sprite, bool hideAfterClick)
    {
        _canBeHidden = false;
        _window.SetActive(true);
        _background.gameObject.SetActive(true);
        _trainingPicture.sprite = sprite;
        _replica.text = text;
        _hideAfterClick = hideAfterClick;
        StartCoroutine(WaitAndUnlock());
    }

    private IEnumerator WaitAndUnlock()
    {
        float start = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < start + MinimalShowTime)
        {
            yield return null;
        }
        _canBeHidden = true;
    }
}
