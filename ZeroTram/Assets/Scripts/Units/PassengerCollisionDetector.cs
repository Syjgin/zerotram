using Assets;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class PassengerCollisionDetector : MonoBehaviour
{

    [SerializeField] public Passenger _passenger;

    void OnTriggerEnter2D(Collider2D other)
    {
        Hero hero = other.GetComponentInParent<Hero>();
        _passenger.HandleTriggerEnter(other);
    }

    void OnMouseDown()
    {
        if(Time.timeScale == 0)
            return;
        _passenger.HandleClick();
    }

    void OmMouseUp()
    {
        _passenger.StopDrag();
    }
}
