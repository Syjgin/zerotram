using System.Collections.Generic;
using System.Diagnostics;
using Assets;
using UnityEngine;

public class SnowBonus : PassengerEffectBonus
{
    public class FreezeData
    {
        public Vector2 StartPoint;
        public float Distance;
    }

    private FreezeData _data;
    private Floor _floor;
    private bool _isSnowDeactivated;

    public override GameController.BonusTypes GetBonusType()
    {
        return GameController.BonusTypes.Snow;
    }
    
    protected override void AddEffectAfterCheck(PassengerSM passenger)
    {
        MonobehaviorHandler.GetMonobeharior().GetObject<DoorsTimer>("DoorsTimer").SetPaused(false);
        passenger.ActiveBonuses.Add(GetBonusType());
        if (_data == null)
        {
            _data = new FreezeData();
            _data.Distance = Dist;
            _data.StartPoint = Position;
            _floor = MonobehaviorHandler.GetMonobeharior().GetObject<Floor>("Floor");
            _floor.SnowDrop(_data, true);
        }
        passenger.ActivateFreezeBonus(_data);
    }

    protected override void RemoveEffectAfterCheck(PassengerSM passenger)
    {
        passenger.UnFreeze();
        if (!_isSnowDeactivated)
        {
            _isSnowDeactivated = true;
            _floor.SnowDrop(_data, false);
        }
    }
    
    public SnowBonus(string bonusName = "snowBonus")
    {
        InitTTL(bonusName);
        InitDist(bonusName);
        IsPassengersAffected = true;
    }
}