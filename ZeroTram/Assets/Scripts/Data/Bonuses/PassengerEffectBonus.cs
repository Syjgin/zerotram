using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets;

public abstract class PassengerEffectBonus : AbstractBonus
{
    public override void Activate()
    {
        base.Activate();
        GameController.GetInstance().BonusEffectToPassengers(this, true);
    }

    public override void Deactivate()
    {
        base.Deactivate();
        GameController.GetInstance().BonusEffectToPassengers(this, false);
    }

    protected bool IsEffectAdditionPossible(PassengerSM passenger)
    {
        if (IsBonusExist(passenger))
            return false;
        passenger.ActiveBonuses.Add(GetBonusType());
        return true;
    }

    protected bool IsEffectRemovingPossible(PassengerSM passenger)
    {
        if (!IsBonusExist(passenger))
            return false;
        passenger.ActiveBonuses.Remove(GetBonusType());
        return true;
    }
}
