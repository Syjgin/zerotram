using Assets;
using UnityEngine;
using System.Collections;

public class ConductorCollisionDetector : CollisionDetector
{

    private ConductorSM _conductor;

    void Awake()
    {
        _conductor = (ConductorSM) Character;
    }
    
    void OnMouseUp()
    {
        if (Time.timeScale == 0)
            return;
        MonobehaviorHandler.GetMonobeharior()
                .GetObject<BonusTimer>("bonusTimer")
                .HandleTouchUp(_conductor.transform.position);
        _conductor.StopDrag();
    }
}
