using UnityEngine;
using System.Collections;

public class DoorsAnimationController : MonoBehaviour
{
    private Animator _animator;
    private Spawner _unitSpawner;

	void Awake ()
	{
	    _animator = GetComponent<Animator>();
	    _unitSpawner = GameObject.Find("Spawner").GetComponent<Spawner>();
	}
	
	void Update () {
	
	}

    public void Open()
    {
        _animator.enabled = true;
        _animator.Play("doors_open");
        _unitSpawner.Spawn(gameObject);
    }

    public void Close()
    {
        _animator.Play("doors_close");
    }
}
