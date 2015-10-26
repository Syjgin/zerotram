using System.Collections.Generic;
using System.Diagnostics;
using Assets;
using UnityEngine;

using Debug = UnityEngine.Debug;

public class TicketBonus : AbstractBonus
{
    private float _dist;
    public override GameController.BonusTypes GetBonusType()
    {
        return GameController.BonusTypes.Ticket;
    }
    
    public override void Deactivate()
    {
    }

    public override void AddEffect(PassengerSM passenger)
    {
    }

    public override void RemoveEffect(PassengerSM passenger)
    {
    }

    public override List<MovableCharacterSM> HandleClick(Vector2 point, bool doubleClick)
    {
        List<MovableCharacterSM> affectedPassengers = new List<MovableCharacterSM>();
        if (doubleClick)
            return affectedPassengers;
        List<PassengerSM> passengersNear = GameController.GetInstance().AllPassengersInDist(point, _dist);
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

    public TicketBonus()
    {
        TTL = ConfigReader.GetConfig().GetField("ticketBonus").GetField("TTL").n;
        _dist = ConfigReader.GetConfig().GetField("ticketBonus").GetField("dist").n;
    }


}