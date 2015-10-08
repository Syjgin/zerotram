using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class AttackState : MovableCharacterState
{
    protected float TimeSinceAttackMade;
    protected override void OnStart()
    {
        TimeSinceAttackMade = 0;
    }

    public override void OnUpdate()
    {
        MovableCharacter.Animator.Play("attack");
        if (MovableCharacter.AttackTarget == null && TimeSinceAttackMade > MovableCharacter.AttackReloadPeriod)
        {
            MovableCharacter.MakeIdle();
        }
    }

    public override void OnFixedUpdate()
    {
        TimeSinceAttackMade += Time.fixedDeltaTime;
    }
    
    public AttackState(StateMachine parent) : base(parent)
    {
    }
}
