using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EPlayerStates
{
    Idle,
    Run,
    Crouch,
    Air,
    Slide,
    Dash,
    WallRun
}

public class PlayerStateMachine : StateMachine<EPlayerStates>
{
   
}
