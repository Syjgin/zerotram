using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets;
using Assets.Scripts.Math;

public class BonusTimer : MonoBehaviour
{
    private List<IBonus> _activeBonuses;
    private List<UnknownDrop> _droppedBonuses;
    [SerializeField] private GameObject _unknownDropPrefab;
    [SerializeField] private CurrentBonusTimer _currentBonusTimer;
    [SerializeField] private List<MegaBonusButton> _megaBonusButtons;
    [SerializeField] private Floor _floor;

	void Awake () {
        if(_activeBonuses == null)
            _activeBonuses = new List<IBonus>();
        if(_droppedBonuses == null)
            _droppedBonuses = new List<UnknownDrop>();	
	}

    public void SetMegaBonus(GameController.BonusTypes bonusType)
    {
        IBonus megaBonus = null;
        switch (bonusType)
        {
            case GameController.BonusTypes.Wheel:
                megaBonus = new WheelMegaBonus();
                break;
            case GameController.BonusTypes.Ticket:
                megaBonus = new TicketMegaBonus();
                break;
            case GameController.BonusTypes.Boot:
                megaBonus = new BootMegaBonus();
                break;
            case GameController.BonusTypes.Magnet:
                megaBonus = new MagnetMegaBonus();
                break;
            case GameController.BonusTypes.Smile:
                megaBonus = new SmileMegaBonus();
                break;
            case GameController.BonusTypes.AntiHare:
                megaBonus = new AntiHareMegaBonus();
                break;
            case GameController.BonusTypes.SandGlass:
                megaBonus = new SandGlassMegaBonus();
                break;
            case GameController.BonusTypes.Vortex:
                megaBonus = new VortexBonus();
                break;
            case GameController.BonusTypes.Snow:
                megaBonus = new SnowBonus();
                break;
            case GameController.BonusTypes.Wrench:
                break;
            case GameController.BonusTypes.Cogwheel:
                break;
            case GameController.BonusTypes.Heal:
                megaBonus = new HealMegaBonus();
                break;
            case GameController.BonusTypes.Clew:
                megaBonus = new ClewMegaBonus();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        _megaBonusButtons[0].SetMegaBonus(megaBonus);
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
        ActivateBonus(drop.Bonus);
        foreach (var droppedBonus in _droppedBonuses)
        {
            Destroy(droppedBonus.gameObject);    
        }
        _droppedBonuses.Clear();
    }

    public void ActivateBonus(IBonus bonus)
    {
        bonus.SetPosition(_floor.GetCurrentMousePosition());
        bonus.Activate();
        _currentBonusTimer.Activate(bonus);
        _activeBonuses.Add(bonus);
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
