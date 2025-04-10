using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{

    private int comboCounter;

    private float lastTimeAttacked;
    private float comboWindow = 2;


    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) 
        : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (comboCounter > 2 || Time.time >= lastTimeAttacked + comboWindow)
            comboCounter = 0;
        //Debug.Log("콤보 카운터 : " + comboCounter);

        player.anim.SetInteger("ComboCounter",comboCounter);

        #region 공격방향선택
        float attackDir = player.facingDir;

        if (xInput != 0)
            attackDir = xInput;
        #endregion

        //player.anim.speed = 3; //애니메이션 속도 증가 (공속 증가 느낌?)

        player.SetVelocity(player.attackMovement[comboCounter].x * attackDir, player.attackMovement[comboCounter].y);


    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine("BusyFor", 0.1f);
        //player.anim.speed = 1;

        comboCounter++;
        lastTimeAttacked = Time.time;

        //Debug.Log("마지막 공격 시간 : " + lastTimeAttacked);
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            player.SetZeroVelocity();

        if (triggercCalled)
            stateMachine.ChangeState(player.idleState);
    }
}
