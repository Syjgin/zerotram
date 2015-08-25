using System;
using UnityEngine;
using System.Collections;

public class MovableObject : MonoBehaviour {
    protected BoxCollider2D BoxCollider;
    protected Rigidbody2D Rb2D;
    protected Animator Animator;
    protected bool IsDead;
    protected int Hp;
    protected BackgroundManager Background;

    protected enum State
    {
        Idle,
        Walking,
        Drag,
        Boot, 
        Attack
    }

    protected State _currentState;
    private Vector3 _target;
    private const float Velocity = 5f;

    // Use this for initialization
    protected void Start()
    {
        _currentState = State.Idle;
        BoxCollider = GetComponent<BoxCollider2D>();
        Rb2D = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        Background = GameObject.Find("background").GetComponent<BackgroundManager>();
        StartCoroutine(mainLoop());
    }

    public void AddDamage(int damage)
    {
        Hp -= damage;
        if (damage < 0)
        {
            IsDead = true;   
        }
    }

    public void SetTarget(Vector3 target)
    {
        _target = target;
        if (_target.x > Rb2D.position.x)
            Rb2D.transform.localScale = new Vector3(-1, 1, 1);
        else
            Rb2D.transform.localScale = new Vector3(1, 1, 1);
        _currentState = State.Walking;
    }

    public bool IsIdle()
    {
        return _currentState == State.Idle;
    }

    void Update()
    {
    }

    IEnumerator mainLoop()
    {
        while (!IsDead)
        {
            switch (_currentState)
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
                case State.Boot:
                    yield return StartCoroutine(boot());
                    break;
                case State.Attack:
                    yield return StartCoroutine(attack());
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    protected IEnumerator walk()
    {
        Animator.Play("walk");
        float sqrRemainingDistance = (transform.position - _target).sqrMagnitude;
        if (sqrRemainingDistance < float.Epsilon)
        {
            _currentState = State.Idle;
            yield return null;
        }
        Vector3 newPosition = Vector3.MoveTowards(Rb2D.position, _target, Velocity * Time.deltaTime);
        Rb2D.MovePosition(newPosition);
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

    protected IEnumerator attack()
    {
        Animator.Play("attack");
        yield return null;
    }

    protected IEnumerator boot()
    {
        Animator.Play("boot");
        yield return null;
    }
}
