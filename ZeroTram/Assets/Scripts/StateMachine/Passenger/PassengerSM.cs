using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets;
using Assets.Scripts.Math;
using UnityEngine.Networking.Match;

public class PassengerSM : MovableCharacterSM
{
    private int _tramStopCount;
    private int _currentTramStopCount;
    public float MoveProbability = 50;
    public float AttackProbability = 50;
    public float ChangeStatePeriod = 10;
    protected float CounterAttackProbability = 50;
    protected float StickProbability = 0;
    protected float TicketProbability;
    private bool _hasTicket;
    private float _maxStopCount;

    protected NewCharacterWindow Window;

    [SerializeField]
    private Sprite _question;
    [SerializeField]
    private Sprite _ticket;
    [SerializeField]
    private Sprite _hare;
    [SerializeField]
    private Sprite _stick;
    [SerializeField]
    protected SpriteRenderer Indicator;

    void Awake()
    {
        PassengerIdleState idleState = new PassengerIdleState(this);
        PassengerMoveState moveState = new PassengerMoveState(this);
        PassengerAttackState attackState = new PassengerAttackState(this);
        PassengerAttackedState attackedState = new PassengerAttackedState(this);
        PassengerStickState stickState = new PassengerStickState(this);
        PassengerFlyingAwayState flyingAwayState = new PassengerFlyingAwayState(this);
        PassengerDraggedState draggedState = new PassengerDraggedState(this);
        Dictionary<int, State> stateMap = new Dictionary<int, State>
        {
            {(int) MovableCharacterStates.Idle, idleState},
            {(int) MovableCharacterStates.Move, moveState},
            {(int) MovableCharacterStates.Attack, attackState},
            {(int) MovableCharacterStates.Attacked, attackedState},
            {(int) MovableCharacterStates.Stick, stickState},
            {(int) MovableCharacterStates.FlyingAway, flyingAwayState},
            {(int) MovableCharacterStates.Dragged, draggedState},
        };
        InitWithStates(stateMap, (int)MovableCharacterStates.Idle);
    }

    public virtual void Init()
    {
        _hasTicket = Randomizer.GetPercentageBasedBoolean((int)TicketProbability);
        CalculateStick();
        _maxStopCount = ConfigReader.GetConfig().GetField("tram").GetField("MaxStopCount").n;
        int stopCount = Randomizer.GetInRange(1, (int)_maxStopCount);
        _tramStopCount = stopCount;
        GameController.GetInstance().RegisterPassenger(this);
        Window = NewCharacterWindowHandler.GetWindow();
    }

    public float AttackTargetDistance()
    {
        if (AttackTarget == null)
            return float.MaxValue;
        return ((Vector2)transform.position - (Vector2)AttackTarget.transform.position).sqrMagnitude;
    }
    
    public bool HasTicket()
    {
        return _hasTicket;
    }

    public bool IsStick()
    {
        return GetActiveState().Equals((int) MovableCharacterStates.Stick);
    }

    public bool WasStickWhenFlyAway { get; set; }

    private bool _isVisibleToHero;

    public bool IsVisibleToHero()
    {
        return _isVisibleToHero;
    }

    public void SetDragged(bool dragged)
    {
        if (dragged)
        {
            ActivateState((int) MovableCharacterStates.Dragged);
        }
        else
        {
            if (GetActiveState().Equals((int) MovableCharacterStates.Dragged))
            {
                MakeIdle();
            }
        }
    }

    public void IncrementStationCount()
    {
        _currentTramStopCount++;
        if (_currentTramStopCount > _tramStopCount)
        {
            GameObject leftDoor = GameObject.Find("door1");
            GameObject rightDoor = GameObject.Find("door2");
            Vector2 target;
            if (Randomizer.GetRandomPercent() > 50)
                target = leftDoor.transform.position;
            else
                target = rightDoor.transform.position;
            Velocity *= 2;
            SetTarget(target);
            IsGoingAway = true;
        }
    }
    
    public void CalculateStick()
    {
        if(GameController.GetInstance().IsAnybodyStick())
            return;
        bool stick = Randomizer.GetPercentageBasedBoolean((int)StickProbability);
        if (stick && FloorHandler.GetFloor().IsPassengerNearDoors(this))
        {
            ActivateState((int)MovableCharacterStates.Stick);
            Indicator.sprite = _stick;
        }
    }

    public override bool CanNotInteract()
    {
        int activeStateCode = GetActiveState();
        return (activeStateCode == (int) MovableCharacterStates.FlyingAway || IsGoingAway ||
                IsStick() || activeStateCode == (int) MovableCharacterStates.Dragged);
    }

    public void HandleTriggerEnter(Collider2D other)
    {
        if (CanNotInteract())
            return;
        MovableCharacterSM movable = other.gameObject.GetComponentInParent<MovableCharacterSM>();
        if (movable != null)
        {
            TryAttackMovable(movable);
        }
    }

    public void TryAttackMovable(MovableCharacterSM movable)
    {
        if (Randomizer.GetPercentageBasedBoolean((int)AttackProbability))
        {
            AttackTarget = movable;
            ActivateState((int)MovableCharacterStates.Attack);
        }
        else
        {
            MakeIdle();
        }
    }

    public void CalculateRandomTarget()
    {
        Vector2 target = FloorHandler.GetFloor().GetRandomPosition();
        if (target != default(Vector2))
            SetTarget(target);
    }

    public void CalculateAttackReaction()
    {
        bool willCounterAttack = Randomizer.GetPercentageBasedBoolean((int)CounterAttackProbability);
        if (willCounterAttack)
        {
            if (AttackTarget != null)
                ActivateState((int)MovableCharacterStates.Attack);
            else
                MakeIdle();
        }
        else
        {
            AttackTarget = null;
            CalculateRandomTarget();
        }
    }

    public void StopStick()
    {
        MakeIdle();
        if (!IsGoingAway)
            CalculateRandomTarget();
        DoorsHandler.GetTimer().SetPaused(false);
    }

    protected virtual void ShowCharacterInfo()
    {

    }

    public void FlyAway()
    {
        ActivateState((int)MovableCharacterStates.FlyingAway);
        AttackTarget = null;
        GameController.GetInstance().RegisterDeath(this);
    }

    public void HandleClick()
    {
        ConductorSM hero = FloorHandler.GetFloor().GetHero();
        if (hero.IsInAttackRadius(transform.position))
        {
            if (!_isVisibleToHero)
            {
                _isVisibleToHero = true;
                GameController.GetInstance().UpdatePassenger(this);
                if (!_hasTicket)
                {
                    AttackTarget = hero;
                    CalculateAttackReaction();
                }
                else
                {
                    AudioController.GetPlayer().PlayAudioById("coins");
                }
                StopStick();
                ShowCharacterInfo();
                return;
            }
            if (hero.IsInWayoutZone)
            {
                if (!_hasTicket)
                {
                    hero.Kick(this);
                    return;
                }
                if (AttackTarget != null && AttackStrength > 0)
                {
                    hero.Kick(this);
                    return;
                }
                if (IsStick())
                {
                    if (IsGoingAway)
                        hero.Kick(this);
                    else
                        hero.StartDrag(this);
                    return;
                }
            }
            hero.StartDrag(this);
            ActivateState((int)MovableCharacterStates.Dragged);
        }
        else
        {
            hero.SetTarget(transform.position);
        }
    }

    private void CalculateIndicator()
    {
        if (IsStick())
        {
            Indicator.sprite = _stick;
            return;
        }
        if (!_isVisibleToHero)
        {
            Indicator.sprite = _question;
            return;
        }
        if (_hasTicket)
            Indicator.sprite = _ticket;
        else
            Indicator.sprite = _hare;
    }

    public void StopDrag()
    {
        FloorHandler.GetFloor().OnMouseUp();
    }

    protected override void Update()
    {
        base.Update();

        if (FloorHandler.GetFloor().GetHero() == null)
            return;
        if (IsGoingAway && 
            GameController.GetInstance().IsDoorsOpen() && 
            !IsStick() && 
            FloorHandler.GetFloor().IsPassengerNearDoors(this))
        {
            Destroy(gameObject);
            return;
        }
        if (AttackTarget != null)
        {
            if (CanNotInteract() || AttackTarget.CanNotInteract())
            {
                AttackTarget = null;
                MakeIdle();
            }
        }
        /*if (GetPosition().z > -1)
        {
            Vector3 correctPos = GetPosition();
            correctPos.z = -1;
            SetPosition(correctPos);
        }*/
        CalculateIndicator();
        
    }
}
