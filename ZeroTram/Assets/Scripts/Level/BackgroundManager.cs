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
	    if (IsHeroNearWayout(_centralWayout))
	    {
            _hero.SetInWayoutZone(true);
	    }
	    else
	    {
	        if (IsHeroNearWayout(_leftDoor) || IsHeroNearWayout(_rightDoor))
	        {
	            if(_timer.IsDoorsOpen)
                    _hero.SetInWayoutZone(true);
                else
                    _hero.SetInWayoutZone(true);
	        }
	        else
	        {
                _hero.SetInWayoutZone(false);   
	        }
	    }
	}

    private bool IsHeroNearWayout(Collider2D wayout)
    {
        float size = wayout.bounds.size.x;
        float radius = Mathf.Sqrt(2 * size * size) * 0.5f;
        if(_hero == null)
            return false;
        float dist = (_hero.transform.position - wayout.bounds.center).sqrMagnitude;
        return dist < radius;
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
        GameController.GetInstance().UndragAll();
    }

    public Vector2 GetCurrentMousePosition()
    {
        return _mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }
}
