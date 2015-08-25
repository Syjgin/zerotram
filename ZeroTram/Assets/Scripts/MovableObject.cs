using System;
using UnityEngine;
using System.Collections;

public class MovableObject : MonoBehaviour {
    protected BoxCollider2D BoxCollider;
    protected Rigidbody2D Rb2D;
    protected Animator Animator;

    enum State
    {
        Idle,
        Walking,
        Drag,
        Boot, 
        Attack
    }

    private State _currentState;
    private Vector3 _target;
    private const float Velocity = 5f;

    // Use this for initialization
    void Start()
    {
        _currentState = State.Idle;
        BoxCollider = GetComponent<BoxCollider2D>();
        Rb2D = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        StartCoroutine(mainLoop());
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
        while (true)
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

    IEnumerator walk()
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

    IEnumerator idle()
    {
        Animator.Play("idle");
        yield return null;
    }

    IEnumerator drag()
    {
        Animator.Play("drag");
        yield return null;
    }

    IEnumerator attack()
    {
        Animator.Play("attack");
        yield return null;
    }

    IEnumerator boot()
    {
        Animator.Play("boot");
        yield return null;
    }
}
