using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets;
using UnityEngine;

public class ConductorDragState : MovableCharacterState
{
    private ConductorSM _conductor;

    public ConductorDragState(StateMachine parent) : base(parent)
    {
        _conductor = (ConductorSM) parent;
    }
    
    public override void OnUpdate()
    {
        if (_conductor.GetDragTarget() == null || !Input.GetMouseButton(0))
        {
            _conductor.StopDrag();
            return;
        }
        Vector2 targetPos = MonobehaviorHandler.GetMonobeharior().GetObject<Floor>("Floor").GetCurrentMousePosition();
        float currentDist = (targetPos - _conductor.GetDragStartPoint()).sqrMagnitude;
        if (currentDist > _conductor.GetMaxDragDistance())
        {
            _conductor.StopDrag();
            return;
        }
        Vector2 dragOffset = _conductor.GetDragOffset();
        if (dragOffset.sqrMagnitude > 0.1f)
        {
            MovableCharacter.Animator.Play("walk");
            Vector3 newPosition = Vector3.MoveTowards(MovableCharacter.transform.position, _conductor.GetDragTarget().transform.position,
                _conductor.MovingToDragTargetVelocity*Time.deltaTime);
            newPosition.z = -1;
            MovableCharacter.transform.position = newPosition;
        }
        else
        {
            MovableCharacter.Animator.Play("drag");
            Vector3 newPos = targetPos + _conductor.GetDragOffset();
            MovableCharacter.CalculateOrientation(newPos);
            MovableCharacter.transform.position = newPos;
        }
    }
}
