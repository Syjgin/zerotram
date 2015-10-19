using Assets;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class PassengerCollisionDetector : CollisionDetector
{
    private PassengerSM _passenger;

    void Awake()
    {
        _passenger = (PassengerSM) Character;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        _passenger.HandleTriggerEnter(other);
    }
    
    void OmMouseUp()
    {
        _passenger.StopDrag();
    }
}
