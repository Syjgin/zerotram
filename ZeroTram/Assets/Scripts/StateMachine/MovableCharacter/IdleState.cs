using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using UnityEngine;

public class IdleState : MovableCharacterState
{
    public override void OnUpdate()
    {
        MovableCharacter.Animator.Play("idle");
    }

    public IdleState(StateMachine parent) : base(parent)
    {
    }
}
