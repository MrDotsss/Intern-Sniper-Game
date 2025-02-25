using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerState
{
    public override EPlayerStates StateType => EPlayerStates.Run;

    public override void Enter(Dictionary<string, object> data = null)
    {
        
    }

    public override void Exit(Dictionary<string, object> data = null)
    {
        
    }

    public override void PhysicsUpdate()
    {
        player.MovePlayer(player.GetMoveDirection(), player.maxSpeed, player.acceleration);
    }

    public override void UpdateState()
    {
        player.cam.TiltCam(player.GetInputDir().x * -2 * Mathf.Deg2Rad);
        player.cam.FOVCam(80);

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
            stateMachine.TransitionTo(EPlayerStates.Slide, new Dictionary<string, object>
                { { "dir", player.GetMoveDirection() }, { "pow", player.slideStrength } });
        }
        else if (player.GetInputDir().magnitude == 0)
        {
            stateMachine.TransitionTo(EPlayerStates.Idle);
        }
    }
}
