using UnityEngine;

public class ConductorMoveState : MoveState
{
    private ConductorSM _conductor;
    public override void OnUpdate()
    {
        MovableCharacter.Animator.Play("walk");
        float sqrRemainingDistance = MovableCharacter.GetTargetDistance();
        if (sqrRemainingDistance <= 0.1f)
        {
            _conductor.StopStickIfNeeded();
            MovableCharacter.MakeIdle();
        }
        CalculatePosition();
    }
    
    public ConductorMoveState(StateMachine parent) : base(parent)
    {
        _conductor = (ConductorSM) parent;
    }
}
