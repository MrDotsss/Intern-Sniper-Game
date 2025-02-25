using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public override EPlayerStates StateType => EPlayerStates.Air;

    public override void Enter(Dictionary<string, object> data = null)
    {
        if (data != null)
        {
            if (data.ContainsKey("jump"))
            {
                player.DoJump((float)data["jump"]);
            }
        }
    }

    public override void Exit(Dictionary<string, object> data = null)
    {

    }

    public override void PhysicsUpdate()
    {
        player.MovePlayer(player.GetMoveDirection(), player.maxSpeed, player.airFriction);
    }

    public override void UpdateState()
    {
        if (!player.isOnWall)
        {
            player.canWallRun = true;
        }

        if (player.GetPlayerVelocity().y < 0 && player.GetInputDir().y > 0 && player.isOnLedge)
        {
            player.DoVault(player.jumpStrength);
        }

        player.cam.TiltCam(0);
        if (player.GetPlayerVelocity().y > 0)
        {
            player.cam.FOVCam(75, 8f);
        }
        else if (player.GetPlayerVelocity().y < 0)
        {
            player.cam.FOVCam(70, 8f);
        }

        if (player.isGrounded)
        {
            if (player.input.actions["crouch"].IsPressed() && player.GetInputDir().magnitude != 0) 
                stateMachine.TransitionTo(EPlayerStates.Slide);
            else stateMachine.TransitionTo(EPlayerStates.Idle);
        }

        if (player.isOnWall && player.GetInputDir().y > 0 && !player.isGrounded && player.canWallRun)
        {
            stateMachine.TransitionTo(EPlayerStates.WallRun);
        }
        else if (player.input.actions["crouch"].WasPressedThisFrame())
        {
            stateMachine.TransitionTo(EPlayerStates.Dash, new Dictionary<string, object>
            { { "dir", player.GetMoveDirection() }, { "pow", player.dashStrength } });
        }
    }
}
