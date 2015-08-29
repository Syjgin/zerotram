using Assets;
using UnityEngine;
using System.Collections;

public class HeroCollisionDetector : MonoBehaviour
{

    [SerializeField] public Hero _hero;

    void OnMouseDown()
    {
        if(Time.timeScale == 0)
            return;
        Debug.Log("hero click");
    }

    void OnMouseUp()
    {
        if (Time.timeScale == 0)
            return;
        _hero.StopDrag();
    }
}
