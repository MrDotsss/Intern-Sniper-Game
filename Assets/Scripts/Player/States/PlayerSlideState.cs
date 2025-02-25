using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlideState : PlayerState
{
    public override EPlayerStates StateType => EPlayerStates.Slide;

    private CustomTimer timer;

    public override void Enter(Dictionary<string, object> data = null)
    {
        if (data != null)
        {
            if (data.ContainsKey("dir") && data.ContainsKey("pow"))
            {
                player.DoCrouch(player.crouchYScale);
                player.DoDash((Vector3)data["dir"], (float)data["pow"]);
            }
        }
        
        timer = gameObject.AddComponent<CustomTimer>();
        timer.OnTimerComplete += TimeOut;
        timer.StartTimer(player.slideTime);
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
        player.cam.TiltCam(8f * Mathf.Deg2Rad, 4f);
        player.cam.FOVCam(70, 8);

        if (!player.isGrounded)
        {
            player.DoCrouch(player.startYScale);
            stateMachine.TransitionTo(EPlayerStates.Air);
        }

        if (player.input.actions["jump"].IsPressed() && !player.isOnCeiling)
        {
            player.DoCrouch(player.startYScale);
            stateMachine.TransitionTo(EPlayerStates.Air, new Dictionary<string, object> { { "jump", player.jumpStrength } });
        }
    }

    private void TimeOut()
    {
        stateMachine.TransitionTo(EPlayerStates.Crouch);
    }
}
