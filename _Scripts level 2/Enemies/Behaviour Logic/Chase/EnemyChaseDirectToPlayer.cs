using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Chase-Direct Chase", menuName = "Enemy Logic/Chase/Chase-Direct")]
public class EnemyChaseDirectToPlayer : EnemyChaseSOBase
{
    [SerializeField] private float moveSpeed = 10f;
    
    private static readonly int IsRunning = Animator.StringToHash("IsRunning");
    private bool _isMoving;

    public override void Initialize(GameObject gameObject, Enemy enemy)
    {
        base.Initialize(gameObject, enemy);
    }

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        Enemy.Animator.SetBool(IsRunning, true);
        var direction = Mathf.Sign(PlayerTransform.position.x - Transform.position.x);
        Enemy.SpriteRenderer.flipX = direction < 0;
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
        StopMovingAudio();
        Enemy.Animator.SetBool(IsRunning, false);
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();
    }

   

    public override void DoPhysicsLogic()
    {
        base.DoPhysicsLogic();
        var playerPosition = PlayerTransform.position;
        var distance = Enemy.CheckPlayerDistance(playerPosition);

        if (MoveTowardsPlayer(playerPosition))
        {
            StartMovingAudio();
        }
        else
        {
            StopMovingAudio();
        }
        
        if (distance < Enemy.attackDistance * Enemy.attackDistance)
        {
            Enemy.StateMachine.ChangeState(Enemy.AttackState);
        }


    }
    private bool MoveTowardsPlayer(Vector3 playerPosition)
    {
        var position = Transform.position;
        var direction = (playerPosition - position).normalized;
        direction.y = 0f;
        if (!IsGrounded())
        {
            direction.y -= 0.5f;
        }

        Enemy.Rigidbody2D.MovePosition(Transform.position + direction * (moveSpeed * Time.deltaTime));
        return direction.sqrMagnitude > 0.01f;
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
    public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
    }

    public override void ResetValues()
    {
        base.ResetValues();
    }
}
