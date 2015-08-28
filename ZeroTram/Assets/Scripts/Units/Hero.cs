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
        private float _maxDragDistance;
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
            _maxDragDistance = ConfigReader.GetConfig().GetField("hero").GetField("MaxDragDistance").n;
            Hp = InitialLifes = ConfigReader.GetConfig().GetField("hero").GetField("InitialLifes").n;
            AttackMaxDistance = ConfigReader.GetConfig().GetField("hero").GetField("AttackMaxDistance").n;
            AttackReloadPeriod = ConfigReader.GetConfig().GetField("hero").GetField("AttackReloadPeriod").n;
            Velocity = ConfigReader.GetConfig().GetField("hero").GetField("Velocity").n;
            AttackReactionPeriod = ConfigReader.GetConfig().GetField("hero").GetField("AttackReactionPeriod").n;
            AttackReloadPeriod = ConfigReader.GetConfig().GetField("hero").GetField("AttackReloadPeriod").n;
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
            if (obj.IsStick)
            {
                obj.StopStick();
                obj.WasStickWhenFlyAway = true;
            }
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

        public bool IsInAttackRadius(Passenger obj)
        {
            float sqrRemainingDistance = (GetPosition() - obj.GetPosition()).sqrMagnitude;
            bool isDistanceCorrect = sqrRemainingDistance <= AttackMaxDistance;
            return isDistanceCorrect;
        }


        public void StartDrag(Passenger obj)
        {
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
            Vector2 position2D = GetPosition();
            float dist = (position2D - targetPos).sqrMagnitude;
            if(dist > 0.001f)
                CalculateOrientation(targetPos);
            float currentDist = (targetPos - _dragStartPoint).sqrMagnitude;
            if (currentDist > _maxDragDistance)
            {
                StopDrag();
                return;
            }
            SetPosition(targetPos);
            _dragTarget.SetPosition(targetPos);
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
