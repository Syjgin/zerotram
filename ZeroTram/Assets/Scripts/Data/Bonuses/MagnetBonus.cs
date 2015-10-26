using System.Collections.Generic;
using System.Diagnostics;
using Assets;
using UnityEngine;

using Debug = UnityEngine.Debug;

public class MagnetBonus : AbstractBonus
{
    private float _dist;
    public override GameController.BonusTypes GetBonusType()
    {
        return GameController.BonusTypes.Magnet;
    }
    
    public override void Deactivate()
    {
    }

    public override void AddEffect(PassengerSM passenger)
    {
        passenger.TurnOnMagnet(_dist);
    }

    public override void RemoveEffect(PassengerSM passenger)
    {
        passenger.TurnOffMagnet();
    }

    public MagnetBonus()
    {
        TTL = ConfigReader.GetConfig().GetField("magnetBonus").GetField("TTL").n;
        _dist = ConfigReader.GetConfig().GetField("magnetBonus").GetField("dist").n;
    }


}