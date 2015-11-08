using System.Collections.Generic;
using System.Diagnostics;
using Assets;
using UnityEngine;

public class VortexBonus : PointBonus
{
    private float _dist;
    public override GameController.BonusTypes GetBonusType()
    {
        return GameController.BonusTypes.Vortex;
    }

    public override void HandleTouchUp(Vector2 point)
    {
        List<PassengerSM> passengersNear = GameController.GetInstance().AllPassengersInDist(point, _dist);
        foreach (var passengerSm in passengersNear)
        {
            passengerSm.AddVortexEffect(point, _dist);
        }
        Deactivate();
    }

    public VortexBonus()
    {
        TTL = ConfigReader.GetConfig().GetField("vortexBonus").GetField("TTL").n;
        _dist = ConfigReader.GetConfig().GetField("vortexBonus").GetField("dist").n;
    }

    protected override void AddEffectAfterCheck(PassengerSM passenger)
    {
        
    }

    protected override void RemoveEffectAfterCheck(PassengerSM passenger)
    {
        
    }
}