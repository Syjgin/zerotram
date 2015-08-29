using System;
using Assets;
using Assets.Scripts.Math;
using UnityEngine;
using System.Collections;

public class BackgroundManager : MonoBehaviour
{

    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Hero _hero;
    [SerializeField] private Collider2D _leftDoor;
    [SerializeField] private Collider2D _rightDoor;
    [SerializeField] private Collider2D _centralWayout;
    [SerializeField] private DoorsTimer _timer;

    private const float ColliderOffset = 1.5f;

    private BoxCollider2D _collider;
	// Use this for initialization
	void Awake ()
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
	    if (IsHeroNearWayout(_centralWayout))
	    {
            _hero.SetInWayoutZone(true);
	    }
	    else
	    {
	        if (IsHeroNearWayout(_leftDoor) || IsHeroNearWayout(_rightDoor))
	        {
                _hero.SetInWayoutZone(_timer.IsDoorsOpen);
	        }
	        else
	        {
                _hero.SetInWayoutZone(false);   
	        }
	    }
	}

    public bool IsPassengerNearDoors(Passenger ps)
    {
        Vector2 position = ps.GetPosition();
        if (_leftDoor.OverlapPoint(position) || _rightDoor.OverlapPoint(position))
        {
            return true;
        }
        return false;
    }

    private bool IsHeroNearWayout(Collider2D wayout)
    {
        if (_hero == null)
            return false;
        Vector2 position = _hero.GetPosition();
        return wayout.OverlapPoint(position);
    }

    public Vector2 GetRandomPosition()
    {
        if (_collider != null)
        {
            float xPos = Randomizer.GetNormalizedRandom() * _collider.bounds.size.x - _collider.bounds.size.x * 0.5f;
            float yPos = Randomizer.GetNormalizedRandom() * _collider.bounds.size.y - _collider.bounds.size.y * 0.5f - ColliderOffset;
            return new Vector2(xPos, yPos);   
        }
        return new Vector2(0,0);
    }

    void OnMouseDown()
    {
        if(Time.timeScale == 0)
            return;
        if(_hero == null)
            return;
        Vector2 pos = GetCurrentMousePosition();
        _hero.SetTarget(pos);
    }

    void OnMouseUp()
    {
        _hero.StopDrag();
        GameController.GetInstance().UndragAll();
    }

    public Vector2 GetCurrentMousePosition()
    {
        return _mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }
}
