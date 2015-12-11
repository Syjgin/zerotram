using System.Collections.Generic;
using System.Diagnostics;
using Assets;
using UnityEngine;

public class VortexBonus : OneActionBonus
{
    public override GameController.BonusTypes GetBonusType()
    {
        return GameController.BonusTypes.Vortex;
    }

    protected override void AddEffectToPassenger(PassengerSM passenger)
    {
        passenger.AddVortexEffect(StartPoint, Dist);
    }

    public VortexBonus(string bonusName = "vortexBonus")
    {
        InitTTL(bonusName);
        InitDist(bonusName);
        IsPassengersAffected = true;
    }
}