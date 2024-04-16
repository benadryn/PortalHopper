using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Attack-Ranged", menuName = "Enemy Logic/Attack/Range Attack")]
public class EnemyRangedAttack : EnemyAttackSOBase
{
    private ProjectileManager _projectileManager;
    private ProjectileBehaviour _projectileBehaviour;
    private GameObject _projectile;
    
    [SerializeField] private float secondsForNextAttack = 3.0f;
    
    private bool _isAttacking;
    private bool _isResetCoroutineRunning;
    
    private static readonly int IsAttacking = Animator.StringToHash("IsAttacking");

    public override void Initialize(GameObject gameObject, Enemy enemy)
    {
        base.Initialize(gameObject, enemy);
        _projectileManager = ProjectileManager.Instance;
    }


    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        StartAttack();
    }

    private void StartAttack()
    {
        if (_isAttacking || Enemy.IsDead() || Enemy.StateMachine.CurrentEnemyState != Enemy.AttackState) return;
        Enemy.AudioSource.PlayOneShot(Enemy.attackSfx);
        Enemy.FlipSprite(PlayerTransform.position);
        
        _isAttacking = true;
        
        Enemy.StartCoroutine(PerformAttack());
    }

    private IEnumerator PerformAttack()
    {
        Enemy.Animator.SetBool(IsAttacking, true);
        var position = Enemy.transform.position;

        _projectile = _projectileManager.GetPooledProjectile();
        _projectile.transform.position = position;
        
        // get the projectileBehaviour from the projectile
        _projectileBehaviour = _projectile.GetComponent<ProjectileBehaviour>();
        _projectileBehaviour.SetOwner(Enemy);
        
        yield return new WaitForFixedUpdate();
        if (!_isResetCoroutineRunning  && !Enemy.IsDead() && !_projectileBehaviour.IsBlocked())
        {
            Enemy.StartCoroutine(ResetAttack(secondsForNextAttack));
        }

    }
    
    private IEnumerator ResetAttack(float delay)
    {
        _isResetCoroutineRunning = true;
        Enemy.FlipSprite(PlayerTransform.position);
       
        yield return new WaitForSeconds(delay);
        
        if (_projectile && !_projectileBehaviour.IsBlocked())
        {
            _projectileManager.ReturnProjectileToPool(_projectile);
        }
        _isAttacking = false;
        _isResetCoroutineRunning = false;
        
        StartAttack();
    }

    public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
        if (triggerType == Enemy.AnimationTriggerType.EnemyAttackEnd)
        {
            Enemy.Animator.SetBool(IsAttacking, false);
        }
    }
}
