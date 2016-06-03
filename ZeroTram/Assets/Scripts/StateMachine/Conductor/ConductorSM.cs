using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ConductorSM : MovableCharacterSM
{
    public static int _hp { get; set; }
    public bool IsInWayoutZone;

    private PassengerSM _dragTarget;
    private Vector2 _dragStartPoint;
    private Text _lifes;
    public float MovingToDragTargetVelocity;
    private PassengerSM _stickTarget;
    private bool _isTrainingKickHandlerActivated;

    void Awake()
    {
        Hp = InitialLifes = ConfigReader.GetConfig().GetField("hero").GetField("InitialLifes").n;
        AttackDistance = ConfigReader.GetConfig().GetField("hero").GetField("AttackDistance").n;
        AttackReloadPeriod = ConfigReader.GetConfig().GetField("hero").GetField("AttackReloadPeriod").n;
        Velocity = ConfigReader.GetConfig().GetField("hero").GetField("Velocity").n;
        AttackReactionPeriod = ConfigReader.GetConfig().GetField("hero").GetField("AttackReactionPeriod").n;
        AttackReloadPeriod = ConfigReader.GetConfig().GetField("hero").GetField("AttackReloadPeriod").n;
        MovingToDragTargetVelocity = ConfigReader.GetConfig().GetField("hero").GetField("MovingToDragTargetVelocity").n;
        _lifes = GameObject.Find("userLifes").GetComponent<Text>();
        _lifes.text = "100%";
        _hp = 100;

        IdleState idleState = new IdleState(this);
        ConductorMoveState moveState = new ConductorMoveState(this);
        ConductorDragState dragState = new ConductorDragState(this);
        AttackState attackState = new AttackState(this);
        AttackedState attackedState = new AttackedState(this);
        FrozenState frozenState = new FrozenState(this);
        Dictionary<int, State> stateMap = new Dictionary<int, State>
        {
            {(int) MovableCharacterStates.Idle, idleState},
            {(int) MovableCharacterStates.Move, moveState},
            {(int) MovableCharacterStates.Drag, dragState},
            {(int) MovableCharacterStates.Attack, attackState},
            {(int) MovableCharacterStates.Attacked, attackedState},
            {(int) MovableCharacterStates.Frozen, frozenState}
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
            _dragTarget.StopDrag(false);
        if (obj.IsStick())
        {
            obj.StopStick();
        }
        if(obj.IsDragDenied())
            return;
        _dragTarget = obj;
        _dragStartPoint = MonobehaviorHandler.GetMonobeharior().GetObject<Floor>("Floor").GetCurrentMousePosition();
        CalculateOrientation(_dragStartPoint);
        ActivateState((int)MovableCharacterStates.Drag);
        obj.StartDrag();
    }

    public bool CanKick(PassengerSM obj)
    {
        if (IsInWayoutZone && IsInAttackRadius(obj.transform.position) && !obj.IsStick())
            return true;
        return false;
    }

    public void Kick(PassengerSM obj)
    {
        if (obj.IsFrozen())
        {
            obj.TemporalyUnfreeze();
        }
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

    public void StopDrag(bool attack)
    {
        if (GetActiveState().Equals((int)MovableCharacterStates.Drag))
        {
            MakeIdle();
            if (_dragTarget != null)
            {
                _dragTarget.StopDrag(attack);
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

    public override void AddDamageValue(float damage)
    {
        base.AddDamageValue(damage);
        int lifesPercent = Mathf.RoundToInt(100 * (Hp / (float)InitialLifes));
        _lifes.text = lifesPercent + "%";
        _hp = lifesPercent;
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
