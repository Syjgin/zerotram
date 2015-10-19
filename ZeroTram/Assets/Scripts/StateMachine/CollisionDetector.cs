using Assets;
using UnityEngine;
using System.Collections;

public class CollisionDetector : MonoBehaviour
{

    [SerializeField] public MovableCharacterSM Character;
    private const float MaxClickDuration = 0.6f;

    void OnMouseDown()
    {
        if(Time.timeScale == 0)
            return;
        if(Character.TimeSincePreviousClickMade > MaxClickDuration)
            Character.HandleClick();
        else
            Character.HandleDoubleClick();
        Character.TimeSincePreviousClickMade = 0;
    }
}
