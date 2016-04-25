using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

using Debug = UnityEngine.Debug;

public class AntiHareBonus : PassengerEffectBonus
{
    public override GameController.BonusTypes GetBonusType()
    {
        return GameController.BonusTypes.AntiHare;
    }

    protected override void AddEffectAfterCheck(PassengerSM passenger)
    {
        passenger.RecalculateTicketProbability(IncrementCoef, true);
    }

    protected override void RemoveEffectAfterCheck(PassengerSM passenger)
    {
        passenger.RecalculateTicketProbability(DecrementCoef, true);
    }

    public AntiHareBonus(string bonusName= "antiHareBonus")
    {
        InitTTL(bonusName);
        InitCoef(bonusName);
        IsPassengersAffected = true;
    }


}