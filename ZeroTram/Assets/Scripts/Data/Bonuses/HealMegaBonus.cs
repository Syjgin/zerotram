using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class HealMegaBonus : OneActionBonus
{
    public override GameController.BonusTypes GetBonusType()
    {
        return GameController.BonusTypes.Heal;
    }
    
    public override void AddEffectToConductor(ConductorSM conductor)
    {
        conductor.AddDamageValue(-conductor.GetInitialLifes());
    }

    protected override void AddEffectToPassenger(PassengerSM passenger)
    {
        passenger.AddDamageValue(-passenger.GetInitialLifes());
    }

    public HealMegaBonus()
    {
        TTL = 0;
        InitDist("healMegaBonus");
        IsConductorAffected = true;
        IsPassengersAffected = true;
    }
}