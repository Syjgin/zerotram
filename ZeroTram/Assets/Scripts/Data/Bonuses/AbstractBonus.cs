using Assets;
using UnityEngine;
using System.Collections.Generic;

public abstract class AbstractBonus :IBonus
{
    protected float TTL;
    private bool _isActive;
    public bool IsActive()
    {
        return _isActive;
    }

    public virtual void Activate()
    {
        _isActive = true;
    }

    public virtual void Deactivate()
    {
        _isActive = false;
    }

    public void DecrementTimer(float delta)
    {
        TTL -= delta;
        if (TTL <= 0)
            Deactivate();
    }

    public abstract GameController.BonusTypes GetBonusType();

    public virtual void AddEffect(PassengerSM passenger)
    {
        
    }

    public virtual void RemoveEffect(PassengerSM passenger)
    {
        
    }

    protected bool IsBonusExist(PassengerSM passenger)
    {
        return passenger.ActiveBonuses.Contains(GetBonusType());
    }

    public virtual List<MovableCharacterSM> HandleClick(Vector2 pos, bool isDouble)
    {
        return new List<MovableCharacterSM>();
    }

    public virtual void HandleTouchUp(Vector2 position)
    {
        
    }
}