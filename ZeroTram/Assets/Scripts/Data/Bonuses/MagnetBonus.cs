using System.Collections.Generic;
using System.Diagnostics;
using Assets;
using UnityEngine;

using Debug = UnityEngine.Debug;

public class MagnetBonus : PassengerEffectBonus
{
    private float _dist;
    public override GameController.BonusTypes GetBonusType()
    {
        return GameController.BonusTypes.Magnet;
    }

    protected override void AddEffectAfterCheck(PassengerSM passenger)
    {
        passenger.TurnOnMagnet(_dist);
    }

    protected override void RemoveEffectAfterCheck(PassengerSM passenger)
    {
        passenger.TurnOffMagnet();
    }

    public MagnetBonus()
    {
        TTL = ConfigReader.GetConfig().GetField("magnetBonus").GetField("TTL").n;
        _dist = ConfigReader.GetConfig().GetField("magnetBonus").GetField("dist").n;
        IsPassengersAffected = true;
    }


}