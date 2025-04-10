using UnityEngine;

public class SkeletonAttackState : EnemyState
{
    protected Enemy_Skeleton enemy;
    public SkeletonAttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton _enemy)
        : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        enemy.lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();
        Debug.Log("공격");
        enemy.SetZeroVelocity();

        //BattleState로 돌아가서 추격할지 더 공격할지 정하기
        if (triggerCalled)
            stateMachine.ChangeState(enemy.battleState);
        
    }
}
