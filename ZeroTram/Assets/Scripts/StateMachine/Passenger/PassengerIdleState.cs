﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PassengerIdleState : IdleState
{
    public PassengerIdleState(StateMachine parent) : base(parent)
    {
        _passenger = (PassengerSM) parent;
    }

    protected override void OnStart()
    {
        _timeForNextUpdate = 0;
    }

    private float _timeForNextUpdate;
    private PassengerSM _passenger;

    private bool CanMove()
    {
        if (_timeForNextUpdate > 0)
            return false;
        _timeForNextUpdate = _passenger.ChangeStatePeriod;
        return true;
    }

    public override void OnUpdate()
    {
        MovableCharacter.Animator.Play("idle");
        if (_passenger.IsMagnetTurnedOn())
        {
            _passenger.CalculateMagnet();
        }
        if (GameController.GetInstance().IsNearOtherPassenger(_passenger))
        {
            _passenger.CalculateRandomTarget();
            return;
        }
        if (MovableCharacter.AttackTarget != null)
        {
            float dist = _passenger.CalculatedAttackTargetDistance();
            if (dist > _passenger.AttackDistance)
            {
                _passenger.BeginHunt();
            }
            else
            {
                _passenger.AttackIfPossible();
            }
        }
        else
        {
            if(CanMove())
               _passenger.CalculateRandomTarget();
        }
    }

    public override void OnFixedUpdate()
    {
        _timeForNextUpdate -= Time.fixedDeltaTime;
    }
}
