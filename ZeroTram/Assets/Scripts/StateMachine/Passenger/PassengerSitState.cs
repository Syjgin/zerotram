using UnityEngine;

public class PassengerSitState : MovableCharacterState
{
    private PassengerSM _passenger;
    private float _timeLeft;

    public override void OnUpdate()
    {
        if (_passenger.IsGoingAway)
        {
            _passenger.HandleStandUp();
            return;
        }
        if (_timeLeft > _passenger.GetStopStandPeriod())
        {
            _timeLeft = 0;
            if (Randomizer.GetPercentageBasedBoolean(_passenger.GetStandPossibility()))
            {
                _passenger.HandleStandUp();
            }
        }
    }

    public PassengerSitState(StateMachine parent) : base(parent)
    {
        _passenger = (PassengerSM) parent;
    }

    protected override void OnStart()
    {
        _timeLeft = 0;
        MovableCharacter.Animator.Play("idle");
    }

    protected override void OnEnd()
    {
        _passenger.HandleStandUp();
    }

    public override void OnFixedUpdate()
    {
        _timeLeft += Time.fixedDeltaTime;
    }
}
