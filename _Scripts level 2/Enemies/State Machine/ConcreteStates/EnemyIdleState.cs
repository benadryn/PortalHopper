

public class EnemyIdleState : NewEnemyState
{
    private readonly Enemy _enemy;

    public EnemyIdleState(Enemy enemy, NewEnemyStateMachine newEnemyStateMachine) : base(enemy, newEnemyStateMachine)
    {
        _enemy = enemy;
    }

    public override void EnterState()
    {
        base.EnterState();
        _enemy.EnemyIdleBaseInstance.DoEnterLogic();
    }

    public override void ExitState()
    {

        base.ExitState();
        _enemy.EnemyIdleBaseInstance.DoExitLogic();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        _enemy.EnemyIdleBaseInstance.DoFrameUpdateLogic();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        _enemy.EnemyIdleBaseInstance.DoPhysicsLogic();
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
        _enemy.EnemyIdleBaseInstance.DoAnimationTriggerEventLogic(triggerType);
    }
}
