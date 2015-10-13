using UnityEngine;
public class PassengerDraggedState : MovableCharacterState
{
    public override void OnUpdate()
    {
        MovableCharacter.Animator.Play("attacked");
        Vector2 targetPos = MonobehaviorHandler.GetMonobeharior().GetObject<Floor>("Floor").GetCurrentMousePosition();
        MovableCharacter.transform.position = new Vector3(targetPos.x, targetPos.y, -1);
    }

    public PassengerDraggedState(StateMachine parent) : base(parent)
    {
    }

    public override bool IsTransitionAllowed()
    {
        return !MonobehaviorHandler.GetMonobeharior().GetObject<Floor>("Floor").GetHero().IsDragging();
    }
}
