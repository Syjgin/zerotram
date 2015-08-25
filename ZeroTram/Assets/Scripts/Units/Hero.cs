using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets
{
    public class Hero : MovableObject
    {
        void Awake()
        {
            Hp = 100;
        }
    }
}
