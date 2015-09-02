using System.Collections.Generic;
using Assets;
using Assets.Scripts.Math;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NewCharacterWindow : MonoBehaviour
{
    [SerializeField] private Text _description;
    [SerializeField] private List<Sprite> _images;
    [SerializeField] private Image _image;
    [SerializeField] private GameObject _window;

    public enum Character
    {
        Gnome = 0,
        Granny = 1,
        Cat = 2,
        Alien = 3,
        Bird = 4
    }

    private Character _currentCharacter;

    public const string Prefix = "CharacterWindow";

    public void OnExit()
    {
        PlayerPrefs.SetInt(Prefix + _currentCharacter, 1);
        Time.timeScale = 1;
        _window.SetActive(false);
    }
    
    public void SetCharacterToShow(Character character)
    {
        if(_window.activeSelf)
            return;
        _currentCharacter = character;
        ShowCharacter();
    }

    private void ShowCharacter()
    {
        Time.timeScale = 0;
        int imageIndex = (int)_currentCharacter;
        _image.sprite = _images[imageIndex];
        _image.SetNativeSize();
        string charStr = _currentCharacter.ToString();
        _description.text = ConfigReader.GetConfig().GetField("descriptions").GetField(charStr).str;
        _window.SetActive(true);
    }
}
