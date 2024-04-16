public class EnemyDamagedState : NewEnemyState
{
    private readonly Enemy _enemy;
    public EnemyDamagedState(Enemy enemy, NewEnemyStateMachine newEnemyStateMachine) : base(enemy, newEnemyStateMachine)
    {
        _enemy = enemy;
    }

    public override void EnterState()
    {
        base.EnterState();
        _enemy.EnemyDamagedBaseInstance.DoEnterLogic();
    }

    public override void ExitState()
    {
        base.ExitState();
        _enemy.EnemyDamagedBaseInstance.DoExitLogic();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        _enemy.EnemyDamagedBaseInstance.DoFrameUpdateLogic();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        _enemy.EnemyDamagedBaseInstance.DoPhysicsLogic();
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
        _enemy.EnemyDamagedBaseInstance.DoAnimationTriggerEventLogic(triggerType);
    }
}
