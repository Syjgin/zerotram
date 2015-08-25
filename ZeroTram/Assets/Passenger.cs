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

        //TODO: fill this in parametrized constructor in derived classes
        private const int MoveProbability = 75;
        private const int AttackProbability = 100;
        private const int AttackReloadPeriod = 100;
        private const int AttackStrength = 10;
        private const int ChangeStatePeriod = 100;
        private const float AttackDistance = 0.5f;
        private const float AttackMaxDistance = 2f;

        private int _timeForNextUpdate;
        private int _timeSinceAttackMade;



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
            Animator.Play("idle");
            if (_attackTarget == null)
            {
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
                    _attackTarget = null;
                }
                else
                {
                    if (dist > AttackDistance)
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
            CalculateOrientation(_attackTarget.transform.position);
            if (CanAttack())
            {
                Animator.Play("attack");
                CurrentState = State.Idle;
                _timeSinceAttackMade = 0;   
            }
            else
            {
                CurrentState = State.Idle;
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
            _timeForNextUpdate = ChangeStatePeriod;
            int range = Randomizer.GetRandomPercent();
            return range > (100 - MoveProbability);
        }

        private bool CanAttack()
        {
            if (_attackTarget == null)
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
                    _attackTarget = movable;
                    CurrentState = State.Attack;
                }
            }
        }
    }
}
