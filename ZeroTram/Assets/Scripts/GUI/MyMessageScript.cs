using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MyMessageScript : MonoBehaviour
{
    [SerializeField] private Button _btn;
    private List<string> _message = new List<string>();
    private const float MessagePeriod = 5;
    private float _messageOpenedTime;
    // Use this for initialization
    void Start()
    {
        _btn.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!_btn.gameObject.activeSelf && _message.Count > 0)
        {
            _btn.GetComponentInChildren<Text>().text = _message[0];
            _btn.gameObject.SetActive(true);
            _messageOpenedTime = Time.timeSinceLevelLoad;
        }
        if (Time.timeSinceLevelLoad - _messageOpenedTime > MessagePeriod)
        {
            if (_btn.gameObject.activeInHierarchy)
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
        _btn.gameObject.SetActive(false);
    }
}
