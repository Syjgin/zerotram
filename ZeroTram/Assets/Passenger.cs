using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Math;
using Random = UnityEngine.Random;

namespace Assets
{
    public class Passenger : MovableObject
    {
        private MovableObject _attackTarget;
        private const float MoveProbability = 0.5f;
        private const int ChangeStatePeriod = 100;
        private int _timeForNextUpdate;

        new void Start()
        {
            base.Start();
            _timeForNextUpdate = 0;
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
            yield return null;
        }

        void FixedUpdate()
        {
            _timeForNextUpdate--;
        }

        private bool CanMove()
        {
            if (_timeForNextUpdate > 0)
                return false;
            _timeForNextUpdate = ChangeStatePeriod;
            float range = Randomizer.GetNormalizedRandom();
            return range > MoveProbability;
        }
    }
}
