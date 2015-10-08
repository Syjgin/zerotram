using Assets;
using UnityEngine;
using System.Collections;

public class ConductorCollisionDetector : MonoBehaviour
{

    [SerializeField] public ConductorSM _conductor;

    void OnMouseDown()
    {
        if(Time.timeScale == 0)
            return;
        _conductor.HandleClick();
    }

    void OnMouseUp()
    {
        if (Time.timeScale == 0)
            return;
        _conductor.StopDrag();
        GameController.GetInstance().UndragAll();
    }
}
