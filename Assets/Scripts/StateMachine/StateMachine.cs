using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine<EStates> : MonoBehaviour where EStates : System.Enum
{
    public EStates initialState;
    public bool showDebugState = false;

    private BaseState<EStates> currentState;

    private Dictionary<EStates, BaseState<EStates>> stateDictionary = new Dictionary<EStates, BaseState<EStates>>();


    private void Start()
    {
        BaseState<EStates>[] states = gameObject.GetComponentsInChildren<BaseState<EStates>>();

        foreach(BaseState<EStates> state in states)
        {
            if (!stateDictionary.ContainsKey(state.StateType))
            {
                stateDictionary[state.StateType] = state;
                state.Initialize(this);
            }
        }

        TransitionTo(initialState);
    }

    private void Update()
    {
        currentState?.UpdateState();
    }

    private void FixedUpdate()
    {
        currentState?.PhysicsUpdate();
    }

    public void TransitionTo(EStates nextState, Dictionary<string, object> data = null)
    {
        if (!stateDictionary.ContainsKey(nextState))
        {
            Debug.LogError($"Trying to switch to non-existent state: {nextState}");
            return;
        }

        currentState?.Exit(data);
        currentState = stateDictionary[nextState];
        currentState?.Enter(data);
    }
}
