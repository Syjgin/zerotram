using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class KilledPassengers : MonoBehaviour, GameStateNotificationListener
{
    public Image Num_Left;
    public Image Num_Right;
    public Text Txt;
    private bool _shouldUpdate;
    private bool _bar_left;
    private bool _bar_right;
    private int _killed_old;
    private int _killed = -1;
    private float _num_left_posY;
    private float _num_right_posY;
    private const float _baraban_speed = 15f;
    private const float _baraban_y = 47f;
    [SerializeField] private Image _indicator;
    [SerializeField]
    private Sprite _blueSprite;
    [SerializeField]
    private Sprite _redSprite;

    void Start()
    {
        Txt.text = "-";
        _num_left_posY = Num_Left.transform.localPosition.y;
        _num_right_posY = Num_Right.transform.localPosition.y;
        _shouldUpdate = true;
        GameController.GetInstance().AddListener(this);
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    _bar_left = true;
        //    _bar_right = true;
        //    _killed++;
        //}
        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    _bar_left = true;
        //    _bar_right = true;
        //    _killed--;
        //}
        int xx = Mathf.FloorToInt(_killed / 10);
        int x = _killed - (xx * 10);
        if (!_bar_left && !_bar_right)
        {
            _killed_old = _killed;
        }
        else
        {
            if (_killed > _killed_old)
            {
                if (Num_Left.transform.localPosition.y > (_num_left_posY - (_baraban_y * xx)))
                    Num_Left.transform.Translate(0, 0.1f * -_baraban_speed, 0);
                else
                {
                    Num_Left.transform.localPosition = new Vector3(Num_Left.transform.localPosition.x, _num_left_posY - (_baraban_y * xx), Num_Left.transform.localPosition.z);
                    _bar_left = false;
                }
                if (x > 0)
                {
                    if (Num_Right.transform.localPosition.y > (_num_right_posY - (_baraban_y * x)))
                        Num_Right.transform.Translate(0, 0.1f * _baraban_speed * (_killed_old - _killed), 0);
                    //  Num_Right.transform.Translate(0, Time.deltaTime * _baraban_right_speed * (_killed_old - _killed), 0);
                    else
                    {
                        Num_Right.transform.localPosition = new Vector3(Num_Right.transform.localPosition.x, _num_right_posY - (_baraban_y * x), Num_Right.transform.localPosition.z);
                        _bar_right = false;
                    }
                }
                else
                {
                    if (Num_Right.transform.localPosition.y < _num_right_posY)
                        Num_Right.transform.Translate(0, 0.45f * _baraban_speed, 0);
                  //  Num_Right.transform.Translate(0, Time.deltaTime * _baraban_right_speed * 9, 0);
                    else
                    {
                        Num_Right.transform.localPosition = new Vector3(Num_Right.transform.localPosition.x, _num_right_posY, Num_Right.transform.localPosition.z);
                        _bar_right = false;
                    }
                }
            }
            if (_killed < _killed_old)
            {
                if (Num_Left.transform.localPosition.y < (_num_left_posY - (_baraban_y * xx)))
                    Num_Left.transform.Translate(0, 0.1f * _baraban_speed, 0);
                else
                {
                    Num_Left.transform.localPosition = new Vector3(Num_Left.transform.localPosition.x, _num_left_posY - (_baraban_y * xx), Num_Left.transform.localPosition.z);
                    _bar_left = false;
                }
                if (x < 9)
                {
                    if (Num_Right.transform.localPosition.y < (_num_right_posY - (_baraban_y * x)))
                        Num_Right.transform.Translate(0, 0.1f * _baraban_speed * (_killed_old - _killed), 0);
                    else
                    {
                        Num_Right.transform.localPosition = new Vector3(Num_Right.transform.localPosition.x, _num_right_posY - (_baraban_y * x), Num_Right.transform.localPosition.z);
                        _bar_right = false;
                    }
                }
                else
                {
                    if (Num_Right.transform.localPosition.y > (_num_right_posY - (_baraban_y * 9)))
                        Num_Right.transform.Translate(0, -0.45f * _baraban_speed, 0);
                    else
                    {
                        Num_Right.transform.localPosition = new Vector3(Num_Right.transform.localPosition.x, _num_right_posY - (_baraban_y * 9), Num_Right.transform.localPosition.z);
                        _bar_right = false;
                    }
                }
            }
        }
    }

    private IEnumerator Blink(GameController.StateInformation information)
    {
        for (int i = 0; i < 5; i++)
        {
            _indicator.sprite = i % 2 == 0 ? _blueSprite : _redSprite;
            yield return new WaitForSeconds(0.2f);
        }
        _indicator.sprite = information.RemainKilled < 2 ? _redSprite : _blueSprite;
    }

    void OnDestroy()
    {
        GameController.GetInstance().RemoveListener(this);
    }

    public void UpdateInfo(GameController.StateInformation information)
    {
        if (_shouldUpdate)
        {
            Txt.text = information.RemainKilled < 0 ? "-" : information.RemainKilled.ToString();
        }
        if (_killed != information.RemainKilled)
        {
            _killed = information.RemainKilled;
            _bar_left = true;
            _bar_right = true;
            if (_killed == -1)
            {
                _indicator.sprite = information.RemainKilled < 2 ? _redSprite : _blueSprite;
            }
            else
            {
                if (_indicator.gameObject.activeInHierarchy)
                    StartCoroutine(Blink(information));
            }
        }
    }

    public void GameOver()
    {
        _shouldUpdate = false;
    }
}
