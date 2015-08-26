using UnityEngine;
using System.Collections;

public class DoorsAnimationController : MonoBehaviour
{
    private Animator _animator;

	void Awake ()
	{
	    _animator = GetComponent<Animator>();
	}
	
	void Update () {
	
	}

    public void Open()
    {
        _animator.enabled = true;
        _animator.Play("doors_open");
    }

    public void Close()
    {
        _animator.Play("doors_close");
    }
}
