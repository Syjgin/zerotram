using UnityEngine;

public class PassengerMoveState : MoveState
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
            if (MovableCharacter.IsGoingAway)
            {
                if (GameController.GetInstance().IsDoorsOpen() && MonobehaviorHandler.GetMonobeharior().GetObject<Floor>("Floor").IsPassengerNearDoors(_passenger))
                {
                    CalculateStickOnExit();
                    if (!_passenger.IsStick())
                    {
                        GameObject.Destroy(_passenger.gameObject);
                        return;
                    }
                }
            }
            else
            {
                if (!_passenger.IsStick())
                {
                    _passenger.MakeIdle();
                }
            }
            return;
        }
        CalculatePosition();
    }

    public PassengerMoveState(StateMachine parent) : base(parent)
    {
        _passenger = (PassengerSM) parent;
    }
    
    private void CalculateStickOnExit()
    {
        if (_passenger.GetActiveState().Equals((int)MovableCharacterSM.MovableCharacterStates.Stick))
            return;
        _passenger.CalculateStick();
        if (_passenger.IsStick())
        {
            MonobehaviorHandler.GetMonobeharior().GetObject<DoorsTimer>("DoorsTimer").SetPaused(true);
        }
    }
}
