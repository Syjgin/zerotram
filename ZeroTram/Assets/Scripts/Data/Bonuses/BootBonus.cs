using System.Diagnostics;
using Assets;
using UnityEngine;

using Debug = UnityEngine.Debug;

public class BootBonus : AbstractBonus
{
    private float _incrementCoef;
    private float _decrementCoef;

    public override GameController.BonusTypes GetBonusType()
    {
        return GameController.BonusTypes.Boot;
    }

    public override void Activate()
    {
        base.Activate();
        MonobehaviorHandler.GetMonobeharior().GetObject<Floor>("Floor").ChangeWayoutSquare(_incrementCoef);
    }

    public override void Deactivate()
    {
        MonobehaviorHandler.GetMonobeharior().GetObject<Floor>("Floor").ChangeWayoutSquare(_decrementCoef);
    }

    public override void AddEffect(PassengerSM passenger)
    {
    }

    public override void RemoveEffect(PassengerSM passenger)
    {
    }

    public BootBonus()
    {
        TTL = ConfigReader.GetConfig().GetField("bootBonus").GetField("TTL").n;
        _incrementCoef = ConfigReader.GetConfig().GetField("bootBonus").GetField("incrementCoef").n;
        _decrementCoef = 1/_incrementCoef;
    }
}