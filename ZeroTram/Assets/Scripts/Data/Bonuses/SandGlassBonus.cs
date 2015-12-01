using System.Collections.Generic;
using System.Diagnostics;
using Assets;
using UnityEngine;

using Debug = UnityEngine.Debug;

public class SandGlassBonus : PassengerEffectBonus
{
    private float _incrementCoef;
    private float _decrementCoef;
    public override GameController.BonusTypes GetBonusType()
    {
        return GameController.BonusTypes.SandGlass;
    }
    
    protected override void AddEffectAfterCheck(PassengerSM passenger)
    {
        passenger.Velocity *= _decrementCoef;
    }

    protected override void RemoveEffectAfterCheck(PassengerSM passenger)
    {
        passenger.Velocity *= _incrementCoef;
    }
    
    public SandGlassBonus()
    {
        TTL = ConfigReader.GetConfig().GetField("sandglassBonus").GetField("TTL").n;
        _decrementCoef = ConfigReader.GetConfig().GetField("sandglassBonus").GetField("decrementCoef").n;
        _incrementCoef = 1/_decrementCoef;
        IsPassengersAffected = true;
    }


}