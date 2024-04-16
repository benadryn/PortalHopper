using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack-Boss Attack", menuName = "Enemy Logic/Attack/Boss-Attack")]
public class EnemyBossAttack : EnemyAttackSOBase
{
    [SerializeField] private float attackCooldownTime = 1.1f;
    
    private EnemyDamageCollider _damageCollider;
    private Countdown _attackCooldownTimer;
    private bool _isAttacking;
    private static readonly int IsAttacking = Animator.StringToHash("IsAttacking");
    private static readonly int IsIdle = Animator.StringToHash("IsIdle");

    public override void Initialize(GameObject gameObject, Enemy enemy)
    {
        base.Initialize(gameObject, enemy);
        _attackCooldownTimer = new Countdown(attackCooldownTime);
        _damageCollider = gameObject.GetComponentInChildren<EnemyDamageCollider>();
    }
    
    public override void DoExitLogic()
    {
        base.DoExitLogic();
        Enemy.Animator.SetBool(IsIdle, false);
        Enemy.Animator.SetBool(IsAttacking, false);

    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        if (Enemy.CheckPlayerDistance(PlayerTransform.position) > Enemy.attackDistance * Enemy.attackDistance && !_isAttacking)
        {
            Enemy.StateMachine.ChangeState(Enemy.ChaseState);
        }
        
        if (_attackCooldownTimer.IsTimerElapsed())
        {
            Enemy.FlipSprite(PlayerTransform.position);
            Enemy.Animator.SetBool(IsAttacking, true);
            Enemy.Animator.SetBool(IsIdle, false);
            _isAttacking = true;
            _attackCooldownTimer.ResetTimer();
        }
        else
        {
            Enemy.Animator.SetBool(IsIdle, true);
        }
        _attackCooldownTimer.UpdateTimer();
    }

    public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
        switch (triggerType)
        {
            case Enemy.AnimationTriggerType.EnemyAttackEnd:
                Enemy.Animator.SetBool(IsAttacking, false);
                _damageCollider.SetDamageColliderActive(false);
                _isAttacking = false;
                break;
            case Enemy.AnimationTriggerType.EnemyAttack:
                _damageCollider.SetDamageColliderActive(true);
                if (Enemy.attackSfx)
                {
                    StopAndPlayAudio(Enemy.attackSfx);
                }
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
