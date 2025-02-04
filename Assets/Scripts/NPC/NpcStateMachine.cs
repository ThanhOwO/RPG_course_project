using UnityEngine;

public class NpcStateMachine
{
    public NpcState currentState;

    public void Initialize(NpcState _currentState)
    {
        currentState = _currentState;
        currentState.Enter();
    }

    public void ChangeState(NpcState _newState)
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }
}
