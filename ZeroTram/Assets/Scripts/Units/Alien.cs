using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets
{
    public class Alien : Passenger
    {
        public override void Init()
        {
            MoveProbability = 50;
            AttackProbability = 5;
            ChangeStatePeriod = 5;
            AttackDistance = 1;
            AttackReloadPeriod = 0.5f;
            AttackMaxDistance = 50;
            CounterAttackProbability = 10;
            Hp = 750;
            Velocity = 7;
            AttackStrength = 10;
            AttackReactionPeriod = 0.5f;
            TicketProbability = 80;
            StickProbability = 5;
            base.Init();
        }
    }
}
