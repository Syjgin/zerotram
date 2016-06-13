using System;
using UnityEngine;
using System.Collections;

public class Bench : MonoBehaviour
{
    private float _timeAfterPassengerCheck;
    private float _maxWaitingTime;
    private bool _isCheckPossible;
    private bool _checkStateEnabled;

    void Start()
    {
        _timeAfterPassengerCheck = 0;
        _maxWaitingTime = ConfigReader.GetConfig().GetField("tram").GetField("SitRecheckPeriod").n;
        _isCheckPossible = true;
        _checkStateEnabled = true;
    }

    public void SetCheckState(bool state)
    {
        _checkStateEnabled = state;
    }

    void FixedUpdate()
    {
        _timeAfterPassengerCheck += Time.fixedDeltaTime;
        if (_timeAfterPassengerCheck > _maxWaitingTime)
        {
            _isCheckPossible = true;
        }
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
        if (!_isCheckPossible && _checkStateEnabled)
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
            ConductorSM conductor = (ConductorSM)conductorCD.Character;
            if (conductor.IsDragging())
            {
                PassengerSM draggedPassenger = conductor.GetDragTarget();
                TryHaveSetPassenger(draggedPassenger);
            }
        }
    }

    private void TryHaveSetPassenger(PassengerSM passenger)
    {
        if (passenger.IsGoingAway)
        {
            passenger.SetTarget(new Vector2());
            return;
        }
        if (passenger.GetActiveState() != (int) MovableCharacterSM.MovableCharacterStates.Dragged && passenger.IsSitListenerActivated())
        {
            passenger.CalculateRandomTarget();
            return;
        }
        passenger.IsNearBench = true;
        if (passenger.IsOnTheBench())
        {
            return;
        }
        if (Randomizer.GetPercentageBasedBoolean(GetSitPossibility()) || passenger.IsSitListenerActivated())
        {
            if (passenger.GetActiveState() == (int) MovableCharacterSM.MovableCharacterStates.Dragged &&
                passenger.HasTicket())
            {
                passenger.StopDrag(false);
                passenger.ActivateSitListener();
            }
            passenger.HandleSitdown(this);
        }
    }
}
