using System.Collections.Generic;
using System.Diagnostics;
using Assets;
using UnityEngine;

public class ClewMegaBonus : OneActionBonus
{
    public override GameController.BonusTypes GetBonusType()
    {
        return GameController.BonusTypes.Clew;
    }
    
    protected override void AddEffectToPassenger(PassengerSM passenger)
    {
        ClewBonus.ReplacePassenger(passenger);
    }

    public ClewMegaBonus()
    {
        TTL = 0;
        InitDist("clewMegaBonus");
        IsPassengersAffected = true;
    }
}