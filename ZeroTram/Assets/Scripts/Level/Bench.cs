using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Math;

public class Bench : MonoBehaviour
{
    [HideInInspector]public PassengerSM CurrentPassenger;

    private float _timeAfterPassengerCheck;
    private float _maxWaitingTime;
    private bool _isCheckPossible;

    void Start()
    {
        _timeAfterPassengerCheck = 0;
        _maxWaitingTime = ConfigReader.GetConfig().GetField("tram").GetField("SitRecheckPeriod").n;
        _isCheckPossible = true;
    }

    void FixedUpdate()
    {
        _timeAfterPassengerCheck += Time.fixedDeltaTime;
        if (_timeAfterPassengerCheck > _maxWaitingTime)
        {
            _isCheckPossible = true;
        }
    }

    public string CurrentPassengerClassName()
    {
        if (CurrentPassenger == null)
            return string.Empty;
        return CurrentPassenger.GetClassName();
    }

    private int GetSitPossibility()
    {
        return (int)ConfigReader.GetConfig().GetField("tram").GetField("SitPossibility").n;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        PassengerSM passenger = other.GetComponentInParent<PassengerSM>();
        if (passenger != null)
        {
            passenger.IsNearBench = false;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!_isCheckPossible)
            return;
        _isCheckPossible = false;
        _timeAfterPassengerCheck = 0;
        PassengerCollisionDetector passengerCD = other.GetComponent<PassengerCollisionDetector>();
        if (passengerCD != null)
        {
            PassengerSM passenger = (PassengerSM)passengerCD.Character;
            TryHaveSetPassenger(passenger);
            return;
        }
        ConductorCollisionDetector conductorCD = other.GetComponentInParent<ConductorCollisionDetector>();
        if (conductorCD != null)
        {
            ConductorSM conductor = (ConductorSM) conductorCD.Character;
            if (conductor.IsDragging())
            {
                PassengerSM draggedPassenger = conductor.GetDragTarget();
                TryHaveSetPassenger(draggedPassenger);
            }
        }
    }

    private void TryHaveSetPassenger(PassengerSM passenger)
    {
        passenger.IsNearBench = true;
        if (passenger.IsOnTheBench())
        {
            CurrentPassenger = passenger;
            return;
        }
        if (Randomizer.GetPercentageBasedBoolean(GetSitPossibility()))
        {
            CurrentPassenger = passenger;
            if (passenger.GetActiveState() == (int) MovableCharacterSM.MovableCharacterStates.Dragged &&
                passenger.HasTicket())
            {
                passenger.StopDrag(false);
            }
            passenger.HandleSitdown(this);
        }
    }
}
