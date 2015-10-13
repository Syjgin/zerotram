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
            GameController.GetInstance().UndragAll();
            return;
        }
        Vector2 targetPos = MonobehaviorHandler.GetMonobeharior().GetObject<Floor>("Floor").GetCurrentMousePosition();
        float currentDist = (targetPos - _conductor.GetDragStartPoint()).sqrMagnitude;
        if (currentDist > _conductor.GetMaxDragDistance())
        {
            GameController.GetInstance().UndragAll();
            return;
        }
        MovableCharacter.Animator.Play("drag");
        Vector3 newPos = targetPos + _conductor.GetDragOffset();
        MovableCharacter.CalculateOrientation(newPos);
        MovableCharacter.transform.position = newPos;
    }
}
