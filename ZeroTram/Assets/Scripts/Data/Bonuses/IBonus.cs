using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets;
using JetBrains.Annotations;

public interface IBonus
{
    bool IsActive();
    void Activate();
    void Deactivate();
    void DecrementTimer(float delta);
    GameController.BonusTypes GetBonusType();
    void AddEffect(PassengerSM passenger);
    void RemoveEffect(PassengerSM passenger);
}
