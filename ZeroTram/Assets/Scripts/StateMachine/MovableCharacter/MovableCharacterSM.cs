﻿using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovableCharacterSM : StateMachine
{
    [SerializeField] private SpriteRenderer _lifebar;

    public float Velocity;
    public Animator Animator;
    public Rigidbody2D CharacterBody;
    public BoxCollider2D BoxCollider2D;
    protected bool IsDead;
    protected float Hp;
    public float AttackMaxDistance = 3;
    protected float AttackedStartTime;
    protected Vector2 Target;
    protected float AttackStrength = 10;
    public MovableCharacterSM AttackTarget;
    protected float AttackReactionPeriod = 0.5f;
    public float AttackReloadPeriod = 0.5f;
    protected float InitialLifes;
    public bool IsGoingAway;
    public float AttackDistance = 1;
    public float TimeSincePreviousClickMade;
    private bool _isFreezeInProgress;
    private bool _isFreezeTemporalyDisabled;
    private bool _isFreezeBonusActive;
    private SnowBonus.FreezeData _freezeData;
    private float _currentScale;
    protected bool IsAttackListenerActivated;
    private bool _isHalfImmortal;

    public const float MaxClickDuration = 0.6f;

    public enum MovableCharacterStates
    {
        Idle = 0,
        Move = 1,
        Drag = 2,
        Attack = 3,
        Attacked = 4,
        Stick = 5,
        FlyingAway = 6,
        Dragged = 7,
        Frozen = 8,
        Sit = 9,
        Hunt = 10,
        Escape = 11
    }

    public float GetInitialLifes()
    {
        return InitialLifes;
    }
    
    public Vector2 GetTarget()
    {
        return Target;
    }
    public virtual void SetTarget(Vector2 target)
    {
        Target = target;
        CalculateOrientation(target);
        ActivateState((int)MovableCharacterStates.Move);
    }

    public virtual void AddDamageValue(float damage)
    {
        Hp -= damage;
        if (Hp > InitialLifes)
            Hp = InitialLifes;
        float lifesPercent = Hp / InitialLifes;
        float originalValue = _lifebar.bounds.min.x;
        _lifebar.transform.localScale = new Vector3(lifesPercent, 1, 1);
        float newValue = _lifebar.bounds.min.x;
        float difference = newValue - originalValue;
        _lifebar.transform.Translate(new Vector3(-difference, 0, 0));
        if (lifesPercent > 0.5f)
        {
            _lifebar.color = Color.green;
        }
        if (lifesPercent < 0.5f && lifesPercent > 0.1f)
        {
            _lifebar.color = Color.yellow;
        }
        if (lifesPercent < 0.1f)
        {
            _lifebar.color = Color.red;
        }
        if (Hp <= 0)
        {
            Hp = 0;
            IsDead = true;
            GameController.GetInstance().RegisterDeath(this);
            if (!TrainingHandler.IsTrainingFinished())
            {
                if (this is ConductorSM)
                    return;
            }
            Destroy(this.gameObject);
        }
    }

    public void Resurrect()
    {
        Hp = InitialLifes;
        GameController.GetInstance().Resurrect();
    }

    public void SetAttackListenerActivated()
    {
        IsAttackListenerActivated = true;
    }

    public void SetHalfImmortal(bool value)
    {
        _isHalfImmortal = value;
    }

    public virtual void AddDamage(MovableCharacterSM attacker)
    {
        AttackedStartTime = Time.time;
        ActivateState((int)MovableCharacterStates.Attacked);
        if (attacker.AttackStrength < 0)
            MonobehaviorHandler.GetMonobeharior().GetObject<AudioPlayer>("AudioPlayer").PlayAudioById("heal");
        if (attacker.AttackStrength < 0 && Hp >= InitialLifes)
        {
            Hp = InitialLifes;
            return;
        }
        if (attacker.AttackStrength > 0)
            MonobehaviorHandler.GetMonobeharior().GetObject<AudioPlayer>("AudioPlayer").PlayAudioById("lowkick");
        float currentStrength = attacker.AttackStrength*Randomizer.GetNormalizedRandom();
        AttackTarget = attacker;
        AddDamageValue(currentStrength);
        if (_isHalfImmortal)
        {
            if (Hp < InitialLifes/2)
            {
                Hp = InitialLifes/2;
            }
        }
        if (IsAttackListenerActivated)
        {
            TrainingHandler handler =
                    MonobehaviorHandler.GetMonobeharior().GetObject<TrainingHandler>("TrainingHandler");
            handler.SetAttackedPassenger(this);
            handler.ShowNext();
            IsAttackListenerActivated = false;
        }
    }

    public bool IsAttackReationFinished()
    {
        return Time.time - AttackedStartTime > AttackReactionPeriod;
    }

    public float GetTargetDistance()
    {
        return ((Vector2) transform.position - GetTarget()).sqrMagnitude;
    }

    public bool IsInAttackRadius(Vector2 pos)
    {
        float sqrRemainingDistance = ((Vector2)transform.position - pos).sqrMagnitude;
        return sqrRemainingDistance <= AttackDistance;
    }

    public void MakeAttack()
    {
        AttackTarget.AddDamage(this);
    }

    public void CalculateOrientation(Vector2 target)
    {
        if (_currentScale == 0)
        {
            _currentScale = MonobehaviorHandler.GetMonobeharior().GetObject<Floor>("Floor").CalculateLocalScaleForMovable(this);
        }
        if (target.x > transform.position.x)
        {
            CharacterBody.transform.localScale = new Vector3(-_currentScale, _currentScale, 1);
        }
        else
        {
            CharacterBody.transform.localScale = new Vector3(_currentScale, _currentScale, 1);
        }
    }

    public virtual void MakeIdle()
    {
        ActivateState((int)MovableCharacterSM.MovableCharacterStates.Idle);
    }

    public virtual bool CanNotInteract()
    {
        return false;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        TimeSincePreviousClickMade += Time.fixedDeltaTime;
        if (!_isFreezeTemporalyDisabled && _isFreezeBonusActive)
        {
            float currentDist = ((Vector2)transform.position - _freezeData.StartPoint).sqrMagnitude;
            if (currentDist < _freezeData.Distance)
            {
                Freeze();
            }
        }
    }

    protected override void Update()
    {
        base.Update();
        float newScale = MonobehaviorHandler.GetMonobeharior().GetObject<Floor>("Floor").CalculateLocalScaleForMovable(this);
        if (Math.Abs(newScale - _currentScale) > 0.01f)
        {
            _currentScale = newScale;
            bool inversed = (CharacterBody.transform.localScale.x < 0);
            CharacterBody.transform.localScale = new Vector3(inversed ? -newScale : newScale, newScale, 1);
        }
    }

    public virtual void HandleClick()
    {   
    }

    public virtual void HandleDoubleClick()
    {
    }

    public void Freeze()
    {
        _isFreezeInProgress = true;
        ActivateState((int)MovableCharacterStates.Frozen);
    }

    public void ActivateFreezeBonus(SnowBonus.FreezeData data)
    {
        _isFreezeBonusActive = true;
        _freezeData = data;
    }

    public void UnFreeze()
    {
        _isFreezeInProgress = false;
        _isFreezeTemporalyDisabled = false;
        _isFreezeBonusActive = false;
        Animator.enabled = true;
        MakeIdle();
    }

    public void TemporalyUnfreeze()
    {
        _isFreezeInProgress = false;
        _isFreezeTemporalyDisabled = true;
    }

    public bool IsUnfreezeIsTemporary()
    {
        return _isFreezeTemporalyDisabled;
    }

    public bool IsFrozen()
    {
        return _isFreezeInProgress;
    }

    public virtual string GetClassName()
    {
        return string.Empty;
    }
}
