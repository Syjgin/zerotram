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
    public float AttackProbability = 50;
    public float ChangeStatePeriod = 10;
    public float DragChangeStatePeriod = 10;
    protected float CounterAttackProbability = 50;
    protected float StickProbability = 0;
    protected float TicketProbability;
    private bool _hasTicket;
    private float _maxStopCount;
    private bool _isMagnetTurnedOn;
    private float _magnetDistance;

    private float _savedStickProbability;

    public List<GameController.BonusTypes> ActiveBonuses; 

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
        ActiveBonuses = new List<GameController.BonusTypes>();
        PassengerIdleState idleState = new PassengerIdleState(this);
        PassengerMoveState moveState = new PassengerMoveState(this);
        PassengerAttackState attackState = new PassengerAttackState(this);
        PassengerAttackedState attackedState = new PassengerAttackedState(this);
        PassengerStickState stickState = new PassengerStickState(this);
        PassengerFlyingAwayState flyingAwayState = new PassengerFlyingAwayState(this);
        PassengerDraggedState draggedState = new PassengerDraggedState(this);
        FrozenState frozenState = new FrozenState(this);
        Dictionary<int, State> stateMap = new Dictionary<int, State>
        {
            {(int) MovableCharacterStates.Idle, idleState},
            {(int) MovableCharacterStates.Move, moveState},
            {(int) MovableCharacterStates.Attack, attackState},
            {(int) MovableCharacterStates.Attacked, attackedState},
            {(int) MovableCharacterStates.Stick, stickState},
            {(int) MovableCharacterStates.FlyingAway, flyingAwayState},
            {(int) MovableCharacterStates.Dragged, draggedState},
            {(int) MovableCharacterStates.Frozen, frozenState},
        };
        InitWithStates(stateMap, (int)MovableCharacterStates.Idle);
    }

    public void RecalculateTicketProbability(float coef, bool onlyForInvisible)
    {
        TicketProbability *= coef;
        if(onlyForInvisible && _isVisibleToHero)
            return;
        _hasTicket = Randomizer.GetPercentageBasedBoolean((int)TicketProbability);
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

    public void TurnOnMagnet(float dist)
    {
        _isMagnetTurnedOn = true;
        _magnetDistance = dist;
    }

    public void TurnOffMagnet()
    {
        _isMagnetTurnedOn = false;
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

    public void StartDrag()
    {
        if (IsFrozen())
        {
            TemporalyUnfreeze();
        }
        ActivateState((int)MovableCharacterStates.Dragged);
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
        if (stick && MonobehaviorHandler.GetMonobeharior().GetObject<Floor>("Floor").IsPassengerNearDoors(this))
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
        if(AttackTarget != null)
            return;
        Vector2 target = MonobehaviorHandler.GetMonobeharior().GetObject<Floor>("Floor").GetRandomPosition();
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
        MonobehaviorHandler.GetMonobeharior().GetObject<DoorsTimer>("Spawner").SetPaused(false);
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

    public void StartUnstick(ConductorSM hero)
    {
        TimeSincePreviousClickMade = MaxClickDuration;
        hero.SetTarget(transform.position);
        hero.StartSaveStickPassenger(this);
    }

    public override void HandleClick()
    {
        ConductorSM hero = MonobehaviorHandler.GetMonobeharior().GetObject<Floor>("Floor").GetHero();
        if (!_isVisibleToHero)
        {
            if (IsStick())
            {
                StartUnstick(hero);
            }
            else
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
                    MonobehaviorHandler.GetMonobeharior().GetObject<AudioPlayer>("AudioPlayer").PlayAudioById("coins");
                }
                ShowCharacterInfo();
            }
            return;
        }
        if (IsStick())
        {
            StartUnstick(hero);
            return;
        }
        hero.StartDrag(this);
    }

    public override void HandleDoubleClick()
    {
        ConductorSM hero = MonobehaviorHandler.GetMonobeharior().GetObject<Floor>("Floor").GetHero();
        if (hero.CanKick(this))
        {
            hero.Kick(this);
            return;
        }
        hero.StartDrag(this);
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
        if(GetActiveState() != (int)MovableCharacterStates.Dragged)
            return;
        MonobehaviorHandler.GetMonobeharior().GetObject<Floor>("Floor").OnMouseUp();
        ConductorSM conductor = MonobehaviorHandler.GetMonobeharior().GetObject<Floor>("Floor").GetHero();
        MakeIdle();
        if (conductor.CanKick(this))
        {
            CalculateRandomTarget();
        }
    }

    public void CalculateMagnet()
    {
        ConductorSM hero = MonobehaviorHandler.GetMonobeharior().GetObject<Floor>("Floor").GetHero();
        Vector2 heroPosition = hero.transform.position;
        float heroDist = ((Vector2)transform.position - heroPosition).sqrMagnitude;
        if (heroDist < _magnetDistance)
        {
            if (heroDist < 0.1f)
                return;
            if (!GetTarget().Equals(heroPosition))
                SetTarget(hero.transform.position);
        }
    }

    public bool IsMagnetTurnedOn()
    {
        return _isMagnetTurnedOn;
    }

    public void AddVortexEffect(Vector2 point, float dist)
    {
        float randomAngleInDegrees = Randomizer.GetInRange(0, 360);
        float radians = randomAngleInDegrees*Mathf.Deg2Rad;
        float finalDist = Mathf.Min(dist*0.5f, Randomizer.GetNormalizedRandom() * dist);
        float xOffset = finalDist * Mathf.Cos(radians);
        float yOffset = finalDist * Mathf.Sin(radians);
        Vector3 oldPos = transform.position;
        Vector3 newPos = new Vector3(oldPos.x + xOffset, oldPos.y + yOffset, oldPos.z);
        MonobehaviorHandler.GetMonobeharior().GetObject<Floor>("Floor").NormalizePosition(ref newPos);
        StopStick();
        MakeIdle();
        transform.position = newPos;
    }

    protected override void Update()
    {
        base.Update();
        ConductorSM hero = MonobehaviorHandler.GetMonobeharior().GetObject<Floor>("Floor").GetHero();
        if (hero == null)
            return;
        if (IsGoingAway && 
            GameController.GetInstance().IsDoorsOpen() && 
            !IsStick() &&
            MonobehaviorHandler.GetMonobeharior().GetObject<Floor>("Floor").IsPassengerNearDoors(this))
        {
            Destroy(gameObject);
            return;
        }
        CalculateIndicator();
        if(!hero.IsDragging())
            StopDrag();
    }

    public void ApplyWrenchBonus(bool add)
    {
        if(!_hasTicket)
            return;
        if (add)
        {
            _savedStickProbability = StickProbability;
            StickProbability = 0;
            MakeIdle();
        }
        else
        {
            StickProbability = _savedStickProbability;
        }
    }
}
