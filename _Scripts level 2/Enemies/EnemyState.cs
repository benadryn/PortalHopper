using UnityEngine;

public abstract class EnemyState
{
    protected EnemyStateMachine StateMachine;

    protected Rigidbody2D Rigidbody2D;
    protected SpriteRenderer SpriteRenderer;
    protected float MoveSpeed;
    protected float AttackDistance;
    protected Collider2D Collider2D;

    protected EnemyState(EnemyStateMachine stateMachine, Rigidbody2D rigidbody2D, SpriteRenderer spriteRenderer,
        float attackDistance, float moveSpeed, Collider2D collider2D)
    {
        StateMachine = stateMachine;
        Rigidbody2D = rigidbody2D;
        SpriteRenderer = spriteRenderer;
        AttackDistance = attackDistance;
        MoveSpeed = moveSpeed;
        Collider2D = collider2D;
    }

    protected EnemyState(EnemyStateMachine stateMachine)
    {
        StateMachine = stateMachine;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
}
/// <summary>
/// ///////////////////////////////////////////////////////////////////////////////
/// </summary>
public class IdleState : EnemyState
{
    private static readonly int IsIdle = Animator.StringToHash("IsIdle");

    public IdleState(EnemyStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void EnterState()
    {
        StateMachine.GetAnimator().SetBool(IsIdle, true);
    }

    public override void UpdateState()
    {
    }

    public override void ExitState()
    {
        StateMachine.GetAnimator().SetBool(IsIdle, false);
    }
}
/// <summary>
/// ///////////////////////////////////////////////////////////////////////////////////
/// </summary>
public class ChasePlayerState : EnemyState
{
    private Rigidbody2D _rigidbody2D;
    private readonly float _moveSpeed;
    private readonly float _attackDistance;
    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider2D;
    private static readonly int IsRunning = Animator.StringToHash("IsRunning");

    public ChasePlayerState(EnemyStateMachine stateMachine, Rigidbody2D rigidbody2D, SpriteRenderer spriteRenderer, float attackDistance, float moveSpeed, Collider2D collider2D) : base(stateMachine, rigidbody2D, spriteRenderer,  attackDistance,  moveSpeed, collider2D)
    {
        _rigidbody2D = rigidbody2D;
        _spriteRenderer = spriteRenderer;
        _attackDistance = attackDistance;
        _moveSpeed = moveSpeed;
        _collider2D = collider2D;
    }

    public override void EnterState()
    {
        StateMachine.GetAnimator().SetBool(IsRunning, true);
        var direction = Mathf.Sign(StateMachine.GetPlayerController().transform.position.x -
                                   StateMachine.transform.position.x);
        _spriteRenderer.flipX = direction < 0;
    }



    public override void UpdateState()
    {
        var playerPosition = StateMachine.GetPlayerController().transform.position;
        var distance = (playerPosition - StateMachine.transform.position).sqrMagnitude;

        if (distance < _attackDistance * _attackDistance)
        {
            StateMachine.TransitionToState(new AttackPlayerState(StateMachine));
        }
        

        MoveTowardsPlayer(playerPosition);
    }

    public override void ExitState()
    {
        StateMachine.GetAnimator().SetBool(IsRunning, false);

    }
    
    private void MoveTowardsPlayer(Vector3 playerPosition)
    {
        var position = StateMachine.transform.position;
        var direction = (playerPosition - position).normalized;

        if (!IsGrounded())
        {
            direction.y += -0.1f;
        }

        _rigidbody2D.MovePosition(StateMachine.transform.position + direction * (_moveSpeed * Time.deltaTime));
    }

    private bool IsGrounded()
    {
        var bottom = _collider2D.bounds.min.y;
        var position = StateMachine.transform.position;
        var hit = Physics2D.Raycast(new Vector2(position.x , bottom),  Vector2.down, 0.1f, LayerMask.GetMask("Ground"));
        return hit.collider;
    }

}

/// <summary>
/// ///////////////////////////////////////////////////////////////////////////////////////
/// </summary>
public class AttackPlayerState : EnemyState
{    
    private readonly float _attackDistance;

    private static readonly int IsAttacking = Animator.StringToHash("IsAttacking");

    public AttackPlayerState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
        _attackDistance = stateMachine.AttackDistance;
    }

    public override void EnterState()
    {
        StateMachine.GetAnimator().SetBool(IsAttacking, true);
    }

    public override void UpdateState()
    {
        var playerPosition = StateMachine.GetPlayerController().transform.position;
        var distance = (playerPosition - StateMachine.transform.position).sqrMagnitude;
        
        if (distance >= _attackDistance * _attackDistance)
        {
            StateMachine.TransitionToChaseState();
        }
    }

    public override void ExitState()
    {
        StateMachine.GetAnimator().SetBool(IsAttacking, false);

    }
}

/// <summary>
/// /////////////////////////////////////////////////////////////
/// </summary>
    public class DeathState : EnemyState
{
    private readonly CircleCollider2D _playerDetectionCollider; 
        public DeathState(EnemyStateMachine stateMachine) : base(stateMachine)
        {
            _playerDetectionCollider = stateMachine.PlayerDetectionCollider;
        }

        public override void EnterState()
        {
            // Stop detecting player and resetting follow player state 
            _playerDetectionCollider.enabled = false;
        }
        

        public override void UpdateState()
        {
        }

        public override void ExitState()
        {
            
        }
    }
