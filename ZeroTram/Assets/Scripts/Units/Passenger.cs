using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Math;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets
{
    public class Passenger : MovableObject
    {
        [SerializeField] private Sprite _question;
        [SerializeField] private Sprite _ticket;
        [SerializeField] private Sprite _hare;
        [SerializeField] private Sprite _stick;
        [SerializeField] protected SpriteRenderer Indicator;

        public bool WasStickWhenFlyAway { get; set; }

        private int _tramStopCount;
        private int _currentTramStopCount;
        protected float MoveProbability = 50;
        protected float AttackProbability = 50;
        protected float ChangeStatePeriod = 10;
        protected float AttackDistance = 1;
        protected float CounterAttackProbability = 50;
        protected float StickProbability = 0;
        protected float TicketProbability;

        private bool _hasTicket;
        private bool _isVisibleToHero;

        private float _timeForNextUpdate;

        private bool _isGoingAway;

        private Hero _hero;

        protected BackgroundManager Background;

        private bool _isDragged;

        private bool _isFlyingAway;

        private Vector2 _flyTarget;

        private const int FlyLength = 30;

        private const int MaxStopCount = 3;

        private Vector3 _indicatorOffset;


        public bool HasTicket()
        {
            return _hasTicket;
        }

        public void SetDragged(bool dragged)
        {
            _isDragged = dragged;
            if(!dragged)
                CurrentState = State.Idle;
        }

        public void FlyAway()
        {
            _isFlyingAway = true;
            _isDragged = true;
            CurrentState = State.Attacked;
            _flyTarget = new Vector2(Rb2D.transform.position.x, Rb2D.transform.position.y + FlyLength);
            GameController.GetInstance().RegisterDeath(this);
        }

        void Awake()
        {
            Background = GameObject.Find("background").GetComponent<BackgroundManager>();
        }

        new void Start()
        {
            base.Start();
            _timeForNextUpdate = 0;
            TimeSinceAttackMade = AttackReloadPeriod;
            _hero = GameObject.Find("hero").GetComponent<Hero>();
            _indicatorOffset = Indicator.transform.position - Rb2D.transform.position;
        }

        private void CalculateTicket(float currentTicketProbability)
        {
            int ticketProbability = Randomizer.GetRandomPercent();
            _hasTicket = ticketProbability > (100 - currentTicketProbability);
        }

        public virtual void Init()
        {
            CalculateTicket(TicketProbability);
            CalculateStick();
            int stopCount = Randomizer.GetInRange(1, MaxStopCount);
            _tramStopCount = stopCount;
            GameController.GetInstance().RegisterPassenger(this);
        }

        public void SetAttackTarget(MovableObject target)
        {
            AttackTarget = target;
        }

        protected override IEnumerator idle()
        {
            if (AttackTarget == null)
            {
                Animator.Play("idle");
                if (CanMove())
                {
                    SetTarget(Background.GetRandomPosition());
                }
            }
            else
            {
                float dist = AttackTargetDistance();
                if (dist > AttackMaxDistance)
                {
                    AttackTarget = null;
                }
                else
                {
                    if (dist > AttackDistance)
                    {
                        SetTarget(AttackTarget.GetPosition());
                    }
                    else
                    {
                        CurrentState = State.Attack;
                    }   
                }
            }
            yield return null;
        }

        protected override IEnumerator walk()
        {
            Animator.Play("walk");
            float sqrRemainingDistance = (GetPosition() - Target).sqrMagnitude;
            if (sqrRemainingDistance <= 1)
            {
                if (_isGoingAway)
                {
                    GameController.GetInstance().RegisterTravelFinish();
                    CalculateStickOnExit();
                    if(CurrentState != State.Stick)
                        Destroy(gameObject);
                }
                if(CurrentState != State.Stick)
                    CurrentState = State.Idle;
                yield return null;
            }
            Vector3 newPosition = Vector3.MoveTowards(Rb2D.position, Target, Velocity * Time.deltaTime);
            Rb2D.MovePosition(newPosition);
            MoveLifebar(newPosition);
        }

        private float AttackTargetDistance()
        {
            return (GetPosition() - AttackTarget.GetPosition()).sqrMagnitude;
        }

        protected override IEnumerator attack()
        {
            if (AttackTarget != null)
            {
                CalculateOrientation(AttackTarget.GetPosition());
                if (CanAttack())
                {
                    Animator.Play("attack");
                    AttackTarget.AddDamage(this);
                    TimeSinceAttackMade = 0;
                }
                else
                {
                    CurrentState = State.Idle;
                }
            }
            else
            {
                CurrentState = State.Idle;
            }
            yield return null;
        }

        protected override IEnumerator attacked()
        {
            Animator.Play("attacked");
            if (!_isDragged)
            {
                float timeDist = Time.time - AttackedStartTime;
                if (timeDist > AttackReactionPeriod)
                {
                    CalculateAttackReaction();
                }
            }
            yield return null;
        }

        private void CalculateAttackReaction()
        {
            float currentCounterAttackProbability = Randomizer.GetRandomPercent();
            if (currentCounterAttackProbability > (100 - CounterAttackProbability))
            {
                if (AttackTarget != null)
                    CurrentState = State.Attack;
                else
                    CurrentState = State.Idle;
            }
            else
            {
                SetTarget(Background.GetRandomPosition());
            }
        }

        void FixedUpdate()
        {
            _timeForNextUpdate-=Time.fixedDeltaTime;
            TimeSinceAttackMade+=Time.fixedDeltaTime;
        }

        public bool IsVisibleToHero
        {
            get { return _isVisibleToHero; }
        }

        public void HandleClick()
        {
            if (_hero.IsInAttackRadius(this))
            {
                if (!_isVisibleToHero)
                {
                    _isVisibleToHero = true;
                    GameController.GetInstance().UpdatePassenger(this);
                    if (!_hasTicket)
                    {
                        AttackTarget = _hero;
                        CalculateAttackReaction();
                    }
                    StopStick();
                    return;
                }
                if (!_hero.IsInWayoutZone())
                {
                    if (!_hero.IsUnderAttack())
                    {
                        _hero.StartDrag(this);
                        CurrentState = State.Attacked;   
                    }
                }
                else
                {
                    if (!_hasTicket)
                        _hero.Kick(this);
                    else
                    {
                        if (AttackTarget == _hero)
                        {
                            _hero.Kick(this);
                        }
                    }
                    if (IsStick)
                    {
                        if(_isGoingAway)
                            _hero.Kick(this);
                        else
                            _hero.StartDrag(this);
                    }
                }
            }
            else
            {
                _hero.SetTarget(GetPosition());
            }
        }

        private void CalculateIndicator()
        {
            if (IsStick)
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

        void Update()
        {
            CalculateIndicator();
            if(_hero == null)
                return;
            if (_isFlyingAway)
            {
                CurrentState = State.Attacked;
                Vector3 newPosition = Vector3.MoveTowards(Rb2D.position, _flyTarget, 5 * Velocity * Time.deltaTime);
                Rb2D.MovePosition(newPosition);
                MoveLifebar(newPosition);
                Vector2 position2D = GetPosition();
                float sqrRemainingDistance = (position2D - _flyTarget).sqrMagnitude;
                if (sqrRemainingDistance <= 1)
                {
                    Destroy(this.gameObject);
                }
            }
            if (IsStick)
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (Input.GetMouseButtonDown(0))
                {
                    Vector2 transform2d = GetPosition();
                    float distance = (transform2d - hit.centroid).sqrMagnitude;
                    if (distance < 1)
                    {
                        HandleClick();
                    }
                }   
            }
        }

        void OnMouseUp()
        {
            SetDragged(false);
        }

        private bool CanMove()
        {
            if (_timeForNextUpdate > 0)
                return false;
            _timeForNextUpdate = ChangeStatePeriod;
            int range = Randomizer.GetRandomPercent();
            return range > (100 - MoveProbability);
        }

        private bool CanAttack()
        {
            if (AttackTarget == null)
            {
                int range = Randomizer.GetRandomPercent();
                if (range > (100 - AttackProbability))
                    return true;
            }
            else
            {
                Passenger ps = AttackTarget.GetComponent<Passenger>();
                if (ps != null)
                {
                    if (ps.IsGoingAway || ps.IsStick)
                    {
                        AttackTarget = null;
                        return false;   
                    }
                }
                if (TimeSinceAttackMade >= AttackReloadPeriod && AttackTargetDistance() <= AttackDistance)
                    return true;
            }
            return false;
        }

        public void HandleTriggerEnter(Collider2D other)
        {
            if (_isDragged || _isFlyingAway || _isGoingAway)
                return;
            MovableObject movable = other.gameObject.GetComponentInParent<MovableObject>();
            if (movable != null)
            {
                if (CanAttack())
                {
                    AttackTarget = movable;
                    CurrentState = State.Attack;
                }
                else
                {
                    CurrentState = State.Idle;
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
                SetTarget(target);
                _isGoingAway = true;
            }
        }

        public bool IsGoingAway
        {
            get { return _isGoingAway; }
        }

        public bool IsStick
        {
            get { return CurrentState == State.Stick; }
        }

        public void StopStick()
        {
            ForceChangeState(State.Idle);
            GetTimer().SetPaused(false);
        }

        private void CalculateStick()
        {
            int random = Randomizer.GetRandomPercent();
            if (random > (100 - StickProbability) && Background.IsPassengerNearDoors(this))
            {
                CurrentState = State.Stick;
                Indicator.sprite = _stick;   
            }
        }

        public void CalculateStickOnExit()
        {
            CalculateStick();
            if (IsStick)
            {
                SetPosition(new Vector3(Target.x, Target.y - Spawner.StickYOffset, 0));
                GetTimer().SetPaused(true);
            }
        }

        private DoorsTimer GetTimer()
        {
            return GameObject.Find("Spawner").GetComponent<DoorsTimer>();
        }

        protected override void MoveLifebar(Vector3 position)
        {
            base.MoveLifebar(position);
            Indicator.transform.position = position + _indicatorOffset;
        }
    }
}
