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
    
    protected override void AddEffectAfterCheck(PassengerSM passenger)
    {
        passenger.AttackProbability *= _coef;
    }

    protected override void RemoveEffectAfterCheck(PassengerSM passenger)
    {
        passenger.AttackProbability *= _incrementCoef;
    }
    
    public SmileBonus()
    {
        TTL = ConfigReader.GetConfig().GetField("smileBonus").GetField("TTL").n;
        _coef = ConfigReader.GetConfig().GetField("smileBonus").GetField("decrementCoef").n;
        _incrementCoef = 1/_coef;
        IsPassengersAffected = true;
    }


}