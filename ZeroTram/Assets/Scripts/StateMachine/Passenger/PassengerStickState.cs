using UnityEngine;
public class PassengerStickState : MovableCharacterState
{
    private PassengerSM _passenger;

    protected override void OnStart()
    {
        MovableCharacter.Animator.Play("idle");
    }

    public override void OnUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 transform2d = MovableCharacter.transform.position;
            float distance = (transform2d - hit.centroid).sqrMagnitude;
            if (distance < 1 && MonobehaviorHandler.GetMonobeharior().GetObject<Floor>("Floor").GetHero().IsInAttackRadius(MovableCharacter.transform.position))
            {
                _passenger.HandleClick();
            }
        }
    }

    public PassengerStickState(StateMachine parent) : base(parent)
    {
        _passenger = (PassengerSM) parent;
    }
}
