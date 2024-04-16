using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathSOBase : ScriptableObject
{
    protected Enemy Enemy;
    protected Transform Transform;
    protected GameObject GameObject;

    protected Transform PlayerTransform;

    public virtual void Initialize(GameObject gameObject, Enemy enemy)
    {
        GameObject = gameObject;
        Transform = gameObject.transform;
        Enemy = enemy;

        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }
    public virtual void DoEnterLogic(){}
    public virtual void DoExitLogic(){ResetValues();}
    public virtual void DoFrameUpdateLogic(){}
    public virtual void DoPhysicsLogic(){}
    public virtual void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType){}
    public virtual void ResetValues(){}
}
