using System.Collections.Generic;
using System.Diagnostics;
using Assets;
using UnityEngine;

using Debug = UnityEngine.Debug;

public class WrenchBonus : PassengerEffectBonus
{
    public override GameController.BonusTypes GetBonusType()
    {
        return GameController.BonusTypes.Wrench;
    }
    
    protected override void AddEffectAfterCheck(PassengerSM passenger)
    {
        passenger.ApplyWrenchBonus(true);
    }

    protected override void RemoveEffectAfterCheck(PassengerSM passenger)
    {
        passenger.ApplyWrenchBonus(false);
    }
    
    public WrenchBonus()
    {
        TTL = ConfigReader.GetConfig().GetField("wrenchBonus").GetField("TTL").n;
        IsPassengersAffected = true;
    }


}