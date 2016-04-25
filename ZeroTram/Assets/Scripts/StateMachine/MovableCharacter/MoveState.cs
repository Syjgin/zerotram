using UnityEngine;

public class MoveState : MovableCharacterState
{   
    protected void CalculatePosition()
    {
		if(MovableCharacter.GetTarget () != null) {
			Vector3 newPosition = Vector3.MoveTowards(MovableCharacter.transform.position, MovableCharacter.GetTarget(), MovableCharacter.Velocity * Time.deltaTime);
			newPosition.z = -1;
			MovableCharacter.transform.position = newPosition;	
		} else {
			MovableCharacter.MakeIdle ();
		}
    }

    public MoveState(StateMachine parent) : base(parent)
    {
    }
}
