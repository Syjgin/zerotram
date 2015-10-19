using UnityEngine;

public class MoveState : MovableCharacterState
{   
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
