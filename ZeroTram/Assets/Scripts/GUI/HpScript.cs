using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HpScript : MonoBehaviour
{
    [SerializeField]
    private Image _numLeft;
    [SerializeField]
    private Image _numMiddle;
    [SerializeField]
    private Image _numRight;
   
    [SerializeField] private Image _mask;
    [SerializeField] private Sprite[] _maskSprites;
    private bool _bar_left;
    private bool _bar_middle;
    private bool _bar_right;
    private int _hp;
    private int _hp_old;
    private float _num_posY;
    private const float _baraban_speed = 15f;
    private const float _baraban_y = 52f;
    

    void Start()
    {
        _num_posY = _numRight.transform.localPosition.y;
    }
    
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    _bar_middle = true;
        //    _bar_right = true;
        //    HpPercent++;
        //}
        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    _bar_middle = true;
        //    _bar_right = true;
        //    HpPercent--;
        //}
        if (_hp != ConductorSM.HpPercent)
        {
            _hp = ConductorSM.HpPercent;
            _mask.gameObject.SetActive(_hp < 100);
            if (_mask.gameObject.activeInHierarchy)
            {
                _mask.sprite = _maskSprites[_hp];
            }
            _bar_left = true;
            _bar_middle = true;
            _bar_right = true;
        }
        int xxx = Mathf.FloorToInt(_hp / 100);
        int xx = Mathf.FloorToInt((_hp - (xxx * 100)) / 10);
        int x = _hp - (xxx * 100) - (xx * 10);
        
        if (!_bar_left && !_bar_middle && !_bar_right)
        {
            _hp_old = _hp;
        }

        if (_hp < _hp_old)
        {
            if (xxx < 9)
            {
                if (_numLeft.transform.localPosition.y < (_num_posY - (_baraban_y*xxx)))
                    _numLeft.transform.Translate(0, 0.1f*_baraban_speed*(_hp_old - _hp), 0);
                else
                {
                    _numLeft.transform.localPosition = new Vector3(_numLeft.transform.localPosition.x,
                        _num_posY - (_baraban_y*xxx), _numLeft.transform.localPosition.z);
                    _bar_left = false;
                }
            }
            else
            {
                if (_numLeft.transform.localPosition.y > (_num_posY - (_baraban_y*9)))
                    _numLeft.transform.Translate(0, -0.45f*_baraban_speed, 0);
                else
                {
                    _numLeft.transform.localPosition = new Vector3(_numLeft.transform.localPosition.x,
                        _num_posY - (_baraban_y*9), _numLeft.transform.localPosition.z);
                    _bar_left = false;
                }
            }
            if (xx < 9)
            {
                if (_numMiddle.transform.localPosition.y < (_num_posY - (_baraban_y*xx)))
                    _numMiddle.transform.Translate(0, 0.1f*_baraban_speed*(_hp_old - _hp), 0);
                else
                {
                    _numMiddle.transform.localPosition = new Vector3(_numMiddle.transform.localPosition.x,
                        _num_posY - (_baraban_y*xx), _numMiddle.transform.localPosition.z);
                    _bar_middle = false;
                }
            }
            else
            {
                if (_numMiddle.transform.localPosition.y > (_num_posY - (_baraban_y*9)))
                    _numMiddle.transform.Translate(0, -0.45f*_baraban_speed, 0);
                else
                {
                    _numMiddle.transform.localPosition = new Vector3(_numMiddle.transform.localPosition.x,
                        _num_posY - (_baraban_y*9), _numMiddle.transform.localPosition.z);
                    _bar_middle = false;
                }
            }
            if (x < 9)
            {
                if (_numRight.transform.localPosition.y < (_num_posY - (_baraban_y*x)))
                    _numRight.transform.Translate(0, 0.1f*_baraban_speed*(_hp_old - _hp), 0);
                else
                {
                    _numRight.transform.localPosition = new Vector3(_numRight.transform.localPosition.x,
                        _num_posY - (_baraban_y*x), _numRight.transform.localPosition.z);
                    _bar_right = false;
                }
            }
            else
            {
                if (_numRight.transform.localPosition.y > (_num_posY - (_baraban_y*9)))
                    _numRight.transform.Translate(0, -0.45f*_baraban_speed, 0);
                else
                {
                    _numRight.transform.localPosition = new Vector3(_numRight.transform.localPosition.x,
                        _num_posY - (_baraban_y*9), _numRight.transform.localPosition.z);
                    _bar_right = false;
                }
            }
        }
        else
        {
            if(_hp == _hp_old)
                return;
            {
                if (_numLeft.transform.localPosition.y > (_num_posY - (_baraban_y * xxx)))
                    _numLeft.transform.Translate(0, Time.deltaTime * -_baraban_speed, 0);
                else
                {
                    _numLeft.transform.localPosition = new Vector3(_numLeft.transform.localPosition.x, _num_posY - (_baraban_y * xxx), _numLeft.transform.localPosition.z);
                    _bar_left = false;
                }
                if (xx > 0)
                {
                    if (_numMiddle.transform.localPosition.y > (_num_posY - (_baraban_y * xx)))
                        _numMiddle.transform.Translate(0, 0.1f * _baraban_speed * (_hp_old - _hp), 0);
                    else
                    {
                        _numMiddle.transform.localPosition = new Vector3(_numMiddle.transform.localPosition.x, _num_posY - (_baraban_y * x), _numMiddle.transform.localPosition.z);
                        _bar_middle = false;
                    }
                }
                else
                {
                    if (_numMiddle.transform.localPosition.y < _num_posY)
                        _numMiddle.transform.Translate(0, 0.45f * _baraban_speed, 0);
                    else
                    {
                        _numMiddle.transform.localPosition = new Vector3(_numMiddle.transform.localPosition.x, _num_posY, _numMiddle.transform.localPosition.z);
                        _bar_middle = false;
                    }
                }
                if (x > 0)
                {
                    if (_numRight.transform.localPosition.y > (_num_posY - (_baraban_y * x)))
                        _numRight.transform.Translate(0, 0.1f * _baraban_speed * (_hp_old - _hp), 0);
                    else
                    {
                        _numRight.transform.localPosition = new Vector3(_numRight.transform.localPosition.x, _num_posY - (_baraban_y * x), _numRight.transform.localPosition.z);
                        _bar_right = false;
                    }
                }
                else
                {
                    if (_numRight.transform.localPosition.y < _num_posY)
                        _numRight.transform.Translate(0, 0.45f * _baraban_speed, 0);
                    else
                    {
                        _numRight.transform.localPosition = new Vector3(_numRight.transform.localPosition.x, _num_posY, _numRight.transform.localPosition.z);
                        _bar_right = false;
                    }
                }
            }
        }
        }
    }
