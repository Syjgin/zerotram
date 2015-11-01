using System.Collections.Generic;
using System.Diagnostics;
using Assets;
using UnityEngine;

using Debug = UnityEngine.Debug;

public class AntiHareBonus : PassengerEffectBonus
{
    private float _coef;
    private float _decrementCoef;
    public override GameController.BonusTypes GetBonusType()
    {
        return GameController.BonusTypes.AntiHare;
    }

    protected override void AddEffectAfterCheck(PassengerSM passenger)
    {
        passenger.RecalculateTicketProbability(_coef, true);
    }

    protected override void RemoveEffectAfterCheck(PassengerSM passenger)
    {
        passenger.RecalculateTicketProbability(_decrementCoef, true);
    }

    public AntiHareBonus()
    {
        TTL = ConfigReader.GetConfig().GetField("antiHareBonus").GetField("TTL").n;
        _coef = ConfigReader.GetConfig().GetField("antiHareBonus").GetField("incrementCoef").n;
        _decrementCoef = 1/_coef;
    }


}