using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class VortexBonus : OneActionBonus
{
    public override GameController.BonusTypes GetBonusType()
    {
        return GameController.BonusTypes.Vortex;
    }

    protected override void AddEffectToPassenger(PassengerSM passenger)
    {
        passenger.AddVortexEffect(Position, Dist);
    }

    public VortexBonus(string bonusName = "vortexBonus")
    {
        InitTTL(bonusName);
        InitDist(bonusName);
        IsPassengersAffected = true;
    }
}