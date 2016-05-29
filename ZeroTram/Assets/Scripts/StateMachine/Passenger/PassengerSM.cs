using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking.NetworkSystem;

public class PassengerSM : MovableCharacterSM
{
    public bool IsNearBench;

    private int _tramStopCount;
    private int _currentTramStopCount;
    public float AttackProbability = 50;
    public float ChangeStatePeriod = 10;
    public float DragChangeStatePeriod = 10;
    protected float CounterAttackProbability = 50;
    protected float StickProbability = 0;
    protected float TicketProbability;
    public float BonusProbability = 0;
    private bool _hasTicket;
    private float _maxStopCount;
    private bool _isMagnetTurnedOn;
    private float _magnetDistance;
    private float _attackingDenyPeriod;
    private bool _isMagnetActivated;

    public bool IsAttackingAllowed;
    private Bench _currentBench;

    private bool _isFlyingAwayListenerActivated;

    public Dictionary<GameController.BonusTypes, float> BonusProbabilities; 

    private float _savedStickProbability;

    public List<GameController.BonusTypes> ActiveBonuses;

    private bool _isDragRunawayDeniedByTraining;

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
    
    private bool _isTrainingEnabled;

    private bool _attackDenyedByTraining;
    private bool _isStickModifiedForTraining;

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
        PassengerSitState sitState = new PassengerSitState(this);
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
            {(int) MovableCharacterStates.Sit, sitState},
        };
        InitWithStates(stateMap, (int)MovableCharacterStates.Idle);
    }

    public void SetAttackEnabled(bool isEnabled)
    {
        _attackDenyedByTraining = !isEnabled;
    }
    
    public void AttackIfPossible()
    {
        if (IsAttackingAllowed && !_attackDenyedByTraining)
            ActivateState((int)MovableCharacterSM.MovableCharacterStates.Attack);
    }

    public void SetRunawayDenied(bool denied)
    {
        _isDragRunawayDeniedByTraining = denied;
    }

    public bool IsRunawayDenied()
    {
        return _isDragRunawayDeniedByTraining;
    }

    public void ActivateFlyAwayListener()
    {
        _isFlyingAwayListenerActivated = true;
    }

    public bool IsFlyAwayListenerActivated()
    {
        return _isFlyingAwayListenerActivated;
    }

    public void RecalculateTicketProbability(float coef, bool onlyForInvisible)
    {
        TicketProbability *= coef;
        if(onlyForInvisible && _isVisibleToHero)
            return;
        _hasTicket = Randomizer.GetPercentageBasedBoolean((int)TicketProbability);
    }

    public void EnableTrainingClick()
    {
        _isTrainingEnabled = true;
    }

    public virtual void Init(bool register, bool unstickable = false)
    {
        AttackProbability = ConfigReader.GetConfig().GetField(GetClassName()).GetField("AttackProbability").n;
        DragChangeStatePeriod = ConfigReader.GetConfig().GetField(GetClassName()).GetField("DragChangeStatePeriod").n;
        ChangeStatePeriod = ConfigReader.GetConfig().GetField(GetClassName()).GetField("ChangeStatePeriod").n;
        AttackDistance = ConfigReader.GetConfig().GetField(GetClassName()).GetField("AttackDistance").n;
        AttackReloadPeriod = ConfigReader.GetConfig().GetField(GetClassName()).GetField("AttackReloadPeriod").n;
        AttackMaxDistance = ConfigReader.GetConfig().GetField(GetClassName()).GetField("AttackMaxDistance").n;
        CounterAttackProbability = ConfigReader.GetConfig().GetField(GetClassName()).GetField("CounterAttackProbability").n;
        Hp = InitialLifes = ConfigReader.GetConfig().GetField(GetClassName()).GetField("InitialLifes").n;
        Velocity = ConfigReader.GetConfig().GetField(GetClassName()).GetField("Velocity").n;
        AttackStrength = ConfigReader.GetConfig().GetField(GetClassName()).GetField("AttackStrength").n;
        AttackReactionPeriod = ConfigReader.GetConfig().GetField(GetClassName()).GetField("AttackReactionPeriod").n;
        TicketProbability = ConfigReader.GetConfig().GetField(GetClassName()).GetField("TicketProbability").n;
        StickProbability = ConfigReader.GetConfig().GetField(GetClassName()).GetField("StickProbability").n;
        BonusProbability = ConfigReader.GetConfig().GetField(GetClassName()).GetField("BonusProbability").n;
        _attackingDenyPeriod = ConfigReader.GetConfig().GetField("tram").GetField("AttackDenyPeriod").n;
        ParseBonusMap();
        _hasTicket = Randomizer.GetPercentageBasedBoolean((int)TicketProbability);
        if(!unstickable)
            CalculateStick();
        _maxStopCount = ConfigReader.GetConfig().GetField("tram").GetField("MaxStopCount").n;
        int stopCount = Randomizer.GetInRange(1, (int)_maxStopCount);
        _tramStopCount = stopCount;
        if(register)
            GameController.GetInstance().RegisterPassenger(this);
    }

    public float AttackTargetDistance()
    {
        if (AttackTarget == null)
            return float.MaxValue;
        Vector2 position2D = transform.position;
        Vector2 attackTargetPosition2D = AttackTarget.BoxCollider2D.bounds.ClosestPoint(transform.position);
        return (position2D - attackTargetPosition2D).sqrMagnitude;
    }

    public bool HasTicket()
    {
        return _hasTicket;
    }

    public void SetTicketAndVisibility(bool hasTicket, bool isVisible)
    {
        _hasTicket = hasTicket;
        _isVisibleToHero = isVisible;
    }

    public void TurnOnMagnet(float dist)
    {
        if(_hasTicket && _isVisibleToHero)
            return;
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

    public void StartGoAway()
    {
        _currentTramStopCount = int.MaxValue - 1;
    }

    public void SetAlwaysStickForTraining()
    {
        StickProbability = 100;
        _isStickModifiedForTraining = true;
    }

    public void IncrementStationCount()
    {
        _currentTramStopCount++;
        if (_currentTramStopCount > _tramStopCount && !IsGoingAway)
        {
            DoorsAnimationController door1 =
                    MonobehaviorHandler.GetMonobeharior().GetObject<DoorsAnimationController>("door1");
            DoorsAnimationController door2 =
                MonobehaviorHandler.GetMonobeharior().GetObject<DoorsAnimationController>("door2");
            DoorsAnimationController door3 =
                MonobehaviorHandler.GetMonobeharior().GetObject<DoorsAnimationController>("door3");
            DoorsAnimationController door4 =
                MonobehaviorHandler.GetMonobeharior().GetObject<DoorsAnimationController>("door4");
            List<DoorsAnimationController> selected = new List<DoorsAnimationController>();
            if (door1.IsOpened())
                selected.Add(door1);
            if (door2.IsOpened())
                selected.Add(door2);
            if (door3.IsOpened())
                selected.Add(door3);
            if (door4.IsOpened())
                selected.Add(door4);
            if (selected.Count == 0)
                return;
            int randomPercent = Randomizer.GetRandomPercent();
            int step = 100 / selected.Count;
            int currentStep = 0;
            int i = 0;
            for (i = 0; i < selected.Count - 1; i++)
            {
                if (currentStep > randomPercent)
                {
                    break;
                }
                currentStep += step;
            }
            BoxCollider2D collider = selected[i].GetComponent<BoxCollider2D>();
            Vector2 target = new Vector2(selected[i].gameObject.transform.position.x, selected[i].gameObject.transform.position.y) + collider.offset;
            Velocity *= 2;
            IsGoingAway = true;
            base.SetTarget(target);
        }
    }
    
    public void CalculateStick()
    {
        if(GameController.GetInstance().IsAnybodyStick())
            return;
        bool stick = Randomizer.GetPercentageBasedBoolean((int)StickProbability);
        if (stick && MonobehaviorHandler.GetMonobeharior().GetObject<Floor>("Floor").IsPassengerNearDoors(this))
        {
            if (_isStickModifiedForTraining)
            {
                TrainingHandler handler =
                    MonobehaviorHandler.GetMonobeharior().GetObject<TrainingHandler>("TrainingHandler");
                handler.ShowNext();
            }
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

    //will be replaced with current skin config

    public int GetStandPossibility()
    {
        return (int)ConfigReader.GetConfig().GetField("tram").GetField("StandPossibility").n;
    }

    public float GetStopStandPeriod()
    {
        return ConfigReader.GetConfig().GetField("tram").GetField("StopStandCheckPeriod").n; 
    }

    public void HandleStandUp()
    {
        if (_currentBench != null)
        {
            _currentBench = null;
            CalculateRandomTarget();
        }
    }

    public bool IsOnTheBench()
    {
        return GetActiveState() == (int)MovableCharacterStates.Sit;
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

    public void HandleSitdown(Bench bench)
    {
        _currentBench = bench;
        AttackTarget = null;
        transform.position = new Vector3(bench.transform.position.x, bench.transform.position.y, transform.position.z);
        ActivateState((int) MovableCharacterStates.Sit);
    }

    public void TryAttackMovable(MovableCharacterSM movable)
    {
        if(_attackDenyedByTraining)
            return;
        float currentAttackProbability = AttackProbability;
        if (movable is PassengerSM)
        {
            PassengerSM passenger = (PassengerSM) movable;
            if (passenger.IsOnTheBench())
            {
                currentAttackProbability *=
                     ConfigReader.GetConfig().GetField("tram").GetField("SitAggressionIncrement").n;
            }
        }
        if (Randomizer.GetPercentageBasedBoolean((int)currentAttackProbability))
        {
            AttackTarget = movable;
            AttackIfPossible();
        }
        else
        {
            MakeIdle();
        }
    }


    public override void MakeIdle()
    {
        if(IsAttackingAllowed)
            base.MakeIdle();
    }

    public override void SetTarget(Vector2 target)
    {
        if (IsGoingAway)
        {
            CalculateOrientation(GetTarget());
            ActivateState((int) MovableCharacterStates.Move);
        }
        else
        {
            base.SetTarget(target);
        }
    }

    public void CalculateRandomTarget(bool force = false)
    {
        if (IsGoingAway)
        {
            SetTarget(GetTarget());
            return;
        }
        if(AttackTarget != null && !force)
            return;
        Vector2 target = MonobehaviorHandler.GetMonobeharior().GetObject<Floor>("Floor").GetRandomPosition();
        if (target != default(Vector2))
            SetTarget(target);
    }

    public void CalculateAttackReaction()
    {
        if(_attackDenyedByTraining)
            return;
        bool willCounterAttack = Randomizer.GetPercentageBasedBoolean((int)CounterAttackProbability);
        if (willCounterAttack)
        {
            if (AttackTarget != null)
                AttackIfPossible();
            else
                MakeIdle();
        }
        else
        {
            AttackTarget = null;
            CalculateRandomTarget();
        }
    }

    public bool IsStickModifiedForTraining()
    {
        return _isStickModifiedForTraining;
    }

    public void StopStick()
    {
        if (!IsGoingAway)
        {
            CalculateRandomTarget();
        }
        else
        {
            if (_isStickModifiedForTraining)
            {
                TrainingHandler handler =
                    MonobehaviorHandler.GetMonobeharior().GetObject<TrainingHandler>("TrainingHandler");
                handler.SetIsGnomeSurvived(true);
                handler.ShowNext();
            }
            Destroy(gameObject);       
        }
        MonobehaviorHandler.GetMonobeharior().GetObject<DoorsTimer>("DoorsTimer").SetPaused(false);
        GameController.GetInstance().IncreaseAntiStick();
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
        if (_isTrainingEnabled)
        {
            TrainingHandler handler = MonobehaviorHandler.GetMonobeharior().GetObject<TrainingHandler>("TrainingHandler");
            handler.ShowNext();
            _isTrainingEnabled = false;
        }
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
        if (!MonobehaviorHandler.GetMonobeharior().GetObject<TrainingHandler>("TrainingHandler").IsFlyAwayEnabled())
            return;
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

    public void StopDrag(bool attack)
    {
        if(GetActiveState() != (int)MovableCharacterStates.Dragged)
            return;
        MonobehaviorHandler.GetMonobeharior().GetObject<Floor>("Floor").OnMouseUp();
        ConductorSM conductor = MonobehaviorHandler.GetMonobeharior().GetObject<Floor>("Floor").GetHero();
        if (conductor.CanKick(this))
        {
            if (!HasTicket())
            {
                CalculateRandomTarget();
            }
        }
        else
        {
            if (attack)
            {
                AttackTarget = conductor;
                AttackIfPossible();
            }
            else
            {
                MakeIdle();
            }
        }
    }
    
    public void CalculateMagnet()
    {
        if (_isMagnetActivated)
        {
            if (_hasTicket && _isVisibleToHero)
            {
                _isMagnetActivated = false;
                _isMagnetTurnedOn = false;
            }
        }
        ConductorSM hero = MonobehaviorHandler.GetMonobeharior().GetObject<Floor>("Floor").GetHero();
        Vector2 heroPosition = hero.transform.position;
        float heroDist = ((Vector2)transform.position - heroPosition).sqrMagnitude;
        if (heroDist < _magnetDistance || _isMagnetActivated)
        {
            _isMagnetActivated = true;
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
        MonobehaviorHandler.GetMonobeharior().GetObject<Floor>("Floor").NormalizePosition(ref newPos, true);
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
        CalculateIndicator();
        if(!hero.IsDragging())
            StopDrag(false);
        if (AttackTarget != null)
        {
            if (AttackTarget.CanNotInteract())
            {
                AttackTarget = null;
            }
        }
        _attackingDenyPeriod -= Time.deltaTime;
        if (_attackingDenyPeriod <= 0)
        {
            IsAttackingAllowed = true;
        }
        
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

    protected void ParseBonusMap()
    {
        BonusProbabilities = new Dictionary<GameController.BonusTypes, float>();
        JSONObject unparsedMap = ConfigReader.GetConfig().GetField(GetClassName()).GetField("BonusMap");
        foreach (var bonus in Enum.GetValues(typeof(GameController.BonusTypes)))
        {
            string representation = bonus.ToString();
            if (unparsedMap.HasField(representation))
            {
                BonusProbabilities.Add((GameController.BonusTypes)bonus, unparsedMap.GetField(representation).n);
            }
        }
    }

    public virtual string GetClassName()
    {
        return string.Empty;
    }

}
