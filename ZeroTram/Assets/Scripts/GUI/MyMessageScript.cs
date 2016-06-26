using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MyMessageScript : MonoBehaviour
{
    [SerializeField] private GameObject _btn;
    private List<string> _message = new List<string>();
    private const float MessagePeriod = 3;
    private float _messageOpenedTime;
    // Use this for initialization
    void Start()
    {
        _btn.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!_btn.activeInHierarchy && _message.Count > 0)
        {
            _btn.GetComponentInChildren<Text>().text = _message[0];
            _btn.SetActive(true);
            _messageOpenedTime = Time.unscaledTime;
        }
        if (Time.unscaledTime - _messageOpenedTime > MessagePeriod)
        {
            if (_btn.activeInHierarchy)
            {
                RemoveMessage();
            }
        }
    }

    public void AddMessage(string message)
    {
        _message.Add(message);
    }

    public void RemoveMessage()
    {
        if(_message.Count > 0)
            _message.RemoveAt(0);
        _btn.SetActive(false);
    }
}
