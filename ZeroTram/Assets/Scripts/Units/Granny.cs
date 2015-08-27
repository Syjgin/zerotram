using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets
{
    public class Granny : Passenger
    {
        public override void Init()
        {
            MoveProbability = 10;
            AttackProbability = 70;
            ChangeStatePeriod = 20;
            AttackDistance = 1;
            AttackReloadPeriod = 2;
            AttackMaxDistance = 10;
            CounterAttackProbability = 90;
            Hp = 510;
            Velocity = 3;
            AttackStrength = 15;
            AttackReactionPeriod = 0.5f;
            TicketProbability = 100;
            StickProbability = 60;
            base.Init();
        }
    }
}
