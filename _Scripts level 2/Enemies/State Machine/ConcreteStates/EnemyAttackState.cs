
public class EnemyAttackState : NewEnemyState
{
    private Enemy _enemy;
    private NewEnemyStateMachine _newEnemyStateMachine;
    public EnemyAttackState(Enemy enemy, NewEnemyStateMachine newEnemyStateMachine) : base(enemy, newEnemyStateMachine)
    {
        _enemy = enemy;
        _newEnemyStateMachine = newEnemyStateMachine;
    }

    public override void EnterState()
    {
        base.EnterState();
        _enemy.EnemyAttackBaseInstance.DoEnterLogic();
    }

    public override void ExitState()
    {
        base.ExitState();
        _enemy.EnemyAttackBaseInstance.DoExitLogic();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        _enemy.EnemyAttackBaseInstance.DoFrameUpdateLogic();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        _enemy.EnemyAttackBaseInstance.DoPhysicsLogic();
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
        _enemy.EnemyAttackBaseInstance.DoAnimationTriggerEventLogic(triggerType);
    }
}
