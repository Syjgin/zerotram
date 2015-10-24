using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    public class Gnome : PassengerSM
    {
        public override void Init()
        {
            AttackProbability = ConfigReader.GetConfig().GetField("gnome").GetField("AttackProbability").n;
            ChangeStatePeriod = ConfigReader.GetConfig().GetField("gnome").GetField("ChangeStatePeriod").n;
            DragChangeStatePeriod = ConfigReader.GetConfig().GetField("gnome").GetField("DragChangeStatePeriod").n;
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

        protected override void ShowCharacterInfo()
        {
            if (PlayerPrefs.HasKey(NewCharacterWindow.Prefix + NewCharacterWindow.Character.Gnome))
                return;
            Window.SetCharacterToShow(NewCharacterWindow.Character.Gnome);
        }
    }
}
