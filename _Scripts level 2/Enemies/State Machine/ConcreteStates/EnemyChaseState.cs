using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : NewEnemyState
{
    private Enemy _enemy;
    private NewEnemyStateMachine _newEnemyStateMachine;
    public EnemyChaseState(Enemy enemy, NewEnemyStateMachine newEnemyStateMachine) : base(enemy, newEnemyStateMachine)
    {
        _enemy = enemy;
        _newEnemyStateMachine = newEnemyStateMachine;
    }

    public override void EnterState()
    {
        base.EnterState();
        _enemy.EnemyChaseBaseInstance.DoEnterLogic();
    }

    public override void ExitState()
    {
        base.ExitState();
        _enemy.EnemyChaseBaseInstance.DoExitLogic();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        _enemy.EnemyChaseBaseInstance.DoFrameUpdateLogic();
    }

    public override void PhysicsUpdate()
    {

        base.PhysicsUpdate();
        _enemy.EnemyChaseBaseInstance.DoPhysicsLogic();

    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
        _enemy.EnemyChaseBaseInstance.DoAnimationTriggerEventLogic(triggerType);
    }
}
