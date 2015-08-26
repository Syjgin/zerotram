using System;
using Assets;
using Assets.Scripts.Math;
using UnityEngine;
using System.Collections;

public class BackgroundManager : MonoBehaviour
{

    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Hero _hero;

    private BoxCollider2D _collider;
	// Use this for initialization
	void Start ()
	{
	    _collider = GetComponent<BoxCollider2D>();
	}
	
	void Update ()
	{
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
	    if (_hero.IsDragging() && hit.collider == _collider)
	    {
	        _hero.UpdatePositionForDrag();
	    }
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
        if(_hero == null)
            return;
        Vector2 pos = GetCurrentMousePosition();
        _hero.SetTarget(pos);
    }

    void OnMouseUp()
    {
        _hero.StopDrag();
    }

    public Vector2 GetCurrentMousePosition()
    {
        return _mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }
}
