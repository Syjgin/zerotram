using System.Collections.Generic;
using System.Diagnostics;
using Assets;
using UnityEngine;

using Debug = UnityEngine.Debug;

public class SmileBonus : PassengerEffectBonus
{
    public override GameController.BonusTypes GetBonusType()
    {
        return GameController.BonusTypes.Smile;
    }
    
    protected override void AddEffectAfterCheck(PassengerSM passenger)
    {
        passenger.AttackProbability *= DecrementCoef;
    }

    protected override void RemoveEffectAfterCheck(PassengerSM passenger)
    {
        passenger.AttackProbability *= IncrementCoef;
    }
    
    public SmileBonus(string bonusName = "smileBonus")
    {
        InitTTL(bonusName);
        InitCoef(bonusName);
        IsPassengersAffected = true;
    }


}