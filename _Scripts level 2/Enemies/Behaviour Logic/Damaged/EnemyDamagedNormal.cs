using UnityEngine;
[CreateAssetMenu(fileName = "Enemy Damaged-Normal", menuName = "Enemy Logic/Damaged/Normal Damaged")]
public class EnemyDamagedNormal : EnemyDamagedSOBase
{
    private static readonly int IsDamaged = Animator.StringToHash("IsDamaged");

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        Enemy.Animator.SetBool(IsDamaged, true);
        Enemy.AudioSource.Stop();
        if (Enemy.damageSfx)
        {
            Enemy.AudioSource.PlayOneShot(Enemy.damageSfx);
        }

    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
        Enemy.Animator.SetBool(IsDamaged, false);
    }
    
}
