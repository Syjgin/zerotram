using System.Diagnostics;
using Assets;
using UnityEngine;

using Debug = UnityEngine.Debug;

public class BootBonus : AbstractBonus
{
    public override GameController.BonusTypes GetBonusType()
    {
        return GameController.BonusTypes.Boot;
    }

    public override void Activate()
    {
        base.Activate();
        MonobehaviorHandler.GetMonobeharior().GetObject<Floor>("Floor").ChangeWayoutSquare(IncrementCoef);
    }

    public override void Deactivate()
    {
        base.Deactivate();
        MonobehaviorHandler.GetMonobeharior().GetObject<Floor>("Floor").ChangeWayoutSquare(DecrementCoef);
    }

    public BootBonus(string bonusName = "bootBonus")
    {
        InitTTL(bonusName);
        InitCoef(bonusName);
    }
}