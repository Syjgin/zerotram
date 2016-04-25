using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Floor : MonoBehaviour
{

    [SerializeField] private Camera _mainCamera;
    [SerializeField] private ConductorSM _hero;
    [SerializeField] private List<BoxCollider2D> _doors;
    [SerializeField] private BoxCollider2D _centralWayout;
    [SerializeField] private DoorsTimer _timer;
    [SerializeField] private BonusTimer _bonusTimer;
    [SerializeField] private GameObject _snowDropGameObject;
    [SerializeField] private PolygonCollider2D _polygonCollider2D;
    
    private const float HeroOffset = 0.6f;

    private float _normalizedMax;

    private List<GameObject> _spawnedDrops; 

	// Use this for initialization
	void Awake ()
	{
        _spawnedDrops = new List<GameObject>();
	    _normalizedMax = _polygonCollider2D.bounds.max.y - _polygonCollider2D.bounds.min.y;
	}

    void Update()
    {
        if (IsHeroNearCentralWayout())
        {
            _hero.IsInWayoutZone = true;
        }
        else
        {
            bool anyDoorIsReachable = false;
            for (int i = 0; i < _doors.Count; i++)
            {
                if (IsHeroNearWayout(_doors[i], false) && _timer.IsDoorOpenedByNumber(i))
                {
                    anyDoorIsReachable = true;
                    break;
                }
            }
            _hero.IsInWayoutZone = anyDoorIsReachable;
        }
    }

    private bool IsHeroNearCentralWayout()
    {
        return IsHeroNearWayout(_centralWayout, true);
    }

    private bool IsHeroNearWayout(Collider2D wayout, bool central)
    {
        if (_hero == null)
            return false;
        Vector2 position = _hero.transform.position;
        if (central)
            position.y -= 0.7f;
        Vector3 position2check = new Vector3(position.x, position.y, wayout.transform.position.z);
        return wayout.OverlapPoint(position2check);
    }

    public ConductorSM GetHero()
    {
        return _hero;
    }

    public Vector2 GetRandomPosition()
    {
        if (_polygonCollider2D != null)
        {
            float xPos = Randomizer.GetNormalizedRandom() * _polygonCollider2D.bounds.size.x - _polygonCollider2D.bounds.size.x * 0.5f;
            float yPos = Randomizer.GetNormalizedRandom() * _polygonCollider2D.bounds.size.y - _polygonCollider2D.bounds.size.y * 0.5f;
            Vector3 target = new Vector3(xPos, yPos);
            NormalizePosition(ref target, true);
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
        _hero.StopDrag(false);
    }

    public bool NormalizePosition(ref Vector3 position, bool withOffset)
    {
        if (withOffset)
            position.y += HeroOffset;
        if (!_polygonCollider2D.OverlapPoint(position))
        {
            position = _polygonCollider2D.bounds.ClosestPoint(position);
            return false;
        }
        return true;
    }

    public Vector2 GetCurrentMousePosition(bool withOffset = true)
    {
        Vector3 target = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        NormalizePosition(ref target, withOffset);
        return target;
    }

    public bool GetCurrentMousePosition(ref Vector3 position, bool withOffset = true)
    {
        position = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        return NormalizePosition(ref position, withOffset);
    }

    public bool IsPassengerNearDoors(PassengerSM ps)
    {
        Vector2 position = ps.transform.position;
        foreach (var door in _doors)
        {
            Vector3 position2check = new Vector3(position.x, position.y, door.transform.position.z);
            if (door.OverlapPoint(position2check))
                return true;
        }
        return false;
    }
    
    public GameObject GetPassengerDoor(PassengerSM passenger)
    {
        Vector2 position = passenger.transform.position;
        foreach (var door in _doors)
        {
            Vector3 position2check = new Vector3(position.x, position.y, door.transform.position.z);
            if (door.OverlapPoint(position2check))
                return door.gameObject;
        }
        return null;
    }

    public void ChangeWayoutSquare(float coef)
    {
        _centralWayout.size *= coef;
    }

    public float CalculateLocalScaleForMovable(MovableCharacterSM character)
    {
        float posY = character.transform.position.y;
        posY -= _polygonCollider2D.bounds.min.y;
        float scalePercent = 1 + 0.3f*(1 - posY/_normalizedMax);
        return scalePercent;
    }

    public void SnowDrop(SnowBonus.FreezeData freezeData, bool isVisible)
    {
        if (isVisible)
        {
            for (int deg = 0; deg < 360; deg += 20)
            {
                float radians = deg * Mathf.Deg2Rad;
                float xPos = freezeData.StartPoint.x + freezeData.Distance * Mathf.Cos(radians);
                float yPos = freezeData.StartPoint.y + freezeData.Distance * Mathf.Sin(radians);

                Vector3 dropPosition = new Vector3(xPos, yPos, 0);
                NormalizePosition(ref dropPosition, false);
                GameObject drop = Instantiate(_snowDropGameObject);
                drop.transform.position = new Vector3(xPos, yPos, 0);
                _spawnedDrops.Add(drop);
            }
        }
        else
        {
            for (int i = 0; i < _spawnedDrops.Count; i++)
            {
                Destroy(_spawnedDrops[i]);
            }
            _spawnedDrops.Clear();
        }
    }


}
