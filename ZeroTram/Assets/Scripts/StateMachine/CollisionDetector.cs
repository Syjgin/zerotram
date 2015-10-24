using Assets;
using UnityEngine;
using System.Collections;

public class CollisionDetector : MonoBehaviour
{

    [SerializeField] public MovableCharacterSM Character;
    
    void OnMouseDown()
    {
        if(Time.timeScale == 0)
            return;
        if(Character.TimeSincePreviousClickMade > MovableCharacterSM.MaxClickDuration)
            Character.HandleClick();
        else
            Character.HandleDoubleClick();
        Character.TimeSincePreviousClickMade = 0;
    }
}
