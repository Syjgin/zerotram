using UnityEngine;

public class MoveState : MovableCharacterState
{
    protected override void OnStart()
    {
        MovableCharacter.CalculateOrientation(MovableCharacter.GetTarget());
    }
    
    public override void OnUpdate()
    {
        MovableCharacter.Animator.Play("walk");
        float sqrRemainingDistance = MovableCharacter.GetTargetDistance();
        if (sqrRemainingDistance <= 0.1f)
        {
            MovableCharacter.MakeIdle();
        }
        CalculatePosition();
    }

    protected void CalculatePosition()
    {
        Vector3 newPosition = Vector3.MoveTowards(MovableCharacter.transform.position, MovableCharacter.GetTarget(), MovableCharacter.Velocity * Time.deltaTime);
        newPosition.z = -1;
        MovableCharacter.transform.position = newPosition;
    }

    public MoveState(StateMachine parent) : base(parent)
    {
    }
}
