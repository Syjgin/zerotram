using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MyMessageScript : MonoBehaviour
{
    public Button btn;
    private List<string> _message = new List<string>();
    int x = 1;
    // Use this for initialization
    void Start()
    {
        btn.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            AddMessage("Сообщение: " + x.ToString());
            x++;
        }
        if (!btn.gameObject.activeSelf && _message.Count > 0)
        {
            btn.GetComponentInChildren<Text>().text = _message[0];
            btn.gameObject.SetActive(true);
        }
    }

    public void AddMessage(string message)
    {
        _message.Add(message);
    }

    public void RemoveMessage()
    {
        _message.RemoveAt(0);
        btn.gameObject.SetActive(false);
    }
}
