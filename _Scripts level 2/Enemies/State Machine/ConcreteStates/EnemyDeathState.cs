public class EnemyDeathState : NewEnemyState
{
    private readonly Enemy _enemy;
    
    public EnemyDeathState(Enemy enemy, NewEnemyStateMachine newEnemyStateMachine) : base(enemy, newEnemyStateMachine)
    {
        _enemy = enemy;
    }
    public override void EnterState()
    {
        base.EnterState();
        _enemy.EnemyDeathBaseInstance.DoEnterLogic();
    }

    public override void ExitState()
    {
        base.ExitState();
        _enemy.EnemyDeathBaseInstance.DoExitLogic();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        _enemy.EnemyDeathBaseInstance.DoFrameUpdateLogic();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        _enemy.EnemyDeathBaseInstance.DoPhysicsLogic();
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
        _enemy.EnemyDeathBaseInstance.DoAnimationTriggerEventLogic(triggerType);
    }

   
}
