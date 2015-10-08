using UnityEngine;
public class PassengerFlyingAwayState : MovableCharacterState
{
    private Vector3 _flyTarget;
    private const int FlyLength = 30;

    protected override void OnStart()
    {
        Vector3 pos = MovableCharacter.transform.position;
        _flyTarget = new Vector2(pos.x, pos.y + FlyLength);
    }

    public override void OnUpdate()
    {
        MovableCharacter.Animator.Play("attacked");
        Vector3 newPosition = Vector3.MoveTowards(MovableCharacter.transform.position, _flyTarget, 50 * Time.deltaTime);
        MovableCharacter.transform.position = newPosition;
        float sqrRemainingDistance = ((Vector2)newPosition - (Vector2)_flyTarget).sqrMagnitude;
        if (sqrRemainingDistance <= 1)
        {
            MonoBehaviour.Destroy(MovableCharacter.gameObject);
        }
    }

    public PassengerFlyingAwayState(StateMachine parent) : base(parent)
    {
    }

    public override bool IsTransitionAllowed()
    {
        return false;
    }
}
