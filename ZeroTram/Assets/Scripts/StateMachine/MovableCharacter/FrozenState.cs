using UnityEngine;
public class FrozenState : MovableCharacterState
{
    protected override void OnStart()
    {
        MovableCharacter.Animator.enabled = false;
    }

    protected override void OnEnd()
    {
        if(!MovableCharacter.IsUnfreezeIsTemporary())
            MovableCharacter.Animator.enabled = true;
    }

    public override bool IsTransitionAllowed()
    {
        return !MovableCharacter.IsFrozen();
    }

    public FrozenState(StateMachine parent) : base(parent)
    {
    }
}
