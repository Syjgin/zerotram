using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Math;
using UnityEditor;

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

    public String CurrentPassengerClassName()
    {
        if (CurrentPassenger == null)
            return String.Empty;
        return CurrentPassenger.GetClassName();
    }

    private int GetSitPossibility()
    {
        return (int)ConfigReader.GetConfig().GetField("tram").GetField("SitPossibility").n;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!_isCheckPossible)
            return;
        _isCheckPossible = false;
        _timeAfterPassengerCheck = 0;
        PassengerSM passenger = other.GetComponentInParent<PassengerSM>();
        if (passenger != null)
        {
            if(passenger.IsOnTheBench())
                return;
            if(CurrentPassenger != null)
                return;
            if (Randomizer.GetPercentageBasedBoolean(GetSitPossibility()))
            {
                CurrentPassenger = passenger;
                if (passenger.GetActiveState() == (int) MovableCharacterSM.MovableCharacterStates.Dragged && passenger.HasTicket())
                {
                    passenger.StopDrag();
                }
                passenger.HandleSitdown(this);
            }
        }
    }
}
