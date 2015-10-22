using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IBonus
{
    bool IsActive();
    void Activate();
    void DecrementTimer(float delta);
}
