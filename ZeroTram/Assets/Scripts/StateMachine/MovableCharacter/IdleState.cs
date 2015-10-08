using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
