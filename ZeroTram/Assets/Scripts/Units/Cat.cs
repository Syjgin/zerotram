using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    public class Cat : PassengerSM
    {
        public override string GetClassName()
        {
            return "cat";
        }
    }
}
