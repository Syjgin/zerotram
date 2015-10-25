using System.Diagnostics;
using Assets;
using UnityEngine;

using Debug = UnityEngine.Debug;

public class WheelBonus : AbstractBonus
{
    private float _incrementCoef;
    private float _decrementCoef;

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
        if (IsBonusExist(passenger))
            return;
        passenger.ActiveBonuses.Add(GetBonusType());
        passenger.DragChangeStatePeriod *= _incrementCoef;
    }

    public override void RemoveEffect(PassengerSM passenger)
    {
        if (!IsBonusExist(passenger))
            return;
        passenger.ActiveBonuses.Remove(GetBonusType());
        passenger.DragChangeStatePeriod *= _decrementCoef;
    }

    public WheelBonus()
    {
        TTL = ConfigReader.GetConfig().GetField("wheelBonus").GetField("TTL").n;
        _incrementCoef = ConfigReader.GetConfig().GetField("wheelBonus").GetField("incrementCoef").n;
        _decrementCoef = 1/_incrementCoef;
    }
}