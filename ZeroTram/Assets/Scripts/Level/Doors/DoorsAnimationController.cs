﻿using UnityEngine;
using System.Collections;

public class DoorsAnimationController : MonoBehaviour
{
    private Animator _animator;
    private Spawner _unitSpawner;
    private bool _isOpened;

	void Awake ()
	{
	    _animator = GetComponent<Animator>();
	    _unitSpawner = GameObject.Find("Spawner").GetComponent<Spawner>();
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

    public void Close()
    {
        _animator.Play("doors_close");
        _isOpened = false;
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
}
