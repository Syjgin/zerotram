using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ConductorWindow : MonoBehaviour
{

    [SerializeField]
    private Text _replica;
    [SerializeField]
    private Image _trainingPicture;

    [SerializeField]
    private Image _backgroundPicture;

    [SerializeField] private GameObject _window;

    [SerializeField] private Image _background;
    
    private bool _hideAfterClick;
    private const float MinimalShowTime = 0.2f;
    private bool _canBeHidden;
    private bool _showNextAfterClick;

    public void ForceHide()
    {
        _window.SetActive(false);
    }

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
        if(_backgroundPicture != null)
            _backgroundPicture.gameObject.SetActive(false);
        _replica.text = text;
        _hideAfterClick = hideAfterClick;
        StartCoroutine(WaitAndUnlock());
    }

    public void DisplayTextWithImage(string text, Sprite sprite, bool hideAfterClick, bool withCrossfade = false)
    {
        _canBeHidden = false;
        _window.SetActive(true);
        if (withCrossfade)
        {
            _background.canvasRenderer.SetAlpha(0.0f);
            _background.gameObject.SetActive(true);
            _background.CrossFadeAlpha(1, 3, true);
        }
        else
        {
            _background.gameObject.SetActive(true);
        }
        _backgroundPicture.gameObject.SetActive(true);
        _backgroundPicture.sprite = sprite;
        _backgroundPicture.SetNativeSize();
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
