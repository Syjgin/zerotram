using System.Collections.Generic;
using System.Diagnostics;
using Assets;
using UnityEngine;

using Debug = UnityEngine.Debug;

public class VortexBonus : AbstractBonus
{
    private float _dist;
    private bool _isFired;
    public override GameController.BonusTypes GetBonusType()
    {
        return GameController.BonusTypes.Ticket;
    }

    public override void HandleTouchUp(Vector2 point)
    {
        if (_isFired)
            return;
        _isFired = true;
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
}