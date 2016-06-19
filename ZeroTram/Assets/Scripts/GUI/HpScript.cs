using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HpScript : MonoBehaviour
{
    // public Image Num_Left;
    public Image Num_Middle;
    public Image Num_Right;
    private bool _bar_left;
    private bool _bar_middle;
    private bool _bar_right;
    private int _hp;
    private int _hp_old;
    private float _num_posY;
    private const float _baraban_speed = 15f;
    private const float _baraban_y = 50.5f;
    // Use this for initialization
    void Start()
    {
        _num_posY = Num_Right.transform.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    _bar_middle = true;
        //    _bar_right = true;
        //    _hp++;
        //}
        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    _bar_middle = true;
        //    _bar_right = true;
        //    _hp--;
        //}
        if (_hp != ConductorSM._hp)
        {
            _hp = ConductorSM._hp;
            // _bar_left = true;
            _bar_middle = true;
            _bar_right = true;
        }
        int xxx = Mathf.FloorToInt(_hp / 100);
        int xx = Mathf.FloorToInt((_hp - (xxx * 100)) / 10);
        int x = _hp - (xxx * 100) - (xx * 10);
        if (!_bar_middle && !_bar_right)
        {
            _hp_old = _hp;
        }
        else
        {
            if (_hp > _hp_old)
            {
                if (xx > 0)
                {
                    if (Num_Middle.transform.localPosition.y > (_num_posY - (_baraban_y * xx)))
                        Num_Middle.transform.Translate(0, 0.1f * _baraban_speed * (_hp_old - _hp), 0);
                    else
                    {
                        Num_Middle.transform.localPosition = new Vector3(Num_Middle.transform.localPosition.x, _num_posY - (_baraban_y * xx), Num_Middle.transform.localPosition.z);
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
                        Num_Right.transform.Translate(0, 0.1f * _baraban_speed * (_hp_old - _hp), 0);
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
            if (_hp < _hp_old)
            {
                if (xx < 9)
                {
                    if (Num_Middle.transform.localPosition.y < (_num_posY - (_baraban_y * xx)))
                        Num_Middle.transform.Translate(0, 0.1f * _baraban_speed * (_hp_old - _hp), 0);
                    else
                    {
                        Num_Middle.transform.localPosition = new Vector3(Num_Middle.transform.localPosition.x, _num_posY - (_baraban_y * xx), Num_Middle.transform.localPosition.z);
                        _bar_middle = false;
                    }
                }
                else
                {
                    if (Num_Middle.transform.localPosition.y > (_num_posY - (_baraban_y * 9)))
                        Num_Middle.transform.Translate(0, -0.45f * _baraban_speed, 0);
                    else
                    {
                        Num_Middle.transform.localPosition = new Vector3(Num_Middle.transform.localPosition.x, _num_posY - (_baraban_y * 9), Num_Middle.transform.localPosition.z);
                        _bar_middle = false;
                    }
                }
                if (x < 9)
                {
                    if (Num_Right.transform.localPosition.y < (_num_posY - (_baraban_y * x)))
                        Num_Right.transform.Translate(0, 0.1f * _baraban_speed * (_hp_old - _hp), 0);
                    else
                    {
                        Num_Right.transform.localPosition = new Vector3(Num_Right.transform.localPosition.x, _num_posY - (_baraban_y * x), Num_Right.transform.localPosition.z);
                        _bar_right = false;
                    }
                }
                else
                {
                    if (Num_Right.transform.localPosition.y > (_num_posY - (_baraban_y * 9)))
                        Num_Right.transform.Translate(0, -0.45f * _baraban_speed, 0);
                    else
                    {
                        Num_Right.transform.localPosition = new Vector3(Num_Right.transform.localPosition.x, _num_posY - (_baraban_y * 9), Num_Right.transform.localPosition.z);
                        _bar_right = false;
                    }
                }
            }
        }
    }
}
