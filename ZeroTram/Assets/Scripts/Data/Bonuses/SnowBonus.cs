using System.Collections.Generic;
using System.Diagnostics;
using Assets;
using UnityEngine;

using Debug = UnityEngine.Debug;

public class SnowBonus : PassengerEffectBonus
{
    private float _dist;
    private bool _isFired;
    private Vector2 _startPoint; 
    public override GameController.BonusTypes GetBonusType()
    {
        return GameController.BonusTypes.Snow;
    }

    public override void AddEffect(PassengerSM passenger)
    {
        if (_isFired)
        {
            MonobehaviorHandler.GetMonobeharior().GetObject<DoorsTimer>("Spawner").SetPaused(false);
            passenger.ActiveBonuses.Add(GetBonusType());
            float currentDist = ((Vector2)passenger.transform.position - _startPoint).sqrMagnitude;
            if (currentDist < _dist)
            {
                passenger.Freeze();
            }
        }
    }

    protected override void AddEffectAfterCheck(PassengerSM passenger)
    {   
    }

    protected override void RemoveEffectAfterCheck(PassengerSM passenger)
    {
        passenger.UnFreeze();
    }

    public override void HandleTouchUp(Vector2 point)
    {
        if (_isFired)
            return;
        _isFired = true;
        _startPoint = point;
        TTL = ConfigReader.GetConfig().GetField("snowBonus").GetField("TTL").n;
    }

    public SnowBonus()
    {
        TTL = ConfigReader.GetConfig().GetField("snowBonus").GetField("TTL").n;
        _dist = ConfigReader.GetConfig().GetField("snowBonus").GetField("dist").n;
        _startPoint = new Vector2();
    }

    public override void DecrementTimer(float delta)
    {
        base.DecrementTimer(delta);
        if(IsActive() && _isFired)
            GameController.GetInstance().BonusEffectToPassengers(this, true);
    }
}