using System;
using Assets.Scripts.Math;
using UnityEngine;
using System.Collections;

public class BackgroundManager : MonoBehaviour
{

    [SerializeField] private Camera _mainCamera;
    [SerializeField] private MovableObject _hero;

    private BoxCollider2D _collider;
	// Use this for initialization
	void Start ()
	{
	    _collider = GetComponent<BoxCollider2D>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public Vector2 GetRandomPosition()
    {
        if (_collider != null)
        {
            float xPos = Randomizer.GetNormalizedRandom() * _collider.bounds.size.x - _collider.bounds.size.x / 2;
            float yPos = Randomizer.GetNormalizedRandom() * _collider.bounds.size.y - _collider.bounds.size.y / 2;
            return new Vector2(xPos, yPos);   
        }
        return new Vector2(0,0);
    }

    void OnMouseDown()
    {
        Vector2 pos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        _hero.SetTarget(pos);
    }
}
