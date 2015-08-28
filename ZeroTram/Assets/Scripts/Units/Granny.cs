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
            MoveProbability = ConfigReader.GetConfig().GetField("granny").GetField("MoveProbability").n;
            AttackProbability = ConfigReader.GetConfig().GetField("granny").GetField("AttackProbability").n;
            ChangeStatePeriod = ConfigReader.GetConfig().GetField("granny").GetField("ChangeStatePeriod").n;
            AttackDistance = ConfigReader.GetConfig().GetField("granny").GetField("AttackDistance").n;
            AttackReloadPeriod = ConfigReader.GetConfig().GetField("granny").GetField("AttackReloadPeriod").n;
            AttackMaxDistance = ConfigReader.GetConfig().GetField("granny").GetField("AttackMaxDistance").n;
            CounterAttackProbability = ConfigReader.GetConfig().GetField("granny").GetField("CounterAttackProbability").n;
            Hp = InitialLifes = ConfigReader.GetConfig().GetField("granny").GetField("InitialLifes").n;
            Velocity = ConfigReader.GetConfig().GetField("granny").GetField("Velocity").n;
            AttackStrength = ConfigReader.GetConfig().GetField("granny").GetField("AttackStrength").n;
            AttackReactionPeriod = ConfigReader.GetConfig().GetField("granny").GetField("AttackReactionPeriod").n;
            TicketProbability = ConfigReader.GetConfig().GetField("granny").GetField("TicketProbability").n;
            StickProbability = ConfigReader.GetConfig().GetField("granny").GetField("StickProbability").n;
            base.Init();
        }
    }
}
