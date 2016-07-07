using UnityEngine;
public class PassengerDraggedState : MovableCharacterState
{
    private float _timeSinceStateChanged;
    private PassengerSM _passenger;

    protected override void OnStart()
    {
        _timeSinceStateChanged = 0;
    }

    protected override void OnEnd()
    {
        if(MovableCharacter.IsUnfreezeIsTemporary())
            MovableCharacter.Freeze();
    }

    public override void OnUpdate()
    {
        if (MonobehaviorHandler.GetMonobeharior().GetObject<Floor>("Floor").IsPassengerNearDoors(_passenger) && !_passenger.HasTicket())
        {
            if (!_passenger.IsRunawayDenied())
            {
                StopDragByConductor(false);
                return;
            }
        }
        if (_timeSinceStateChanged >= _passenger.DragChangeStatePeriod)
        {
            _timeSinceStateChanged = 0;
            if ((Randomizer.GetPercentageBasedBoolean((int) _passenger.GetAttackProbabilityForClass("conductor")) && !_passenger.IsNearBench) || !_passenger.HasTicket())
            {
                if (!_passenger.IsRunawayDenied())
                {
                    StopDragByConductor(true);
                    return;
                }
            }
        }
        MovableCharacter.Animator.Play("attacked");
        Vector3 targetPos = new Vector3();
        if (!MonobehaviorHandler.GetMonobeharior().GetObject<Floor>("Floor").GetCurrentMousePosition(ref targetPos))
            return;
        MovableCharacter.transform.position = new Vector3(targetPos.x, targetPos.y, -1);
    }

    private void StopDragByConductor(bool attack)
    {
        ConductorSM conductor = MonobehaviorHandler.GetMonobeharior().GetObject<Floor>("Floor").GetHero();
        conductor.StopDrag(attack);
    }

    public PassengerDraggedState(StateMachine parent) : base(parent)
    {
        _passenger = (PassengerSM) parent;
    }

    public override bool IsTransitionAllowed()
    {
        return !MonobehaviorHandler.GetMonobeharior().GetObject<Floor>("Floor").GetHero().IsDragging();
    }

    public override void OnFixedUpdate()
    {
        _timeSinceStateChanged += Time.fixedDeltaTime;
    }
}
