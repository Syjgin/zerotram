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
            MoveProbability = ConfigReader.GetConfig().GetField("alien").GetField("MoveProbability").n;
            AttackProbability = ConfigReader.GetConfig().GetField("alien").GetField("AttackProbability").n;
            ChangeStatePeriod = ConfigReader.GetConfig().GetField("alien").GetField("ChangeStatePeriod").n;
            AttackDistance = ConfigReader.GetConfig().GetField("alien").GetField("AttackDistance").n;
            AttackReloadPeriod = ConfigReader.GetConfig().GetField("alien").GetField("AttackReloadPeriod").n;
            AttackMaxDistance = ConfigReader.GetConfig().GetField("alien").GetField("AttackMaxDistance").n;
            CounterAttackProbability = ConfigReader.GetConfig().GetField("alien").GetField("CounterAttackProbability").n;
            Hp = InitialLifes = ConfigReader.GetConfig().GetField("alien").GetField("InitialLifes").n;
            Velocity = ConfigReader.GetConfig().GetField("alien").GetField("Velocity").n;
            AttackStrength = ConfigReader.GetConfig().GetField("alien").GetField("AttackStrength").n;
            AttackReactionPeriod = ConfigReader.GetConfig().GetField("alien").GetField("AttackReactionPeriod").n;
            TicketProbability = ConfigReader.GetConfig().GetField("alien").GetField("TicketProbability").n;
            StickProbability = ConfigReader.GetConfig().GetField("alien").GetField("StickProbability").n;
            base.Init();
        }
    }
}
