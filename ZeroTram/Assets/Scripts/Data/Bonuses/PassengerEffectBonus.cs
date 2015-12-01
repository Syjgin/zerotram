using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets;

public abstract class PassengerEffectBonus : AbstractBonus
{
    protected bool IsConductorAffected;
    protected bool IsPassengersAffected;

    public override void Activate()
    {
        base.Activate();
        if (IsPassengersAffected)
        {
            GameController.GetInstance().BonusEffectToPassengers(this, true);
        }
        if (IsConductorAffected)
        {
            ConductorSM conductor = MonobehaviorHandler.GetMonobeharior().GetObject<Floor>("Floor").GetHero();
            AddEffectToConductor(conductor);
        }
    }

    public override void Deactivate()
    {
        base.Deactivate();
        GameController.GetInstance().BonusEffectToPassengers(this, false);
        if (IsConductorAffected)
        {
            ConductorSM conductor = MonobehaviorHandler.GetMonobeharior().GetObject<Floor>("Floor").GetHero();
            RemoveEffectFromConductor(conductor);
        }
    }

    private bool IsEffectAdditionPossible(PassengerSM passenger)
    {
        if (IsBonusExist(passenger))
            return false;
        passenger.ActiveBonuses.Add(GetBonusType());
        return true;
    }

    private bool IsEffectRemovingPossible(PassengerSM passenger)
    {
        if (!IsBonusExist(passenger))
            return false;
        passenger.ActiveBonuses.Remove(GetBonusType());
        return true;
    }

    protected abstract void AddEffectAfterCheck(PassengerSM passenger);
    protected abstract void RemoveEffectAfterCheck(PassengerSM passenger);

    public override void AddEffect(PassengerSM passenger)
    {
        if (IsEffectAdditionPossible(passenger))
            AddEffectAfterCheck(passenger);
    }

    public virtual void AddEffectToConductor(ConductorSM conductor)
    {

    }

    public virtual void RemoveEffectFromConductor(ConductorSM conductor)
    {

    }

    public override void RemoveEffect(PassengerSM passenger)
    {
        if (IsEffectRemovingPossible(passenger))
            RemoveEffectAfterCheck(passenger);
    }
}
