using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using Assets;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ConductorSM : MovableCharacterSM
{
    public bool IsInWayoutZone;

    private PassengerSM _dragTarget;
    private Vector2 _dragStartPoint;
    private float _maxDragDistance;
    private Text _lifes;
    public float MovingToDragTargetVelocity;
    private PassengerSM _stickTarget;

    void Awake()
    {
        _maxDragDistance = ConfigReader.GetConfig().GetField("hero").GetField("MaxDragDistance").n;
        Hp = InitialLifes = ConfigReader.GetConfig().GetField("hero").GetField("InitialLifes").n;
        AttackDistance = ConfigReader.GetConfig().GetField("hero").GetField("AttackDistance").n;
        AttackReloadPeriod = ConfigReader.GetConfig().GetField("hero").GetField("AttackReloadPeriod").n;
        Velocity = ConfigReader.GetConfig().GetField("hero").GetField("Velocity").n;
        AttackReactionPeriod = ConfigReader.GetConfig().GetField("hero").GetField("AttackReactionPeriod").n;
        AttackReloadPeriod = ConfigReader.GetConfig().GetField("hero").GetField("AttackReloadPeriod").n;
        MovingToDragTargetVelocity = ConfigReader.GetConfig().GetField("hero").GetField("MovingToDragTargetVelocity").n;
        _lifes = GameObject.Find("userLifes").GetComponent<Text>();
        _lifes.text = "100%";

        IdleState idleState = new IdleState(this);
        ConductorMoveState moveState = new ConductorMoveState(this);
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

    public void StartSaveStickPassenger(PassengerSM passenger)
    {
        _stickTarget = passenger;
    }

    public void StopStickIfNeeded()
    {
        if (_stickTarget != null)
        {
            _stickTarget.StopStick();
            _stickTarget = null;
        }
    }

    public void StartDrag(PassengerSM obj)
    {
        if(_dragTarget != null)
            _dragTarget.StopDrag();
        if (obj.IsStick())
        {
            obj.StopStick();
        }
        _dragTarget = obj;
        _dragStartPoint = MonobehaviorHandler.GetMonobeharior().GetObject<Floor>("Floor").GetCurrentMousePosition();
        CalculateOrientation(_dragStartPoint);
        ActivateState((int)MovableCharacterStates.Drag);
        obj.StartDrag();
    }

    public bool CanKick(PassengerSM obj)
    {
        if (IsInWayoutZone && IsInAttackRadius(obj.transform.position))
            return true;
        return false;
    }

    public void Kick(PassengerSM obj)
    {
        MonobehaviorHandler.GetMonobeharior().GetObject<AudioPlayer>("AudioPlayer").PlayAudioById("kick");
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
                _dragTarget.StopDrag();
            }
            _dragTarget = null;
        }
    }

    public bool IsDragged()
    {
        return GetActiveState().Equals((int) MovableCharacterStates.Drag);
    }

    public override bool CanNotInteract()
    {
        return IsDragging();
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
        return ((Vector2) transform.position - (Vector2) _dragTarget.transform.position);
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

    public override void HandleClick()
    {
        MonobehaviorHandler.GetMonobeharior().GetObject<Floor>("Floor").OnMouseDown();
    }

    public override void HandleDoubleClick()
    {
        if (Time.timeScale == 0)
            return;
        MonobehaviorHandler.GetMonobeharior().GetObject<Floor>("Floor").DoubleClick();
    }
}
