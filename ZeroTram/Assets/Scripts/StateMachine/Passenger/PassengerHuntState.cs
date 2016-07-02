using UnityEngine;

public class PassengerHuntState : MoveState
{
    private PassengerSM _passenger;
    
    public override void OnUpdate()
    {
        MovableCharacter.Animator.Play("walk");
        if (_passenger.IsMagnetTurnedOn())
        {
            _passenger.CalculateMagnet();
        }
        float sqrRemainingDistance = MovableCharacter.GetTargetDistance();
        if (sqrRemainingDistance <= 0.1)
        {
            if (_passenger.AttackTarget == null)
            {
                _passenger.MakeIdle();
            }
            else
            {
                _passenger.AttackIfPossible();
            }
            return;
        }
        CalculatePosition();
    }

    public PassengerHuntState(StateMachine parent) : base(parent)
    {
        _passenger = (PassengerSM) parent;
    }

    protected void CalculateOrientation(Vector2 target)
    {
        if (target.x > MovableCharacter.transform.position.x)
        {
            MovableCharacter.CharacterBody.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            MovableCharacter.CharacterBody.transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
