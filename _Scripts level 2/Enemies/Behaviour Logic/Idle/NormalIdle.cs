using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Normal Idle", menuName = "Enemy Logic/Idle/NormalIdle")]
public class NormalIdle : EnemyIdleSOBase
{
    private static readonly int IsIdle = Animator.StringToHash("IsIdle");
    

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        Enemy.Animator.SetBool(IsIdle, true);
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
        Enemy.Animator.SetBool(IsIdle, false);
    }
    
}
