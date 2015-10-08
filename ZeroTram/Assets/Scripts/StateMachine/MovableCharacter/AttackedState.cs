using UnityEngine;
public class AttackedState : MovableCharacterState
{

    public override void OnUpdate()
    {
        MovableCharacter.Animator.Play("attacked");
        if (MovableCharacter.IsAttackReationFinished())
        {
            MovableCharacter.MakeIdle();
        }
    }

    public AttackedState(StateMachine parent) : base(parent)
    {
    }
}
