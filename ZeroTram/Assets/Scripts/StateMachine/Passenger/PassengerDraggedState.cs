using Assets.Scripts.Math;
using UnityEngine;
public class PassengerDraggedState : MovableCharacterState
{
    private float _timeSinceStateChanged;
    private PassengerSM _passenger;

    protected override void OnStart()
    {
        _timeSinceStateChanged = 0;
    }

    public override void OnUpdate()
    {
        if (_timeSinceStateChanged >= _passenger.DragChangeStatePeriod)
        {
            _timeSinceStateChanged = 0;
            if (Randomizer.GetPercentageBasedBoolean((int) _passenger.AttackProbability))
            {
                ConductorSM conductor = MonobehaviorHandler.GetMonobeharior().GetObject<Floor>("Floor").GetHero();
                conductor.StopDrag();
                MovableCharacter.AttackTarget = conductor;
                MovableCharacter.ActivateState((int)MovableCharacterSM.MovableCharacterStates.Attack);
            }
        }
        MovableCharacter.Animator.Play("attacked");
        Vector2 targetPos = MonobehaviorHandler.GetMonobeharior().GetObject<Floor>("Floor").GetCurrentMousePosition();
        MovableCharacter.transform.position = new Vector3(targetPos.x, targetPos.y, -1);
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
