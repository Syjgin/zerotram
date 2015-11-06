using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    public class Alien : PassengerSM
    {
        protected override string GetClassName()
        {
            return "alien";
        }
    }
}
