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
    
    public override void AddEffect(PassengerSM passenger)
    {
        if (IsEffectAdditionPossible(passenger))
            passenger.TurnOnMagnet(_dist);
    }

    public override void RemoveEffect(PassengerSM passenger)
    {
        if (IsEffectRemovingPossible(passenger))
            passenger.TurnOffMagnet();
    }

    public MagnetBonus()
    {
        TTL = ConfigReader.GetConfig().GetField("magnetBonus").GetField("TTL").n;
        _dist = ConfigReader.GetConfig().GetField("magnetBonus").GetField("dist").n;
    }


}