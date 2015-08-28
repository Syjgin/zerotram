using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets
{
    public class Gnome : Passenger
    {
        public override void Init()
        {
            MoveProbability = ConfigReader.GetConfig().GetField("gnome").GetField("MoveProbability").n;
            AttackProbability = ConfigReader.GetConfig().GetField("gnome").GetField("AttackProbability").n;
            ChangeStatePeriod = ConfigReader.GetConfig().GetField("gnome").GetField("ChangeStatePeriod").n;
            AttackDistance = ConfigReader.GetConfig().GetField("gnome").GetField("AttackDistance").n;
            AttackReloadPeriod = ConfigReader.GetConfig().GetField("gnome").GetField("AttackReloadPeriod").n;
            AttackMaxDistance = ConfigReader.GetConfig().GetField("gnome").GetField("AttackMaxDistance").n;
            CounterAttackProbability = ConfigReader.GetConfig().GetField("gnome").GetField("CounterAttackProbability").n;
            Hp = InitialLifes = ConfigReader.GetConfig().GetField("gnome").GetField("InitialLifes").n;
            Velocity = ConfigReader.GetConfig().GetField("gnome").GetField("Velocity").n;
            AttackStrength = ConfigReader.GetConfig().GetField("gnome").GetField("AttackStrength").n;
            AttackReactionPeriod = ConfigReader.GetConfig().GetField("gnome").GetField("AttackReactionPeriod").n;
            TicketProbability = ConfigReader.GetConfig().GetField("gnome").GetField("TicketProbability").n;
            StickProbability = ConfigReader.GetConfig().GetField("gnome").GetField("StickProbability").n;
            base.Init();
        }
    }
}
