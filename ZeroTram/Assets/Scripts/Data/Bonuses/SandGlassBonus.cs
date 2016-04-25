using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

using Debug = UnityEngine.Debug;

public class SandGlassBonus : PassengerEffectBonus
{
    public override GameController.BonusTypes GetBonusType()
    {
        return GameController.BonusTypes.SandGlass;
    }
    
    protected override void AddEffectAfterCheck(PassengerSM passenger)
    {
        passenger.Velocity *= DecrementCoef;
    }

    protected override void RemoveEffectAfterCheck(PassengerSM passenger)
    {
        passenger.Velocity *= IncrementCoef;
    }
    
    public SandGlassBonus(string bonusName = "sandglassBonus")
    {
        InitTTL(bonusName);
        InitCoef(bonusName);
        IsPassengersAffected = true;
    }


}