using UnityEngine;

[CreateAssetMenu(fileName = "No Chase", menuName = "Enemy Logic/Chase/No Chase")]
public class EnemyNoChase : EnemyChaseSOBase
{
    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        Enemy.StateMachine.ChangeState(Enemy.AttackState);
    }
    
}
