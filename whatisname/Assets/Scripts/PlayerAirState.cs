using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) 
        : base(_player, _stateMachine, _animBoolName)
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

        if (Input.GetKeyDown(KeyCode.LeftControl) && player.IsWallDetected())
        {
            Debug.Log("Fall에서 벽 잡기");
            stateMachine.ChangeState(player.wallgripState);            
        }
            

        if (player.IsGroundDetected())
            stateMachine.ChangeState(player.idleState);
        
    }
}
