using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets
{
    public class Bird : Passenger
    {
        public override void Init()
        {
            MoveProbability = 90;
            AttackProbability = 60;
            ChangeStatePeriod = 5;
            AttackDistance = 2f;
            AttackReloadPeriod = 1;
            AttackMaxDistance = 5;
            CounterAttackProbability = 70;
            Hp = 150;
            Velocity = 7;
            AttackStrength = 1;
            AttackReactionPeriod = 0.5f;
            TicketProbability = 50;
            StickProbability = 20;
            base.Init();
        }
    }
}
