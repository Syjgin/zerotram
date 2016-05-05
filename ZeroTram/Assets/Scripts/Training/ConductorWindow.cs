using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ConductorWindow : MonoBehaviour
{

    [SerializeField]
    private Text _replica;
    [SerializeField] private TrainingHandler _handler;

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

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnMouseClicked();
        }
    }

    public void DisplayText(string text, bool hideAfterClick)
    {
        _replica.text = text;
        _hideAfterClick = hideAfterClick;
    }
}
