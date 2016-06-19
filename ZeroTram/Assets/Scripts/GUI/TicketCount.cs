using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TicketCount : MonoBehaviour, GameStateNotificationListener
{
    public Image Num_Left;
    public Image Num_Middle;
    public Image Num_Right;
    private bool _bar_left;
    private bool _bar_middle;
    private bool _bar_right;
    private int _ticket;
    private int _ticket_old;
    private float _num_posY;
    private const float _baraban_speed = 15f;
    private const float _baraban_y = 47.5f;

    void Start()
    {
        _num_posY = Num_Left.transform.localPosition.y;
        GameController.GetInstance().AddListener(this);
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    _bar_left = true;
        //    _bar_middle = true;
        //    _bar_right = true;
        //    _ticket++;
        //}
        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    _bar_left = true;
        //    _bar_middle = true;
        //    _bar_right = true;
        //    _ticket--;
        //}
        int xxx = Mathf.FloorToInt(_ticket / 100);
        int xx = Mathf.FloorToInt((_ticket - (xxx * 100)) / 10);
        int x = _ticket - (xxx * 100) - (xx * 10);
        if (!_bar_left && !_bar_middle && !_bar_right)
        {
            _ticket_old = _ticket;
        }
        else
        {
            if (Num_Left.transform.localPosition.y > (_num_posY - (_baraban_y * xxx)))
                Num_Left.transform.Translate(0, Time.deltaTime * -_baraban_speed, 0);
            else
            {
                Num_Left.transform.localPosition = new Vector3(Num_Left.transform.localPosition.x, _num_posY - (_baraban_y * xxx), Num_Left.transform.localPosition.z);
                _bar_left = false;
            }
            if (xx > 0)
            {
                if (Num_Middle.transform.localPosition.y > (_num_posY - (_baraban_y * xx)))
                    Num_Middle.transform.Translate(0, 0.1f * _baraban_speed * (_ticket_old - _ticket), 0);
                else
                {
                    Num_Middle.transform.localPosition = new Vector3(Num_Middle.transform.localPosition.x, _num_posY - (_baraban_y * x), Num_Middle.transform.localPosition.z);
                    _bar_middle = false;
                }
            }
            else
            {
                if (Num_Middle.transform.localPosition.y < _num_posY)
                    Num_Middle.transform.Translate(0, 0.45f * _baraban_speed, 0);
                else
                {
                    Num_Middle.transform.localPosition = new Vector3(Num_Middle.transform.localPosition.x, _num_posY, Num_Middle.transform.localPosition.z);
                    _bar_middle = false;
                }
            }
            if (x > 0)
            {
                if (Num_Right.transform.localPosition.y > (_num_posY - (_baraban_y * x)))
                    Num_Right.transform.Translate(0, 0.1f * _baraban_speed * (_ticket_old - _ticket), 0);
                else
                {
                    Num_Right.transform.localPosition = new Vector3(Num_Right.transform.localPosition.x, _num_posY - (_baraban_y * x), Num_Right.transform.localPosition.z);
                    _bar_right = false;
                }
            }
            else
            {
                if (Num_Right.transform.localPosition.y < _num_posY)
                    Num_Right.transform.Translate(0, 0.45f * _baraban_speed, 0);
                else
                {
                    Num_Right.transform.localPosition = new Vector3(Num_Right.transform.localPosition.x, _num_posY, Num_Right.transform.localPosition.z);
                    _bar_right = false;
                }
            }
        }
    }

    void OnDestroy()
    {
        GameController.GetInstance().RemoveListener(this);
    }
    
    public void UpdateInfo(GameController.StateInformation information)
    {
        if (_ticket != information.TicketCount)
        {
            _ticket = information.TicketCount;
            _bar_left = true;
            _bar_middle = true;
            _bar_right = true;
        }
    }

    public void GameOver()
    {

    }
}
