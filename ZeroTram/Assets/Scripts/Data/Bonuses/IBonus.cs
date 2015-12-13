using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets;
using JetBrains.Annotations;
using UnityEngine;

public interface IBonus
{
    bool IsActive();
    void Activate();
    void Deactivate();
    void DecrementTimer(float delta);
    GameController.BonusTypes GetBonusType();
    void AddEffect(PassengerSM passenger);
    void RemoveEffect(PassengerSM passenger);
    List<MovableCharacterSM> HandleClick(Vector2 position, bool doubleClick);
    int GetTTL();
    void SetPosition(Vector2 pos);
}
