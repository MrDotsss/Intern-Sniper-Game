using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public override EPlayerStates StateType => EPlayerStates.Dash;

    private CustomTimer timer;

    public override void Enter(Dictionary<string, object> data = null)
    {
        if (data != null)
        {
            if (data.ContainsKey("dir") && data.ContainsKey("pow"))
            {
                player.DoDash((Vector3)data["dir"], (float)data["pow"]);
            }
        }

        timer = gameObject.AddComponent<CustomTimer>();
        timer.OnTimerComplete += TimeOut;
        timer.StartTimer(player.dashTime);
    }

    public override void Exit(Dictionary<string, object> data = null)
    {
        Destroy(timer);
        timer.OnTimerComplete -= TimeOut;
    }

    public override void PhysicsUpdate()
    {

    }

    public override void UpdateState()
    {
        player.cam.TiltCam(player.GetInputDir().x * -4f * Mathf.Deg2Rad);
        player.cam.FOVCam(70, 8f);

        if (player.isGrounded)
        {
            if (player.input.actions["crouch"].IsPressed() && player.GetInputDir().magnitude != 0)
                stateMachine.TransitionTo(EPlayerStates.Slide, new Dictionary<string, object>
                { { "dir", player.GetMoveDirection() }, { "pow", player.slideStrength } });
            else stateMachine.TransitionTo(EPlayerStates.Idle);
        }

        if (player.isOnWall && player.GetInputDir().y > 0 && !player.isGrounded && player.canWallRun)
        {
            stateMachine.TransitionTo(EPlayerStates.WallRun);
        }
    }

    private void TimeOut()
    {
        stateMachine.TransitionTo(EPlayerStates.Air);
    }
}
