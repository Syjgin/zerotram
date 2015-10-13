using Assets;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class PassengerCollisionDetector : MonoBehaviour
{

    [SerializeField]
    private PassengerSM _passenger;

    void OnTriggerEnter2D(Collider2D other)
    {
        _passenger.HandleTriggerEnter(other);
    }

    void OnMouseDown()
    {
        MonobehaviorHandler.GetMonobeharior().GetObject<Floor>("Floor").OnMouseDown();
    }

    void OmMouseUp()
    {
        _passenger.StopDrag();
    }
}
