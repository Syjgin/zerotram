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
        private MovableObject _attackTarget;

        private int _moveProbability = 75;
        private int _attackProbability = 100;
        private int _attackReloadPeriod = 100;
        private int _attackStrength = 10;
        private int _changeStatePeriod = 100;
        private float _attackDistance = 0.5f;
        private float _attackMaxDistance = 2f;

        private bool _hasTicket;

        private int _timeForNextUpdate;
        private int _timeSinceAttackMade;

        public void InitWithParameters(int moveProbability, int attackProbability, int attackReloadPeriod,
            int attackStrength, int changeStatePeriod, int attackDistance, int attackMaxDistance,
            int velocity, int ticketProbability)
        {
            _moveProbability = moveProbability;
            _attackProbability = attackProbability;
            _attackReloadPeriod = attackReloadPeriod;
            _attackStrength = attackStrength;
            _changeStatePeriod = changeStatePeriod;
            _attackDistance = attackDistance;
            _attackMaxDistance = attackMaxDistance;
            Velocity = velocity;
            _hasTicket = Randomizer.GetRandomPercent() > (100 - ticketProbability);
        }

        public bool HasTicket()
        {
            return _hasTicket;
        }

        new void Start()
        {
            base.Start();
            _timeForNextUpdate = 0;
            _timeSinceAttackMade = 0;
        }

        public void SetAttackTarget(MovableObject target)
        {
            _attackTarget = target;
        }

        protected override IEnumerator idle()
        {
            if (_attackTarget == null)
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
                if (dist > _attackMaxDistance)
                {
                    _attackTarget = null;
                }
                else
                {
                    if (dist > _attackDistance)
                    {
                        SetTarget(_attackTarget.transform.position);
                    }
                    else
                    {
                        CurrentState = State.Attack;
                    }   
                }
            }
            yield return null;
        }

        private float AttackTargetDistance()
        {
            return (transform.position - _attackTarget.transform.position).sqrMagnitude;
        }

        protected override IEnumerator attack()
        {
            if (_attackTarget != null)
            {
                CalculateOrientation(_attackTarget.transform.position);
                if (CanAttack())
                {
                    Animator.Play("attack");
                    Debug.Log("attack performed");
                    _attackTarget.AddDamage(_attackStrength);
                    _timeSinceAttackMade = 0;
                }
                else
                {
                    CurrentState = State.Idle;
                }   
            }
            yield return null;
        }

        void FixedUpdate()
        {
            _timeForNextUpdate--;
            _timeSinceAttackMade++;
        }

        private bool CanMove()
        {
            if (_timeForNextUpdate > 0)
                return false;
            _timeForNextUpdate = _changeStatePeriod;
            int range = Randomizer.GetRandomPercent();
            return range > (100 - _moveProbability);
        }

        private bool CanAttack()
        {
            if (_attackTarget == null)
            {
                int range = Randomizer.GetRandomPercent();
                if (range > (100 - _attackProbability))
                    return true;
            }
            else
            {
                if (_timeSinceAttackMade >= _attackReloadPeriod && AttackTargetDistance() <= _attackDistance)
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
                    _attackTarget = movable;
                    CurrentState = State.Attack;
                }
            }
        }
    }
}
