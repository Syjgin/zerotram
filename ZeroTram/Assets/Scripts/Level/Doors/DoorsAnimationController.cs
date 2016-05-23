using UnityEngine;
using System.Collections;

public class DoorsAnimationController : MonoBehaviour
{
    private Animator _animator;
    [SerializeField]
    private Spawner _unitSpawner;
    private bool _isOpened;
    private bool _willBeOpened;

	void Awake ()
	{
	    _animator = GetComponent<Animator>();
	}
	
	void Update () {
	
	}

    public void Open(bool spawn)
    {
        _animator.enabled = true;
        _animator.Play("doors_open");
        if (spawn)
            _unitSpawner.Spawn(gameObject);
        _isOpened = true;
    }

    public void OpenAndSpawnByName(string passengerName, Spawner.TicketAdditionMode ticketMode)
    {
        _animator.enabled = true;
        _animator.Play("doors_open");
        _unitSpawner.Spawn(gameObject, passengerName, ticketMode);
        _isOpened = true;
    }

    public void Close()
    {
        _animator.Play("doors_close");
        _isOpened = false;
        _willBeOpened = false;
    }

    public void Glitch()
    {
        _animator.Play("doors_glitch");
        _isOpened = false;
    }

    public bool IsOpened()
    {
        return _isOpened;
    }

    public void SetWillBeOpened()
    {
        _willBeOpened = true;
    }

    public bool IsWillBeOpened()
    {
        return _willBeOpened;
    }
}
