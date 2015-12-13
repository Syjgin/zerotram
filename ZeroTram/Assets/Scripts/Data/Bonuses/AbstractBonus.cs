using Assets;
using UnityEngine;
using System.Collections.Generic;

public abstract class AbstractBonus :IBonus
{
    protected float TTL;
    protected bool _isActive;
    protected Vector2 Position;

    public void SetPosition(Vector2 pos)
    {
        Position = pos;
    }

    protected float Dist;
    protected float DecrementCoef;
    protected float IncrementCoef;


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

    public virtual void DecrementTimer(float delta)
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
    
    public int GetTTL()
    {
        return (int)TTL;
    }

    protected void InitTTL(string bonusName)
    {
        TTL = ConfigReader.GetConfig().GetField("bonuses").GetField(bonusName).GetField("TTL").n;
    }

    protected void InitCoef(string bonusName)
    {
        IncrementCoef = ConfigReader.GetConfig().GetField("bonuses").GetField(bonusName).GetField("incrementCoef").n;
        DecrementCoef = 1 / IncrementCoef;
    }

    protected void InitDist(string bonusName)
    {
        Dist = ConfigReader.GetConfig().GetField("bonuses").GetField(bonusName).GetField("dist").n;
    }


}