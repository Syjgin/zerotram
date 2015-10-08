using UnityEngine;
public class PassengerAttackedState : AttackedState
{
    private PassengerSM _passenger;
    public override void OnUpdate()
    {
        if (MovableCharacter.IsAttackReationFinished())
        {
            _passenger.CalculateAttackReaction();
        }
    }

    public PassengerAttackedState(StateMachine parent) : base(parent)
    {
        _passenger = (PassengerSM) parent;
    }
}
