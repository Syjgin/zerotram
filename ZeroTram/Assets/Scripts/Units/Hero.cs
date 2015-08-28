using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public class Hero : MovableObject
    {
        private BackgroundManager _backgroundManager;
        private Passenger _dragTarget;
        private Vector2 _dragStartPoint;
        private const float MaxDragDistance = 8f;
        private bool _isInWayoutZone;
        private Text _lifes;
        
        public void SetInWayoutZone(bool inZone)
        {
            _isInWayoutZone = inZone;
        }

        public bool IsInWayoutZone()
        {
            return _isInWayoutZone;
        }

        void Awake()
        {
            Hp = InitialLifes = 500;
            AttackMaxDistance = 1;
            AttackReloadPeriod = 0.5f;
            _lifes = GameObject.Find("userLifes").GetComponent<Text>();
            _lifes.text = "здоровье:100%";
        }

        public override void AddDamage(MovableObject attacker)
        {
            base.AddDamage(attacker);
            int lifesPercent = Mathf.RoundToInt(100*(Hp/(float) InitialLifes));
            _lifes.text = "здоровье:" + lifesPercent + "%";
        }

        void FixedUpdate()
        {
            TimeSinceAttackMade += Time.fixedDeltaTime;
        }
        public void Kick(Passenger obj)
        {
            CurrentState = State.Attack;
            if(obj.IsStick)
                obj.StopStick(true);
            obj.FlyAway();
            TimeSinceAttackMade = 0;
            AttackTarget = null;
            _dragTarget = null;
        }

        protected new void Start()
        {
            base.Start();
            _backgroundManager = GameObject.Find("background").GetComponent<BackgroundManager>();
        }

        public bool IsInAttackRadius(MovableObject obj)
        {
            float sqrRemainingDistance = (transform.position - obj.transform.position).sqrMagnitude;
            bool isDistanceCorrect = sqrRemainingDistance <= AttackMaxDistance;
            Passenger ps = obj.GetComponent<Passenger>();
            if (ps != null)
            {
                if (ps.IsGoingAway)
                    return false;
            }
            return isDistanceCorrect;
        }


        public void StartDrag(Passenger obj)
        {
            if(obj.IsAlreadyDragged())
                return;
            if (obj.IsStick)
            {
                obj.StopStick();
            }
            CurrentState = State.Drag;
            obj.SetDragged(true);
            _dragTarget = obj;
            _dragStartPoint = _backgroundManager.GetCurrentMousePosition();
            CalculateOrientation(_dragStartPoint);
        }

        public void UpdatePositionForDrag()
        {
            if (_dragTarget == null)
            {
                StopDrag();
                return;
            }
            Vector2 targetPos = _backgroundManager.GetCurrentMousePosition();
            Vector2 position2D = transform.position;
            float dist = (position2D - targetPos).sqrMagnitude;
            if(dist > 0.001f)
                CalculateOrientation(targetPos);
            float currentDist = (targetPos - _dragStartPoint).sqrMagnitude;
            if (currentDist > MaxDragDistance)
            {
                StopDrag();
                return;
            }
            transform.position = targetPos;
            _dragTarget.transform.position = targetPos;
        }

        public bool IsUnderAttack()
        {
            return CurrentState == State.Attacked;
        }

        public bool IsDragging()
        {
            return CurrentState == State.Drag;
        }

        public void StopDrag()
        {
            if (CurrentState == State.Drag)
            {
                CurrentState = State.Idle;
                if (_dragTarget != null)
                {
                    _dragTarget.SetDragged(false);
                }   
            }
        }

        void OnMouseUp()
        {
            StopDrag();
        }
    }
}
