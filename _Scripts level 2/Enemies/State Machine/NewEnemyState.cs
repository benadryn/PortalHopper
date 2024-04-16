
public class NewEnemyState
{
    protected Enemy Enemy;
    protected NewEnemyStateMachine NewEnemyStateMachine;

    public NewEnemyState(Enemy enemy, NewEnemyStateMachine newEnemyStateMachine)
    {
        Enemy = enemy;
        NewEnemyStateMachine = newEnemyStateMachine;
    }
    
    public virtual void EnterState(){}
    public virtual void ExitState(){}
    public virtual void FrameUpdate(){}
    public virtual void PhysicsUpdate(){}
    public virtual void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType){}
}
