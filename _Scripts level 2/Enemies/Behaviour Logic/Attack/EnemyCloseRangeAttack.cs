using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Attack-Close", menuName = "Enemy Logic/Attack/Close Range Attack")]
public class EnemyCloseRangeAttack : EnemyAttackSOBase
{
    private EnemyDamageCollider _damageCollider;

    private float _attackDelayReset = 1.5f;
    
    private static readonly int IsAttacking = Animator.StringToHash("IsAttacking");
    
    private bool _isAttacking;
    public override void Initialize(GameObject gameObject, Enemy enemy)
    {
        base.Initialize(gameObject, enemy);
        _damageCollider = gameObject.GetComponentInChildren<EnemyDamageCollider>();
    }

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        Enemy.Animator.SetBool(IsAttacking, true);
        _isAttacking = true;
    }
    
    public override void DoExitLogic()
    {
        base.DoExitLogic();
        Enemy.Animator.SetBool(IsAttacking, false);
        _damageCollider.SetDamageColliderActive(false);
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();
        var playerPosition = PlayerTransform.position;
        var distance = Enemy.CheckPlayerDistance(playerPosition);

        if (distance >= Enemy.attackDistance * Enemy.attackDistance && !_isAttacking)
        {
            Enemy.StateMachine.ChangeState(Enemy.ChaseState);
        }

        if (!_isAttacking)
        {
            _attackDelayReset -= Time.deltaTime;

            if (_attackDelayReset <= 0)
            {
                Enemy.Animator.SetBool(IsAttacking, true);
                _isAttacking = true;
                _attackDelayReset = 1.5f;
            }
        }
        Enemy.FlipSprite(playerPosition);
    }

    public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
        switch (triggerType)
        {
            case Enemy.AnimationTriggerType.EnemyAttack:
                if (Enemy.attackSfx)
                {
                    StopAndPlayAudio(Enemy.attackSfx);
                }
                _isAttacking = true;
                break;
            case Enemy.AnimationTriggerType.EnemyAttackHit:
                _damageCollider.SetDamageColliderActive(true);
                break;
            case Enemy.AnimationTriggerType.EnemyAttackEnd:
                _damageCollider.SetDamageColliderActive(false);
                Enemy.Animator.SetBool(IsAttacking, false);
                _isAttacking = false;
                break;
            case Enemy.AnimationTriggerType.EnemyDamaged:
                _isAttacking = false;
                Enemy.Animator.SetBool(IsAttacking, false);
                break;
        }
    }

    private void StopAndPlayAudio(AudioClip clip)
    {
        if (Enemy.AudioSource.isPlaying)
        {
            Enemy.AudioSource.Stop();
        }
        Enemy.AudioSource.PlayOneShot(clip);
    }
}
