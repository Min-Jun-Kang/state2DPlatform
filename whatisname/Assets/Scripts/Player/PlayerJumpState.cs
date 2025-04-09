using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) 
        : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        rb.linearVelocity = new Vector2(rb.linearVelocityX, player.jumpForce); //12 만큼 뛰기
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.LeftControl) && player.IsWallDetected())
        {
            Debug.Log("Jump에서벽 잡기");
            //stateMachine.ChangeState(player.wallgripState);
        }


        if (rb.linearVelocityY < 0) //떨어질 때
            stateMachine.ChangeState(player.airState);
    }
}
