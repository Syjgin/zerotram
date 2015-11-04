using System.Collections.Generic;
using System.Diagnostics;
using Assets;
using UnityEngine;

using Debug = UnityEngine.Debug;

public class SnowBonus : PointBonus
{
    private float _dist;

    public override GameController.BonusTypes GetBonusType()
    {
        return GameController.BonusTypes.Snow;
    }
    
    protected override void AddEffectAfterCheck(PassengerSM passenger)
    {
        MonobehaviorHandler.GetMonobeharior().GetObject<DoorsTimer>("Spawner").SetPaused(false);
        passenger.ActiveBonuses.Add(GetBonusType());
        float currentDist = ((Vector2)passenger.transform.position - StartPoint).sqrMagnitude;
        if (currentDist < _dist)
        {
            passenger.Freeze();
        }
    }

    protected override void RemoveEffectAfterCheck(PassengerSM passenger)
    {
        passenger.UnFreeze();
    }
    
    public SnowBonus()
    {
        TTL = ConfigReader.GetConfig().GetField("snowBonus").GetField("TTL").n;
        _dist = ConfigReader.GetConfig().GetField("snowBonus").GetField("dist").n;
    }
}