
using UnityEngine;
[CreateAssetMenu(fileName = "Normal Death", menuName = "Enemy Logic/Death/Normal-Death")]
public class NormalDeath : EnemyDeathSOBase
{
    private static readonly int IsDead = Animator.StringToHash("IsDead");

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        Enemy.Animator.SetBool(IsDead, true);
        Enemy.AudioSource.Stop();
        if (Enemy.deathSfx)
        {
            Enemy.AudioSource.PlayOneShot(Enemy.deathSfx);
        }

        Enemy.Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
        Enemy.PlayerDetectionCollider.enabled = false;
        Enemy.CapsuleCollider2D.enabled = false;

        if (Enemy.CompareTag("Boss"))
        {
            EndLevelTwo.Instance.OpenPortal();
        }
        
        Destroy(Enemy.gameObject, 3f);
    }

}
