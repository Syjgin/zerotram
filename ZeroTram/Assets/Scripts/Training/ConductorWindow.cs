using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ConductorWindow : MonoBehaviour
{

    [SerializeField]
    private Text _replica;
    [SerializeField] private TrainingHandler _handler;
    [SerializeField]
    private Image _trainingPicture;

    [SerializeField] private Image _background;

    private Sprite _baseSprite;
    private bool _hideAfterClick;

    public void OnMouseClicked()
    {
        bool hide = _hideAfterClick;
        _handler.ShowNext();
        if (hide)
        {
            gameObject.SetActive(false);
            Time.timeScale = 1;
        }
    }

    private void Awake()
    {
        _baseSprite = _trainingPicture.sprite;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnMouseClicked();
        }
    }

    public void DisplayText(string text, bool hideAfterClick)
    {
        _background.gameObject.SetActive(false);
        if (_trainingPicture.sprite.name != _baseSprite.name)
        {
            _trainingPicture.sprite = _baseSprite;
        }
        _replica.text = text;
        _hideAfterClick = hideAfterClick;
    }

    public void DisplayTextWithImage(string text, Sprite sprite, bool hideAfterClick)
    {
        _background.gameObject.SetActive(true);
        _trainingPicture.sprite = sprite;
        _replica.text = text;
        _hideAfterClick = hideAfterClick;
    }
}
