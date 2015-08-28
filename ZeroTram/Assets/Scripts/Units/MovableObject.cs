﻿using System;
using System.Runtime.InteropServices;
using Assets;
using UnityEngine;
using System.Collections;

public class MovableObject : MonoBehaviour {
    protected Animator Animator;
    protected bool IsDead;
    protected int Hp;
    protected float AttackMaxDistance = 3;
    protected float AttackedStartTime;

    [SerializeField] private SpriteRenderer _lifebar;
    [SerializeField] protected Rigidbody2D Rb2D;
    protected enum State
    {
        Idle,
        Walking,
        Drag, 
        Attack,
        Attacked
    }

    protected State CurrentState;
    protected Vector3 Target;
    protected float Velocity = 5f;
    protected int AttackStrength = 10;
    protected MovableObject AttackTarget;
    protected float AttackReactionPeriod = 0.5f;
    protected float AttackReloadPeriod = 0.5f;
    protected float TimeSinceAttackMade;
    protected int InitialLifes;
    private Vector3 _lifebarOffset;

    // Use this for initialization
    protected void Start()
    {
        CurrentState = State.Idle;
        Animator = Rb2D.GetComponent<Animator>();
        _lifebarOffset = _lifebar.transform.position - Rb2D.transform.position;
        StartCoroutine(mainLoop());
    }

    public Vector3 GetPosition()
    {
        return Rb2D.transform.position;
    }

    public void SetPosition(Vector3 position)
    {
        Rb2D.transform.position = position;
        MoveLifebar(position);
    }

    public virtual void AddDamage(MovableObject attacker)
    {
        Hp -= attacker.AttackStrength;
        CurrentState = State.Attacked;
        AttackedStartTime = Time.time;
        AttackTarget = attacker;
        float lifesPercent = Hp/(float) InitialLifes;
        float originalValue = _lifebar.bounds.min.x;
        _lifebar.transform.localScale = new Vector3(lifesPercent, 1, 1);
        float newValue = _lifebar.bounds.min.x;
        float difference = newValue - originalValue;
        _lifebar.transform.Translate(new Vector3(-difference, 0f, 0f));
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
            IsDead = true; 
            GameController.GetInstance().RegisterDeath(this);
            Destroy(this.gameObject);
        }
    }

    public void SetTarget(Vector3 target)
    {
        Target = target;
        CalculateOrientation(target);
        CurrentState = State.Walking;
    }

    protected void CalculateOrientation(Vector2 target)
    {
        if (Rb2D != null)
        {
            if (target.x > Rb2D.position.x)
            {
                Rb2D.transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                Rb2D.transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }

    public bool IsIdle()
    {
        return CurrentState == State.Idle;
    }   

    IEnumerator mainLoop()
    {
        while (!IsDead)
        {
            switch (CurrentState)
            {
                case State.Walking:
                    yield return StartCoroutine(walk());
                    break;
                case State.Idle:
                    yield return StartCoroutine(idle());
                    break;
                case State.Drag:
                    yield return StartCoroutine(drag());
                    break;
                case State.Attack:
                    yield return StartCoroutine(attack());
                    break;
                case State.Attacked:
                    yield return StartCoroutine(attacked());
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    protected virtual IEnumerator walk()
    {
        Animator.Play("walk");
        float sqrRemainingDistance = (GetPosition() - Target).sqrMagnitude;
        if (sqrRemainingDistance <= 1)
        {
            CurrentState = State.Idle;
            yield return null;
        }
        Vector3 newPosition = Vector3.MoveTowards(Rb2D.position, Target, Velocity * Time.deltaTime);
        Rb2D.MovePosition(newPosition);
        MoveLifebar(newPosition);
    }

    protected virtual void MoveLifebar(Vector3 position)
    {
        _lifebar.transform.position = position + _lifebarOffset;
    }

    protected virtual IEnumerator idle()
    {
        Animator.Play("idle");
        yield return null;
    }

    protected IEnumerator drag()
    {
        Animator.Play("drag");
        yield return null;
    }

    protected virtual IEnumerator attack()
    {
        Animator.Play("attack");
        if (AttackTarget == null && TimeSinceAttackMade > AttackReloadPeriod)
        {
            TimeSinceAttackMade = 0;
            CurrentState = State.Idle;   
        }
        yield return null;
    }

    protected virtual IEnumerator attacked()
    {
        Animator.Play("attacked");
        if (Time.time - AttackedStartTime > AttackReactionPeriod)
        {
            CurrentState = State.Idle;
        }
        yield return null;
    }
}
