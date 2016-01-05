using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateMachine : MonoBehaviour
{
    protected Dictionary<int, State> StateMap; 
    [SerializeField] private State _activeState;
    private const int INCORRECT_STATE = Int16.MinValue;
    protected void InitWithStates(Dictionary<int, State> stateMap, int initialState)
    {
        StateMap = stateMap;
        ActivateState(initialState);
    }

    public void ActivateState(int stateValue)
    {
        if (_activeState != null)
        {
            if (!_activeState.IsTransitionAllowed())
                return;
            _activeState.SetEnabled(false);
        }
        if(GetActiveState().Equals(stateValue))
            return;
        foreach (var state in StateMap)
        {
            if (state.Key.Equals(stateValue))
            {
                state.Value.SetEnabled(true);
                _activeState = state.Value;
            }
        }
    }

    protected virtual void Update()
    {
        _activeState.OnUpdate();
    }

    protected virtual void FixedUpdate()
    {
        _activeState.OnFixedUpdate();
    }

    public int GetActiveState()
    {
        foreach (var state in StateMap)
        {
            if (state.Value == _activeState)
                return state.Key;
        }
        return INCORRECT_STATE;
    }
}
