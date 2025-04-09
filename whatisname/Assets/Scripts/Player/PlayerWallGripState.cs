using UnityEngine;

public class PlayerWallGripState : PlayerState
{
    public PlayerWallGripState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) 
        : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        rb.gravityScale = 0;
        player.SetVelocity(0, 0);
    }

    public override void Update()
    {
        base.Update();

        if (yInput < 0)
        {
            Debug.Log("벽 슬라이딩");
            stateMachine.ChangeState(player.wallSlide);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
