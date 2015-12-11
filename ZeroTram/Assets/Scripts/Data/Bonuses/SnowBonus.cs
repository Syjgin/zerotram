using System.Collections.Generic;
using System.Diagnostics;
using Assets;
using UnityEngine;

using Debug = UnityEngine.Debug;

public class SnowBonus : PointBonus
{
    public override GameController.BonusTypes GetBonusType()
    {
        return GameController.BonusTypes.Snow;
    }
    
    protected override void AddEffectAfterCheck(PassengerSM passenger)
    {
        MonobehaviorHandler.GetMonobeharior().GetObject<DoorsTimer>("DoorsTimer").SetPaused(false);
        passenger.ActiveBonuses.Add(GetBonusType());
        float currentDist = ((Vector2)passenger.transform.position - StartPoint).sqrMagnitude;
        if (currentDist < Dist)
        {
            passenger.Freeze();
        }
    }

    protected override void RemoveEffectAfterCheck(PassengerSM passenger)
    {
        passenger.UnFreeze();
    }
    
    public SnowBonus(string bonusName = "snowBonus")
    {
        InitTTL(bonusName);
        InitDist(bonusName);
    }
}