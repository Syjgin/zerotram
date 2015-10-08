using Assets;
using UnityEngine;

public class PassengerMoveState : MoveState
{
    private PassengerSM _passenger;
    
    public override void OnUpdate()
    {
        MovableCharacter.Animator.Play("walk");
        float sqrRemainingDistance = MovableCharacter.GetTargetDistance();
        if (sqrRemainingDistance <= 0.1)
        {
            if (MovableCharacter.IsGoingAway)
            {
                if (GameController.GetInstance().IsDoorsOpen())
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
                    if (_passenger.AttackTarget == null)
                    {
                        _passenger.MakeIdle();
                    }
                    else
                    {
                       _passenger.ActivateState((int)MovableCharacterSM.MovableCharacterStates.Attack);
                    }
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

    private void CalculateStickOnExit()
    {
        if (_passenger.GetActiveState().Equals((int)MovableCharacterSM.MovableCharacterStates.Stick))
            return;
        _passenger.CalculateStick();
        if (_passenger.IsStick())
        {
            _passenger.transform.position = new Vector3(_passenger.GetTarget().x, _passenger.GetTarget().y - Spawner.StickYOffset, -1);
            DoorsHandler.GetTimer().SetPaused(true);
        }
    }
}
