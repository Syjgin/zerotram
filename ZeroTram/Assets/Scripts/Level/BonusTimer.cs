﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets;

public class BonusTimer : MonoBehaviour
{
    private List<IBonus> _activeBonuses;  

	void Awake () {
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
	                bonus.Deactivate();
                    _activeBonuses.RemoveAt(i);
                }
	        }
	    }

	}
}