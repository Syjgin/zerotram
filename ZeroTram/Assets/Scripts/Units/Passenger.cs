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
        protected int MoveProbability = 50;
        protected int AttackProbability = 50;
        protected float ChangeStatePeriod = 10;
        protected float AttackDistance = 1;
        protected float AttackReloadPeriod = 0.5f;
        protected float CounterAttackProbability = 50;

        private bool _hasTicket;
        private bool _isVisibleToHero;

        private float _timeForNextUpdate;
        private float _timeSinceAttackMade;

        private Hero _hero;

        protected BackgroundManager Background;

        private bool _isDragged;

        private bool _isFlyingAway;

        private Vector2 _flyTarget;

        private const int FlyLength = 30;

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

        public bool IsAlreadyDragged()
        {
            return _isDragged;
        }

        public void FlyAway()
        {
            _isFlyingAway = true;
            _isDragged = true;
            CurrentState = State.Attacked;
            _flyTarget = new Vector2(transform.position.x, transform.position.y + FlyLength);
        }

        new void Start()
        {
            Background = GameObject.Find("background").GetComponent<BackgroundManager>();
            base.Start();
            _timeForNextUpdate = 0;
            _timeSinceAttackMade = AttackReloadPeriod;
            _hero = GameObject.Find("hero").GetComponent<Hero>();
        }

        protected void CalculateTicket(int currentTicketProbability)
        {
            int ticketProbability = Randomizer.GetRandomPercent();
            _hasTicket = ticketProbability > (100 - currentTicketProbability);
        }

        public virtual void Init()
        {
            
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
                        SetTarget(AttackTarget.transform.position);
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
            float sqrRemainingDistance = (transform.position - Target).sqrMagnitude;
            if (sqrRemainingDistance <= 1)
            {
                CurrentState = State.Idle;
                yield return null;
            }
            Vector3 newPosition = Vector3.MoveTowards(Rb2D.position, Target, Velocity * Time.deltaTime);
            Rb2D.MovePosition(newPosition);
        }

        private float AttackTargetDistance()
        {
            return (transform.position - AttackTarget.transform.position).sqrMagnitude;
        }

        protected override IEnumerator attack()
        {
            if (AttackTarget != null)
            {
                CalculateOrientation(AttackTarget.transform.position);
                if (CanAttack())
                {
                    Animator.Play("attack");
                    AttackTarget.AddDamage(this);
                    _timeSinceAttackMade = 0;
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
                        if (CanMove())
                        {
                            SetTarget(Background.GetRandomPosition());
                        }
                    }
                }   
            }
            yield return null;
        }

        void FixedUpdate()
        {
            _timeForNextUpdate-=Time.fixedDeltaTime;
            _timeSinceAttackMade+=Time.fixedDeltaTime;
        }

        void Update()
        {
            if(_hero == null)
                return;
            if (_isFlyingAway)
            {
                CurrentState = State.Attacked;
                Vector3 newPosition = Vector3.MoveTowards(Rb2D.position, _flyTarget, 5 * Velocity * Time.deltaTime);
                Rb2D.MovePosition(newPosition);
                Vector2 position2D = transform.position;
                float sqrRemainingDistance = (position2D - _flyTarget).sqrMagnitude;
                if (sqrRemainingDistance <= 1)
                {
                    GameController.GetInstance().RegisterDeath(this);
                    Destroy(this.gameObject);
                }
                return;
            }
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 transform2d = transform.position;
                float distance = (transform2d - hit.centroid).sqrMagnitude;
                if (distance < 1)
                {
                    if (_hero.IsInAttackRadius(this))
                    {
                        if (!_isVisibleToHero)
                        {
                            _isVisibleToHero = true;
                            DisplayTicketIcon();
                            return;
                        }
                        if (!_hero.IsInWayoutZone())
                        {
                            _hero.StartDrag(this);
                            CurrentState = State.Attacked;
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
                        }
                    }
                    else
                    {
                        _hero.SetTarget(this.transform.position);
                    }   
                }
            }
        }

        private void DisplayTicketIcon()
        {
            
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
                if (_timeSinceAttackMade >= AttackReloadPeriod && AttackTargetDistance() <= AttackDistance)
                    return true;
            }
            return false;
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            MovableObject movable = other.GetComponent<MovableObject>();
            if (movable != null)
            {
                if (CanAttack())
                {
                    AttackTarget = movable;
                    CurrentState = State.Attack;
                }
            }
        }
    }
}
