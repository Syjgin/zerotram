using System.Diagnostics;
using Assets;
using UnityEngine;

using Debug = UnityEngine.Debug;

public class WheelBonus : PassengerEffectBonus
{
    public override GameController.BonusTypes GetBonusType()
    {
        return GameController.BonusTypes.Wheel;
    }
    
    protected override void AddEffectAfterCheck(PassengerSM passenger)
    {
        passenger.DragChangeStatePeriod *= IncrementCoef;
    }

    protected override void RemoveEffectAfterCheck(PassengerSM passenger)
    {
        passenger.DragChangeStatePeriod *= DecrementCoef;
    }

    public WheelBonus(string bonusName = "wheelBonus")
    {
        InitTTL(bonusName);
        InitCoef(bonusName);
        IsPassengersAffected = true;
    }
}