using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

using Debug = UnityEngine.Debug;

public class MagnetBonus : PassengerEffectBonus
{
    public override GameController.BonusTypes GetBonusType()
    {
        return GameController.BonusTypes.Magnet;
    }

    protected override void AddEffectAfterCheck(PassengerSM passenger)
    {
        passenger.TurnOnMagnet(Dist);
    }

    protected override void RemoveEffectAfterCheck(PassengerSM passenger)
    {
        passenger.TurnOffMagnet();
    }

    public MagnetBonus(string bonusName = "magnetBonus")
    {
        InitTTL(bonusName);
        InitDist(bonusName);
        IsPassengersAffected = true;
    }


}