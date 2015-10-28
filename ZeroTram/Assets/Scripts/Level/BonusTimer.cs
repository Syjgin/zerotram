using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets;

public class BonusTimer : MonoBehaviour
{
    private List<IBonus> _activeBonuses;  

	void Awake () {
        if(_activeBonuses == null)
            _activeBonuses = new List<IBonus>();	
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
