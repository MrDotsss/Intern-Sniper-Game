using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallRunState : PlayerState
{
    public override EPlayerStates StateType => EPlayerStates.WallRun;
    private CustomTimer timer;

    public override void Enter(Dictionary<string, object> data = null)
    {
        player.canWallRun = false;

        timer = gameObject.AddComponent<CustomTimer>();
        timer.OnTimerComplete += TimeOut;
        timer.StartTimer(player.wallRunTime);
    }

    public override void Exit(Dictionary<string, object> data = null)
    {
        player.EnableGravity();
        Destroy(timer);
    }

    public override void PhysicsUpdate()
    {
        player.DoWallRun(player.wallRunSpeed, player.acceleration);
    }

    public override void UpdateState()
    {
        player.cam.FOVCam(85f, 8f);

        if (player.frontWall)
        {
            player.DoWallJump(2, 0);
            stateMachine.TransitionTo(EPlayerStates.Air);
        }

        if (player.input.actions["jump"].WasPressedThisFrame())
        {
            player.DoWallJump();
            stateMachine.TransitionTo(EPlayerStates.Air);
        }

        if (!player.isOnWall || player.GetInputDir().y <= 0)
        {
            player.DoWallJump(2, 0);
            stateMachine.TransitionTo(EPlayerStates.Air);
        }
    }

    private void TimeOut()
    {
        player.DoWallJump(2, 0);

        if (!player.isGrounded) stateMachine.TransitionTo(EPlayerStates.Air);
        else stateMachine.TransitionTo(EPlayerStates.Idle);
    }
}
