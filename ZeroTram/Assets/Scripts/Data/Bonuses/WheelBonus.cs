using System.Diagnostics;
using Assets;
using UnityEngine;

using Debug = UnityEngine.Debug;

public class WheelBonus : PassengerEffectBonus
{
    private float _incrementCoef;
    private float _decrementCoef;

    public override GameController.BonusTypes GetBonusType()
    {
        return GameController.BonusTypes.Wheel;
    }
    
    public override void AddEffect(PassengerSM passenger)
    {
        if(IsEffectAdditionPossible(passenger))
            passenger.DragChangeStatePeriod *= _incrementCoef;
    }

    public override void RemoveEffect(PassengerSM passenger)
    {
        if(IsEffectRemovingPossible(passenger))
            passenger.DragChangeStatePeriod *= _decrementCoef;
    }

    public WheelBonus()
    {
        TTL = ConfigReader.GetConfig().GetField("wheelBonus").GetField("TTL").n;
        _incrementCoef = ConfigReader.GetConfig().GetField("wheelBonus").GetField("incrementCoef").n;
        _decrementCoef = 1/_incrementCoef;
    }
}