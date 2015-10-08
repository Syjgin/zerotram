using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ConductorSM : MovableCharacterSM
{
    public bool IsInWayoutZone;

    private PassengerSM _dragTarget;
    private Vector2 _dragStartPoint;
    private Vector2 _dragOffset;
    private float _maxDragDistance;
    private Text _lifes;

    void Awake()
    {
        _maxDragDistance = ConfigReader.GetConfig().GetField("hero").GetField("MaxDragDistance").n;
        Hp = InitialLifes = ConfigReader.GetConfig().GetField("hero").GetField("InitialLifes").n;
        AttackDistance = ConfigReader.GetConfig().GetField("hero").GetField("AttackDistance").n;
        AttackReloadPeriod = ConfigReader.GetConfig().GetField("hero").GetField("AttackReloadPeriod").n;
        Velocity = ConfigReader.GetConfig().GetField("hero").GetField("Velocity").n;
        AttackReactionPeriod = ConfigReader.GetConfig().GetField("hero").GetField("AttackReactionPeriod").n;
        AttackReloadPeriod = ConfigReader.GetConfig().GetField("hero").GetField("AttackReloadPeriod").n;
        _lifes = GameObject.Find("userLifes").GetComponent<Text>();
        _lifes.text = "100%";

        IdleState idleState = new IdleState(this);
        MoveState moveState = new MoveState(this);
        ConductorDragState dragState = new ConductorDragState(this);
        AttackState attackState = new AttackState(this);
        AttackedState attackedState = new AttackedState(this);
        Dictionary<int, State> stateMap = new Dictionary<int, State>
        {
            {(int) MovableCharacterStates.Idle, idleState},
            {(int) MovableCharacterStates.Move, moveState},
            {(int) MovableCharacterStates.Drag, dragState},
            {(int) MovableCharacterStates.Attack, attackState},
            {(int) MovableCharacterStates.Attacked, attackedState}
        };
        InitWithStates(stateMap, (int)MovableCharacterStates.Idle);
    }

    public void StartDrag(PassengerSM obj)
    {
        if (obj.IsStick())
        {
            obj.StopStick();
        }
        _dragTarget = obj;
        _dragStartPoint = FloorHandler.GetFloor().GetCurrentMousePosition();
        _dragOffset = transform.position - obj.transform.position;
        CalculateOrientation(_dragStartPoint);//maybe inverted obj position will be used
        ActivateState((int)MovableCharacterStates.Drag);
        obj.SetDragged(true);
    }

    public void Kick(PassengerSM obj)
    {
        AudioController.GetPlayer().PlayAudioById("kick");
        CalculateOrientation(obj.transform.position);
        ActivateState((int)MovableCharacterStates.Attack);
        if (obj.IsStick())
        {
            obj.StopStick();
            obj.WasStickWhenFlyAway = true;
        }
        obj.FlyAway();
        AttackTarget = null;
        _dragTarget = null;
    }

    public void StopDrag()
    {
        if (GetActiveState().Equals((int)MovableCharacterStates.Drag))
        {
            MakeIdle();
            if (_dragTarget != null)
            {
                _dragTarget.SetDragged(false);
                _dragTarget = null;
            }
        }
    }

    public bool IsDragged()
    {
        return GetActiveState().Equals((int) MovableCharacterStates.Drag);
    }

    public PassengerSM GetDragTarget()
    {
        return _dragTarget;
    }

    public Vector2 GetDragStartPoint()
    {
        return _dragStartPoint;
    }

    public Vector2 GetDragOffset()
    {
        return _dragOffset;
    }

    public float GetMaxDragDistance()
    {
        return _maxDragDistance;
    }

    public override void AddDamage(MovableCharacterSM attacker)
    {
        base.AddDamage(attacker);
        int lifesPercent = Mathf.RoundToInt(100 * (Hp / (float)InitialLifes));
        _lifes.text = lifesPercent + "%";
    }

    public bool IsDragging()
    {
        return GetActiveState().Equals((int) MovableCharacterStates.Drag);
    }

    public void HandleClick()
    {
        FloorHandler.GetFloor().OnMouseDown();
    }
}
