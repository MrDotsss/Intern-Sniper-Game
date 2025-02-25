using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState<EStates> : MonoBehaviour where EStates : System.Enum
{
    protected StateMachine<EStates> stateMachine;
    public abstract EStates StateType { get; }

    public virtual void Initialize(StateMachine<EStates> stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public abstract void Enter(Dictionary<string, object> data = null);
    public abstract void PhysicsUpdate();
    public abstract void UpdateState();
    public abstract void Exit(Dictionary<string, object> data = null);
}
