using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class ConductorWindowCollisionDetector : MonoBehaviour
{

    [SerializeField] private TrainingHandler _handler;
    [SerializeField] private ConductorWindow _window;

    public void MouseClick()
    {
        if (_window.Hide())
        {
            if (_window.ShowNextAfterClick())
            {
                _handler.ShowNext();
            }
            else
            {
                Time.timeScale = 1;
            }
        }
            
    }
}
