using UnityEngine;

public class PassengerEscapeState : MoveState
{
    private PassengerSM _passenger;
    private bool _targetCalculated;
     
    public override void OnUpdate()
    {
        MovableCharacter.Animator.Play("walk");
        if (_passenger.IsMagnetTurnedOn())
        {
            _passenger.CalculateMagnet();
        }
        if (_targetCalculated)
        {
            float sqrRemainingDistance = MovableCharacter.GetTargetDistance();
            if (sqrRemainingDistance > _passenger.AttackMaxDistance)
            {
                _passenger.MakeIdle();
                return;
            }
            if (sqrRemainingDistance <= 0.1)
            {
                if (_passenger.GetPursuer() == null)
                {
                    _passenger.MakeIdle();
                }
                else
                {
                    _passenger.AttackIfPossible();
                }
                return;
            }
        }
        CalculateTarget();
        CalculatePosition();
    }

    private void CalculateTarget()
    {
        if (_passenger.GetPursuer() == null)
        {
            _passenger.MakeIdle();
            return;
        }
        float dot = Vector2.Dot(_passenger.GetPursuer().transform.position, _passenger.transform.position);
        float angle = Mathf.Acos(dot);
        Vector2 incrementVector = new Vector2(_passenger.AttackMaxDistance, 0);
        Vector2 rotated = Quaternion.Euler(0,0,angle) * incrementVector;
        Vector3 final = (Vector2)_passenger.transform.position + rotated;
        MonobehaviorHandler.GetMonobeharior().GetObject<Floor>("Floor").NormalizePosition(ref final, true);
        _passenger.SetEscapeTarget(final);
        _targetCalculated = true;
    }

    protected override void OnEnd()
    {
        _passenger.StopEscape();
        _targetCalculated = false;
    }

    protected override void OnStart()
    {
        _passenger.AttackTarget = null;
        _targetCalculated = false;
    }

    public PassengerEscapeState(StateMachine parent) : base(parent)
    {
        _passenger = (PassengerSM) parent;
    }
}
