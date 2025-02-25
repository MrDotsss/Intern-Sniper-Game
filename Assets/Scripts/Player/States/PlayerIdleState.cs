using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public override EPlayerStates StateType => EPlayerStates.Idle;

    public override void Enter(Dictionary<string, object> data = null)
    {

    }

    public override void Exit(Dictionary<string, object> data = null)
    {

    }

    public override void PhysicsUpdate()
    {
        player.MovePlayer(player.GetMoveDirection(), 0f, player.friction);
    }

    public override void UpdateState()
    {
        player.cam.TiltCam(0);
        player.cam.FOVCam(70, 8f);

        if (!player.isGrounded)
        {
            stateMachine.TransitionTo(EPlayerStates.Air);
        }

        if (player.input.actions["jump"].IsPressed())
        {
            stateMachine.TransitionTo(EPlayerStates.Air, new Dictionary<string, object> { { "jump", player.jumpStrength } });
        }
        else if (player.input.actions["crouch"].IsPressed())
        {
            stateMachine.TransitionTo(EPlayerStates.Crouch);
        }
        else if (player.GetInputDir().magnitude != 0)
        {
            stateMachine.TransitionTo(EPlayerStates.Run);
        }
    }
}
