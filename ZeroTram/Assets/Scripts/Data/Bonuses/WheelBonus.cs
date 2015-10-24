using System.Diagnostics;
using Assets;
using UnityEngine;

using Debug = UnityEngine.Debug;

public class WheelBonus : AbstractBonus
{
    public override GameController.BonusTypes GetBonusType()
    {
        return GameController.BonusTypes.Wheel;
    }

    public override void Activate()
    {
        base.Activate();
        GameController.GetInstance().BonusEffectToPassengers(this, true);
    }

    public override void Deactivate()
    {
        GameController.GetInstance().BonusEffectToPassengers(this, false);
    }

    public override void AddEffect(PassengerSM passenger)
    {
        if (passenger.ActiveBonuses.Contains(GetBonusType()))
            return;
        passenger.ActiveBonuses.Add(GetBonusType());
        passenger.DragChangeStatePeriod *= 2;
    }

    public override void RemoveEffect(PassengerSM passenger)
    {
        if (!passenger.ActiveBonuses.Contains(GetBonusType()))
            return;
        passenger.ActiveBonuses.Remove(GetBonusType());
        passenger.DragChangeStatePeriod *= 0.5f;
    }

    public WheelBonus()
    {
        TTL = ConfigReader.GetConfig().GetField("wheelBonus").GetField("TTL").n;
    }
}