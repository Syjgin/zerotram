using System;
using Assets;
using Assets.Scripts.Math;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Floor : MonoBehaviour
{

    [SerializeField] private Camera _mainCamera;
    [SerializeField] private ConductorSM _hero;
    [SerializeField] private BoxCollider2D _leftDoor;
    [SerializeField] private BoxCollider2D _rightDoor;
    [SerializeField] private BoxCollider2D _centralWayout;
    [SerializeField] private DoorsTimer _timer;
    [SerializeField] private BonusTimer _bonusTimer;

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
        OnMouseDown(false);
    }

    private void OnMouseDown(bool doubleClick)
    {
        if (Time.timeScale == 0)
            return;
        if (_hero == null)
            return;
        Vector2 pos = GetCurrentMousePosition();
        List<MovableCharacterSM> affectedCharacters = new List<MovableCharacterSM>();
        if (MonobehaviorHandler.GetMonobeharior()
            .GetObject<BonusTimer>("bonusTimer").IsAnyBonusActive())
        {
            affectedCharacters = MonobehaviorHandler.GetMonobeharior()
                .GetObject<BonusTimer>("bonusTimer").HandleClick(pos, doubleClick);
        }
        PassengerSM passengerNearClick = GameController.GetInstance().GetPassengerNearClick(pos);
        if (passengerNearClick != null && !affectedCharacters.Contains(passengerNearClick))
        {
            if(doubleClick)
                passengerNearClick.HandleDoubleClick();
            else
                passengerNearClick.HandleClick();
            return;
        }
        if(!doubleClick)
            _hero.SetTarget(pos);
    }

    public void DoubleClick()
    {
        OnMouseDown(true);
    }

    public void OnMouseUp()
    {
        _hero.StopDrag();
    }

    public void NormalizePosition(ref Vector3 position)
    {
        if (position.y > _collider.bounds.max.y)
            position.y = _collider.bounds.max.y;
        if (position.y < _collider.bounds.min.y)
            position.y = _collider.bounds.min.y;
        if (position.x > _collider.bounds.max.x)
            position.x = _collider.bounds.max.x;
        if (position.x < _collider.bounds.min.x)
            position.x = _collider.bounds.min.x;
        position.y += HeroOffset;
    }

    public Vector2 GetCurrentMousePosition(bool withOffset = true)
    {
        Vector2 target = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        if (target.y > _collider.bounds.max.y)
            target.y = _collider.bounds.max.y;
        if (target.y < _collider.bounds.min.y)
            target.y = _collider.bounds.min.y;
        if(withOffset)
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

    public void ChangeWayoutSquare(float coef)
    {
        _centralWayout.size *= coef;
    }
}
