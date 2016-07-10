using UnityEngine;
public class PassengerDraggedState : MovableCharacterState
{
    private float _timeSinceStateChanged;
    private PassengerSM _passenger;
    private ConductorSM _hero;
    private Floor _floor;

    protected override void OnStart()
    {
        _timeSinceStateChanged = 0;
        if (_hero == null)
        {
            _hero = MonobehaviorHandler.GetMonobeharior().GetObject<Floor>("Floor").GetHero();
        }
        if (_floor == null)
        {
            _floor = MonobehaviorHandler.GetMonobeharior().GetObject<Floor>("Floor");
        }
    }

    protected override void OnEnd()
    {
        if(MovableCharacter.IsUnfreezeIsTemporary())
            MovableCharacter.Freeze();
    }

    public override void OnUpdate()
    {
        if (_timeSinceStateChanged >= _passenger.DragChangeStatePeriod)
        {
            _timeSinceStateChanged = 0;
            if ((Randomizer.GetPercentageBasedBoolean((int)_passenger.GetAttackProbabilityForClass("conductor")) && !_passenger.IsNearBench) || !_passenger.HasTicket())
            {
                if (!_passenger.IsRunawayDenied())
                {
                    StopDragByConductor(true);
                    return;
                }
            }
        }
        if (_hero.CanKick(_passenger) && !_passenger.HasTicket())
        {
            if (!_passenger.IsRunawayDenied() && Randomizer.GetPercentageBasedBoolean((int)_passenger.GetRunAwayProbabilityForClass("conductor")))
            {
                _floor.OnMouseUp();
                _passenger.BeginEscape(_hero);
                return;
            }
        }
        MovableCharacter.Animator.Play("attacked");
        Vector3 targetPos = new Vector3();
        if (!_floor.GetCurrentMousePosition(ref targetPos))
            return;
        MovableCharacter.transform.position = new Vector3(targetPos.x, targetPos.y, -1);
    }

    private void StopDragByConductor(bool attack)
    {
        _hero.StopDrag(attack);
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
