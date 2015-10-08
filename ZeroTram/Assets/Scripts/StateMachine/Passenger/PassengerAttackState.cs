using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Math;
using UnityEngine;

public class PassengerAttackState : AttackState
{
    private PassengerSM _passenger;
    public PassengerAttackState(StateMachine parent) : base(parent)
    {
        _passenger = (PassengerSM) parent;
    }

    protected override void OnStart()
    {
        TimeSinceAttackMade = MovableCharacter.AttackReloadPeriod;
    }

    public override void OnUpdate()
    {
        if (MovableCharacter.AttackTarget != null)
        {
            MovableCharacter.CalculateOrientation(MovableCharacter.AttackTarget.transform.position);
            if (CanAttack())
            {
                if (TimeSinceAttackMade >= MovableCharacter.AttackReloadPeriod)
                {
                    MovableCharacter.Animator.Play("attack");
                    MovableCharacter.MakeAttack();
                    TimeSinceAttackMade = 0;
                }
            }
            else
            {
                MovableCharacter.MakeIdle();
            }
        }
        else
        {
            MovableCharacter.MakeIdle();
        }
    }
    
    private bool CanAttack()
    {
        if (_passenger.CanNotInteract())
            return false;
        PassengerSM ps = _passenger.AttackTarget.GetComponent<PassengerSM>();
        if (ps != null)
        {
            if (ps.CanNotInteract())
            {
                _passenger.AttackTarget = null;
                return false;
            }
        }
        if (_passenger.AttackTargetDistance() <= MovableCharacter.AttackDistance)
            return true;
        return false;
    }

}
