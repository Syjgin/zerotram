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

        private Hero _hero;

        protected BackgroundManager Background;

        private bool _isDragged;

        private bool _isFlyingAway;

        private Vector2 _flyTarget;

        private const int FlyLength = 30;

        private float _maxStopCount;

        private Vector3 _indicatorOffset;

        public bool HasTicket()
        {
            return _hasTicket;
        }

        protected override bool CanChangeState()
        {
            return CurrentState != State.Stick && !IsGoingAway && !_isFlyingAway;
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
            GameObject heroObject = GameObject.Find("hero");
            if (heroObject != null)
                _hero = heroObject.GetComponent<Hero>();
            _indicatorOffset = Indicator.transform.position - Rb2D.transform.position;
        }

        private void CalculateTicket(float currentTicketProbability)
        {
            _hasTicket = Randomizer.GetPercentageBasedBoolean((int) currentTicketProbability);
        }

        public virtual void Init()
        {
            CalculateTicket(TicketProbability);
            CalculateStick();
            _maxStopCount = ConfigReader.GetConfig().GetField("tram").GetField("MaxStopCount").n;
            int stopCount = Randomizer.GetInRange(1, (int)_maxStopCount);
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
                    CalculateRandomTarget();
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

        private void CalculateRandomTarget()
        {
            Vector2 target = Background.GetRandomPosition();
            if (target != default(Vector2))
                SetTarget(target);
        }

        protected override IEnumerator walk()
        {
            Animator.Play("walk");
            float sqrRemainingDistance = (GetPosition() - Target).sqrMagnitude;
            if (sqrRemainingDistance <= 1)
            {
                if (IsGoingAway)
                {
                    CalculateStickOnExit();
                    if(CurrentState != State.Stick)
                        Destroy(gameObject);
                }
                if(CurrentState != State.Stick)
                    CurrentState = State.Idle;
                yield return null;
            }
            Vector3 newPosition = Vector3.MoveTowards(Rb2D.position, Target, Velocity * Time.deltaTime);
            newPosition.z = -1;
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
            bool willCounterAttack = Randomizer.GetPercentageBasedBoolean((int) CounterAttackProbability);
            if (willCounterAttack)
            {
                if (AttackTarget != null)
                    CurrentState = State.Attack;
                else
                    CurrentState = State.Idle;
            }
            else
            {
                CalculateRandomTarget();
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
            Debug.Log("click handled");
            if(_isDragged)
                _hero.StopDrag();
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
                    else
                    {
                        Player.PlayAudioById("coins");
                    }
                    StopStick();
                    return;
                }
                if (!_hero.IsInWayoutZone())
                {
                    BecomeDragged();
                }
                else
                {
                    if (!_hasTicket)
                    {
                        _hero.Kick(this);
                        return;
                    }
                    if (AttackTarget == _hero)
                    {
                        _hero.Kick(this);
                        return;
                    }
                    if (IsStick)
                    {
                        if(IsGoingAway)
                            _hero.Kick(this);
                        else
                            _hero.StartDrag(this);
                        return;
                    }
                    BecomeDragged();
                }
            }
            else
            {
                _hero.SetTarget(GetPosition());
            }
        }

        private void BecomeDragged()
        {
            if (!_hero.IsUnderAttack())
            {
                _hero.StartDrag(this);
                CurrentState = State.Attacked;
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
            if (GetPosition().z > -1)
            {
                Vector3 correctPos = GetPosition();
                correctPos.z = -1;
                SetPosition(correctPos);
            }
            CalculateIndicator();
            if(_hero == null)
                return;
            if (_isFlyingAway)
            {
                CurrentState = State.Attacked;
                Vector3 newPosition = Vector3.MoveTowards(Rb2D.position, _flyTarget, 50 * Time.deltaTime);
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
            else
            {
                if (CurrentState != State.Attack && CurrentState != State.Attacked)
                {
                    GameController.GetInstance().TryAttackNearThis(this);
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
            return Randomizer.GetPercentageBasedBoolean((int) MoveProbability);
        }

        private bool CanAttack()
        {
            if (_isFlyingAway || IsGoingAway || IsStick)
                return false;
            if (AttackTarget == null)
            {
                return Randomizer.GetPercentageBasedBoolean((int) AttackProbability);
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
            if (_isDragged || _isFlyingAway || IsGoingAway)
                return;
            MovableObject movable = other.gameObject.GetComponentInParent<MovableObject>();
            if (movable != null)
            {
                TryAttackMovable(movable);
            }
        }

        public void TryAttackMovable(MovableObject movable)
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
            bool stick = Randomizer.GetPercentageBasedBoolean((int) StickProbability);
            if (stick && Background.IsPassengerNearDoors(this))
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
