using System.Collections.Generic;
using System.Diagnostics;
using Assets;
using UnityEngine;

public abstract class OneActionBonus : PassengerEffectBonus
{
    public override void Activate()
    {
        _isActive = true;
        if (IsPassengersAffected)
        {
            List<PassengerSM> passengersNear = GameController.GetInstance().AllPassengersInDist(Position, Dist);
            foreach (var passengerSm in passengersNear)
            {
                AddEffectToPassenger(passengerSm);
            }
        }
        if (IsConductorAffected)
        {
            ConductorSM hero = MonobehaviorHandler.GetMonobeharior().GetObject<Floor>("Floor").GetHero();
            float dist =
                ((Vector2)hero.transform.position -
                 Position).sqrMagnitude;
            if (dist <= Dist)
            {
                AddEffectToConductor(hero);
            }
        }
        Deactivate();
    }

    protected virtual void AddEffectToPassenger(PassengerSM passenger)
    {

    }

    protected override void AddEffectAfterCheck(PassengerSM passenger)
    {
        
    }

    protected override void RemoveEffectAfterCheck(PassengerSM passenger)
    {
        
    }
}