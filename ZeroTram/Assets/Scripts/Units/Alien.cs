using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    public class Alien : PassengerSM
    {
        public override void Init()
        {
            AttackProbability = ConfigReader.GetConfig().GetField("alien").GetField("AttackProbability").n;
            ChangeStatePeriod = ConfigReader.GetConfig().GetField("alien").GetField("ChangeStatePeriod").n;
            DragChangeStatePeriod = ConfigReader.GetConfig().GetField("alien").GetField("DragChangeStatePeriod").n;
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

        protected override void ShowCharacterInfo()
        {
            if (PlayerPrefs.HasKey(NewCharacterWindow.Prefix + NewCharacterWindow.Character.Alien))
                return;
            Window.SetCharacterToShow(NewCharacterWindow.Character.Alien);
        }
    }
}
