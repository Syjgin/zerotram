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
}
