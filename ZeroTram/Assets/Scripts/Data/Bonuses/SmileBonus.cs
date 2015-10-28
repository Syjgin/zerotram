using System.Collections.Generic;
using System.Diagnostics;
using Assets;
using UnityEngine;

using Debug = UnityEngine.Debug;

public class SmileBonus : PassengerEffectBonus
{
    private float _coef;
    private float _incrementCoef;
    public override GameController.BonusTypes GetBonusType()
    {
        return GameController.BonusTypes.Smile;
    }
    
    public override void AddEffect(PassengerSM passenger)
    {
        if (IsEffectAdditionPossible(passenger))
            passenger.AttackProbability *= _coef;
    }

    public override void RemoveEffect(PassengerSM passenger)
    {
        if (IsEffectRemovingPossible(passenger))
            passenger.AttackProbability *= _incrementCoef;
    }
    
    public SmileBonus()
    {
        TTL = ConfigReader.GetConfig().GetField("smileBonus").GetField("TTL").n;
        _coef = ConfigReader.GetConfig().GetField("smileBonus").GetField("decrementCoef").n;
        _incrementCoef = 1/_coef;
    }


}