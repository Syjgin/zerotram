using Assets;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class PassengerCollisionDetector : CollisionDetector
{
    private PassengerSM _passenger;

    void Awake()
    {
        _passenger = (PassengerSM) Character;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        _passenger.HandleTriggerEnter(other);
    }
    
    void OmMouseUp()
    {
        _passenger.StopDrag();
    }

    protected override void OnMouseDown()
    {
        if (Time.timeScale == 0)
            return;
        if (
            MonobehaviorHandler.GetMonobeharior()
                .GetObject<BonusTimer>("bonusTimer")
                .IsAnyBonusActive())
        {
            bool isDoubleClick = Character.TimeSincePreviousClickMade < MovableCharacterSM.MaxClickDuration;
            Vector2 targetPos = MonobehaviorHandler.GetMonobeharior()
                .GetObject<Floor>("Floor").GetCurrentMousePosition();
            List<MovableCharacterSM> affectedCharacters = MonobehaviorHandler.GetMonobeharior()
                .GetObject<BonusTimer>("bonusTimer").HandleClick(targetPos, isDoubleClick);
            if(!affectedCharacters.Contains(Character))
                base.OnMouseDown();
        } else
            base.OnMouseDown();
    }
}
