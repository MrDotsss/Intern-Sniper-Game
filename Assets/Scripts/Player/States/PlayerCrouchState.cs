using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchState : PlayerState
{
    public override EPlayerStates StateType => EPlayerStates.Crouch;

    public override void Enter(Dictionary<string, object> data = null)
    {
        player.DoCrouch(player.crouchYScale);
    }

    public override void Exit(Dictionary<string, object> data = null)
    {
        player.DoCrouch(player.startYScale);
    }

    public override void PhysicsUpdate()
    {
        player.MovePlayer(player.GetMoveDirection(), player.crouchSpeed, player.friction);
    }

    public override void UpdateState()
    {
        player.cam.TiltCam(0);
        player.cam.FOVCam(58, 8f);

        if (!player.isGrounded)
        {
            stateMachine.TransitionTo(EPlayerStates.Air);
        }
        
        if (!player.input.actions["crouch"].IsPressed() && !player.isOnCeiling)
        {
            stateMachine.TransitionTo(EPlayerStates.Idle);
        }
        else if (player.input.actions["jump"].IsPressed() && !player.isOnCeiling)
        {
            stateMachine.TransitionTo(EPlayerStates.Air, new Dictionary<string, object> { { "jump", player.jumpStrength } });
        }
    }
}
