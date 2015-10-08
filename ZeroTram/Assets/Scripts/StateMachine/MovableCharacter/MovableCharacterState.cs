using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
public class MovableCharacterState : State
{
    protected MovableCharacterSM MovableCharacter;
    
    public MovableCharacterState(StateMachine parent) : base(parent)
    {
        MovableCharacter = (MovableCharacterSM) parent;
    }
}
