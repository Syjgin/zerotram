using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets;

public class BonusTimer : MonoBehaviour
{
    private List<IBonus> _activeBonuses;
    private List<UnknownDrop> _droppedBonuses;
    [SerializeField] private GameObject _unknownDropPrefab;
    [SerializeField] private CurrentBonusTimer _currentBonusTimer;

	void Awake () {
        if(_activeBonuses == null)
            _activeBonuses = new List<IBonus>();
        if(_droppedBonuses == null)
            _droppedBonuses = new List<UnknownDrop>();	
	}

    public void ActivateBonusByNumber(int number)
    {
        if (_activeBonuses == null)
        {
            _activeBonuses = new List<IBonus>();
        }
        IBonus activatedBonus = GameController.GetInstance().ActivateBonusByNumber(number);
        _activeBonuses.Add(activatedBonus);
    }

    public void DropBonus(IBonus bonus, Vector3 coords)
    {
        if (_droppedBonuses == null)
        {
            _droppedBonuses = new List<UnknownDrop>();
        }
        coords.z = -5;
        GameObject instantiatedDrop = GameObject.Instantiate(_unknownDropPrefab, coords, Quaternion.identity) as GameObject;
        if (instantiatedDrop != null)
        {
            UnknownDrop newDrop = instantiatedDrop.GetComponent<UnknownDrop>();
            newDrop.InitWithBonus(bonus);
            _droppedBonuses.Add(newDrop);
        }
    }

    public void ActivateDrop(UnknownDrop drop)
    {
        if (!_droppedBonuses.Contains(drop))
        {
            return;
        }
        drop.Bonus.Activate();
        _currentBonusTimer.Activate(drop.Bonus);
        _activeBonuses.Add(drop.Bonus);
        foreach (var droppedBonus in _droppedBonuses)
        {
            Destroy(droppedBonus.gameObject);    
        }
        _droppedBonuses.Clear();
    }

    public bool IsBonusActiveByType(GameController.BonusTypes bonusType)
    {
        foreach (var activeBonus in _activeBonuses)
        {
            if (activeBonus.GetBonusType().Equals(bonusType) && activeBonus.IsActive())
                return true;
        }
        return false;
    }
    
    public bool IsAnyBonusActive()
    {
        return _activeBonuses.Count > 0;
    }

    public List<MovableCharacterSM> HandleClick(Vector2 pos, bool doubleClick)
    {
        List<MovableCharacterSM> affectedCharacters = new List<MovableCharacterSM>();
        foreach (var activeBonus in _activeBonuses)
        {
            List<MovableCharacterSM> characters = activeBonus.HandleClick(pos, doubleClick);
            foreach (var movableCharacterSm in characters)
            {
                if(!affectedCharacters.Contains(movableCharacterSm))
                    affectedCharacters.Add(movableCharacterSm);
            }
        }
        return affectedCharacters;
    }

    public void HandleTouchUp(Vector2 pos)
    {
        foreach (var activeBonus in _activeBonuses)
        {
            activeBonus.HandleTouchUp(pos);
        }
    }

    public void AddBonusEffectToSpawnedPassenger(PassengerSM passenger)
    {
        foreach (var activeBonus in _activeBonuses)
        {
            activeBonus.AddEffect(passenger);
        }
    }

	void FixedUpdate () {
	    for(int i = 0; i < _activeBonuses.Count; i++)
	    {
	        if (i < _activeBonuses.Count)
	        {
	            IBonus bonus = _activeBonuses[i];
                bonus.DecrementTimer(Time.deltaTime);
	            if (!bonus.IsActive())
	            {
                    _activeBonuses.RemoveAt(i);
                }
	        }
	    }

	}
}
