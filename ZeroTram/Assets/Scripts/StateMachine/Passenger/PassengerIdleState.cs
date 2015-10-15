using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets;
using Assets.Scripts.Math;
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
        if (GameController.GetInstance().IsNearOtherPassenger(_passenger))
        {
            _passenger.CalculateRandomTarget();
            return;
        }
        if (MovableCharacter.AttackTarget != null)
        {
            float dist = _passenger.AttackTargetDistance();
            if (dist > _passenger.AttackMaxDistance)
            {
                _passenger.AttackTarget = null;
            }
            else
            {
                if (dist > _passenger.AttackDistance)
                {
                    Vector3 result =
                        _passenger.AttackTarget.BoxCollider2D.bounds.ClosestPoint(_passenger.transform.position);
                    _passenger.SetTarget(result);
                }
                else
                {
                    _passenger.ActivateState((int) MovableCharacterSM.MovableCharacterStates.Attack);
                }
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
