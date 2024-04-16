using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Chase-Boss Chase", menuName = "Enemy Logic/Chase/Chase-Boss")]
public class EnemyBossChase : EnemyChaseSOBase
{
    [SerializeField] private float moveSpeed = 3.2f;
    [SerializeField] private float cooldownRangedAttackDuration = 5.0f;
    [SerializeField] private float cooldownAoeAttackDuration = 3.0f;

    [SerializeField] private float rangeAttackDistanceMax = 16.0f;
    [SerializeField] private float rangeAttackDistanceMin = 13.0f;
    [SerializeField] private float aoeAttackDistance = 7.0f;
    [SerializeField] private float closeRangeAttackDistance = 3.0f;
    

    private Countdown _cooldownRangedAttackTimer;
    private Countdown _cooldownAoeAttackTimer;

    private bool _hasRangedAttacked;
    private bool _hasAoeAttacked;
    private bool _isMoving;


    private ProjectileManager _projectileManager;
    private ProjectileBehaviour _projectileBehaviour;
    private GameObject _projectile;
    private KingEnemy _kingEnemy;
    
    private static readonly int IsAttackingAtRange = Animator.StringToHash("IsAttackingRanged");
    private static readonly int IsRunning = Animator.StringToHash("IsRunning");
    private static readonly int IsAoeAttacking = Animator.StringToHash("IsAoeAttacking");

    
    

    public override void Initialize(GameObject gameObject, Enemy enemy)
    {
        base.Initialize(gameObject, enemy);
        _kingEnemy = enemy as KingEnemy;
        _projectileManager = ProjectileManager.Instance;
        _cooldownRangedAttackTimer = new Countdown(cooldownRangedAttackDuration);
        _cooldownAoeAttackTimer = new Countdown(cooldownAoeAttackDuration);

    }


    private void PerformRangedAttack()
    {
        Enemy.Animator.SetBool(IsAttackingAtRange, true);
        var position = Enemy.transform.position;

        _projectile = _projectileManager.GetPooledProjectile();
        _projectile.transform.position = position;

        _projectileBehaviour = _projectile.GetComponent<ProjectileBehaviour>();
        _projectileBehaviour.SetOwner(Enemy);
        _hasRangedAttacked = true;
    }

    public override void DoPhysicsLogic()
    {
        base.DoPhysicsLogic();
        Enemy.Animator.SetBool(IsAttackingAtRange, false);
        var playerPos = PlayerTransform.position;
        var distance = Enemy.CheckPlayerDistance(playerPos);
            
        _cooldownRangedAttackTimer.UpdateTimer();
        _cooldownAoeAttackTimer.UpdateTimer();
            
        if (StartChasing(playerPos))
        {
            StartMovingAudio();
            if (distance <= rangeAttackDistanceMax * rangeAttackDistanceMax && distance >= rangeAttackDistanceMin * rangeAttackDistanceMin)
            {
                if (!_cooldownRangedAttackTimer.IsTimerElapsed() && _hasRangedAttacked) return;
                StopMovingAudio();
                PerformRangedAttack();
                _cooldownRangedAttackTimer.ResetTimer();
            }
            else if (distance <= closeRangeAttackDistance * closeRangeAttackDistance)
            {
                StopMovingAudio();
                Enemy.StateMachine.ChangeState(Enemy.AttackState);
            }
            else if (distance <= aoeAttackDistance * aoeAttackDistance)
            {
                if (!_cooldownAoeAttackTimer.IsTimerElapsed() && _hasAoeAttacked) return;
                StopMovingAudio();
                PerformAoeAttack();
                _cooldownAoeAttackTimer.ResetTimer();
            }
        }

    }
    
    private bool StartChasing(Vector3 playerPos)
    {
        Enemy.Animator.SetBool(IsRunning, true);

        var position = Transform.position;
        var direction = (playerPos - position).normalized;
        Enemy.FlipSprite(playerPos);

        if (!IsGrounded())
        {
            direction.y -= 0.5f;
        }
        Enemy.Rigidbody2D.MovePosition(position + direction * (moveSpeed * Time.deltaTime));
        return direction.sqrMagnitude > 0.01f;
    }
    public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
        if (triggerType == Enemy.AnimationTriggerType.EnemyAttackEnd)
        {
            Enemy.Animator.SetBool(IsAoeAttacking, false);
        }
    }

    public override void ResetValues()
    {
        base.ResetValues();
        Enemy.Animator.SetBool(IsRunning, false);
    }
    
    private void PerformAoeAttack()
    {
        var position = Enemy.transform.position;
        var bottomOfBoss = new Vector3(position.x, position.y - 1.5f, -0.1f);
        
        Enemy.Animator.SetBool(IsAoeAttacking, true);
        
        Instantiate(_kingEnemy.aoeFireball, bottomOfBoss , Quaternion.identity);
        
        _hasAoeAttacked = true;
    }
    
    private bool IsGrounded()
    {
        var bottom = Enemy.CapsuleCollider2D.bounds.min.y;
        var position = Transform.position;
        var hit = Physics2D.Raycast(new Vector2(position.x , bottom),  Vector2.down, 0.1f, LayerMask.GetMask("Ground"));
        return hit.collider;
    }
    
    private void StartMovingAudio()
    {
        if (!_isMoving && !Enemy.AudioSource.isPlaying)
        {
            Enemy.AudioSource.clip = Enemy.runSfx;
            Enemy.AudioSource.loop = true;
            Enemy.AudioSource.Play();
            _isMoving = true;
        }
    }
    
    private void StopMovingAudio()
    {
        if (_isMoving)
        {
            Enemy.AudioSource.Stop();
            _isMoving = false;
        }
    }
}
