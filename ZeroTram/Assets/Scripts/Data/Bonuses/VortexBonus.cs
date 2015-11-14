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
        passenger.AddVortexEffect(StartPoint, Distance);
    }

    public VortexBonus()
    {
        TTL = ConfigReader.GetConfig().GetField("vortexBonus").GetField("TTL").n;
        Distance = ConfigReader.GetConfig().GetField("vortexBonus").GetField("dist").n;
        IsPassengersAffected = true;
    }
}