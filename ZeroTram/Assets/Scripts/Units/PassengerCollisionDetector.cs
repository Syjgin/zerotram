using Assets;
using UnityEngine;
using System.Collections;

public class PassengerCollisionDetector : MonoBehaviour
{

    [SerializeField] public Passenger _passenger;

    void OnTriggerEnter2D(Collider2D other)
    {
        _passenger.HandleTriggerEnter(other);
    }

    void OnMouseDown()
    {
        if(Time.timeScale == 0)
            return;
        _passenger.HandleClick();
    }
}
