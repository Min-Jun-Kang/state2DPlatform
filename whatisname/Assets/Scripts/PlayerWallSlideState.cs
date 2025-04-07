using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(rb.linearVelocityX, -player.slideSpeed);
        rb.gravityScale = 3.5f;


        if (player.IsGroundDetected())
        {
            Debug.Log("땅 밟기");
            stateMachine.ChangeState(player.idleState);
        }
    }
}
