using System.Collections.Generic;
using System.Diagnostics;
using Assets;
using UnityEngine;

using Debug = UnityEngine.Debug;

public class TicketBonus : AbstractBonus
{
    public override GameController.BonusTypes GetBonusType()
    {
        return GameController.BonusTypes.Ticket;
    }

    public override List<MovableCharacterSM> HandleClick(Vector2 point, bool doubleClick)
    {
        List<MovableCharacterSM> affectedPassengers = new List<MovableCharacterSM>();
        if (doubleClick)
            return affectedPassengers;
        List<PassengerSM> passengersNear = GameController.GetInstance().AllPassengersInDist(point, Dist);
        foreach (var passengerSm in passengersNear)
        {
            if (!passengerSm.IsVisibleToHero())
            {
                passengerSm.HandleClick();
                affectedPassengers.Add(passengerSm);
            }
        }
        return affectedPassengers;
    }

    public TicketBonus(string bonusName = "ticketBonus")
    {
        InitTTL(bonusName);
        InitDist(bonusName);
    }


}