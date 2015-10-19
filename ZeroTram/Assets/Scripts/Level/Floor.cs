using System;
using Assets;
using Assets.Scripts.Math;
using UnityEngine;
using System.Collections;

public class Floor : MonoBehaviour
{

    [SerializeField] private Camera _mainCamera;
    [SerializeField] private ConductorSM _hero;
    [SerializeField] private Collider2D _leftDoor;
    [SerializeField] private Collider2D _rightDoor;
    [SerializeField] private Collider2D _centralWayout;
    [SerializeField] private DoorsTimer _timer;


    private const float ColliderOffset = 1.4f;
    private const float HeroOffset = 0.6f;

    private BoxCollider2D _collider;
	// Use this for initialization
	void Awake ()
	{
	    _collider = GetComponent<BoxCollider2D>();
	}

    void Update()
    {
        if (IsHeroNearCentralWayout())
        {
            _hero.IsInWayoutZone = true;
        }
        else
        {
            _hero.IsInWayoutZone = (IsHeroNearDoors() && _timer.IsDoorsOpen);
        }
    }

    private bool IsHeroNearCentralWayout()
    {
        return IsHeroNearWayout(_centralWayout, true);
    }

    private bool IsHeroNearDoors()
    {
        return IsHeroNearWayout(_leftDoor, false) || IsHeroNearWayout(_rightDoor, false);
    }

    private bool IsHeroNearWayout(Collider2D wayout, bool central)
    {
        if (_hero == null)
            return false;
        Vector2 position = _hero.transform.position;
        if (central)
            position.y -= 0.7f;
        return wayout.OverlapPoint(position);
    }

    public ConductorSM GetHero()
    {
        return _hero;
    }

    public Vector2 GetRandomPosition()
    {
        if (_collider != null)
        {
            float xPos = Randomizer.GetNormalizedRandom() * _collider.bounds.size.x - _collider.bounds.size.x * 0.5f;
            float yPos = Randomizer.GetNormalizedRandom() * _collider.bounds.size.y - _collider.bounds.size.y * 0.5f - ColliderOffset;
            Vector2 target = new Vector2(xPos, yPos);
            if (GameController.GetInstance().IsPlaceFree(target))
            {
                return new Vector2(xPos, yPos);      
            }
        }
        return new Vector2(0,0);
    }

    public void OnMouseDown()
    {
        if (Time.timeScale == 0)
            return;
        if (_hero == null)
            return;
        Vector2 pos = GetCurrentMousePosition();
        PassengerSM passengerNearClick = GameController.GetInstance().GetPassengerNearClick(pos);
        if (passengerNearClick != null)
        {
            passengerNearClick.HandleClick();
            return;
        }
        _hero.SetTarget(pos);
    }

    public void DoubleClick()
    {
        Vector2 pos = GetCurrentMousePosition();
        PassengerSM passengerNearClick = GameController.GetInstance().GetPassengerNearClick(pos);
        if (passengerNearClick != null)
        {
            passengerNearClick.HandleDoubleClick();
        }
    }

    public void OnMouseUp()
    {
        _hero.StopDrag();
    }

    public Vector2 GetCurrentMousePosition()
    {
        Vector2 target = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        if (target.y > _collider.bounds.max.y)
            target.y = _collider.bounds.max.y;
        if (target.y < _collider.bounds.min.y)
            target.y = _collider.bounds.min.y;
        target.y += HeroOffset;
        return target;
    }

    public bool IsPassengerNearDoors(PassengerSM ps)
    {
        Vector2 position = ps.transform.position;
        position.y += ColliderOffset;
        if (_leftDoor.OverlapPoint(position) || _rightDoor.OverlapPoint(position))
        {
            return true;
        }
        return false;
    }
    
    public GameObject GetPassengerDoor(PassengerSM passenger)
    {
        Vector2 position = passenger.transform.position;
        position.y += ColliderOffset;
        if (_leftDoor.OverlapPoint(position))
        {
            return _leftDoor.gameObject;
        }
        if (_rightDoor.OverlapPoint(position))
        {
            return _rightDoor.gameObject;
        }
        return null;
    }
}
