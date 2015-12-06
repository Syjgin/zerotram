using System.Collections.Generic;
using System.Diagnostics;
using Assets;
using UnityEngine;

public abstract class PointBonus : PassengerEffectBonus
{
    protected bool IsFired;
    protected Vector2 StartPoint; 
    
    public override void AddEffect(PassengerSM passenger)
    {
        if (IsFired && !passenger.ActiveBonuses.Contains(GetBonusType()))
        {
            passenger.ActiveBonuses.Add(GetBonusType());
            AddEffectAfterCheck(passenger);
        }
    }
    
    public override void HandleTouchUp(Vector2 point)
    {
        if (IsFired)
            return;
        IsFired = true;
        StartPoint = point;
    }

    protected PointBonus()
    {
        StartPoint = new Vector2();
    }

    public override void DecrementTimer(float delta)
    {
        if(!IsFired)
            return;
        base.DecrementTimer(delta);
        if(_isActive && IsFired)
            GameController.GetInstance().BonusEffectToPassengers(this, true);
    }
}