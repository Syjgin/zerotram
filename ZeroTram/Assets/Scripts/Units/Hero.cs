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
            AttackMaxDistance = 2;
        }

        public bool IsInAttackRadius(MovableObject obj)
        {
            float sqrRemainingDistance = (transform.position - obj.transform.position).sqrMagnitude;
            return sqrRemainingDistance < AttackMaxDistance;
        }
    }
}
