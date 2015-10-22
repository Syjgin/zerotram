using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets;

public class BonusTimer : MonoBehaviour
{
    private List<IBonus> _activeBonuses;  

	void Start () {
        _activeBonuses = new List<IBonus>();	
	}

    void ActivateBonusByNumber(int number)
    {
        IBonus activatedBonus = GameController.GetInstance().ActivateBonusByNumber(number);
        _activeBonuses.Add(activatedBonus);
    }

	void FixedUpdate () {
	    for(int i = 0; i < _activeBonuses.Count; i++)
	    {
	        if (i < _activeBonuses.Count)
	        {
	            IBonus bonus = _activeBonuses[i];
                bonus.DecrementTimer(Time.deltaTime);
	            if (!bonus.IsActive())
	                _activeBonuses.RemoveAt(i);
	        }
	    }

	}
}
