using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Level
{
    public interface GameStateNotificationListener
    {
        void UpdatePercentage(int killedPercent, int haresPercent);
    }
}
