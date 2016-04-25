using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

using Debug = UnityEngine.Debug;

public class ClewBonus : PassengerEffectBonus
{
    private static Spawner _spawner;
    public override GameController.BonusTypes GetBonusType()
    {
        return GameController.BonusTypes.Clew;
    }
    
    protected override void AddEffectAfterCheck(PassengerSM passenger)
    {
        ReplacePassenger(passenger);
    }

    public static void ReplacePassenger(PassengerSM passenger)
    {
        Vector3 passengerPosition = passenger.transform.position;
        bool hasTicket = passenger.HasTicket();
        bool isVisibleToHero = passenger.IsVisibleToHero();
        string passengerType = passenger.GetClassName();
        passenger.enabled = false;
        if (_spawner == null)
        {
            _spawner = MonobehaviorHandler.GetMonobeharior().GetObject<Spawner>("Spawner");
        }
        PassengerSM newPassenger = _spawner.SpawnAlternativePassenger(passengerPosition, passengerType);
        newPassenger.SetTicketAndVisibility(hasTicket, isVisibleToHero);
        GameController.GetInstance().ReplacePassenger(newPassenger, passenger);
    }
    
    protected override void RemoveEffectAfterCheck(PassengerSM passenger)
    {
    }
    
    public ClewBonus()
    {
        TTL = 0;
        IsPassengersAffected = true;
    }


}