using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable, IEnemyAttackBehaviour
{

    [field: SerializeField] public float MaxHealth { get; set; }
    public float CurrentHealth { get; set; }
    [field: SerializeField] public float attackDistance = 2f;
    [field: SerializeField] private bool hasBlockChance;
    [Tooltip("Percentage")][SerializeField] private int blockChance = 20;
    [SerializeField] private float damageAmount;
 
    public Rigidbody2D Rigidbody2D { get; private set; }
    public CircleCollider2D PlayerDetectionCollider { get; private set; }
    public Animator Animator { get; private set; }
    public SpriteRenderer SpriteRenderer { get; private set; }
    public CapsuleCollider2D CapsuleCollider2D { get; private set; }
    
    public AudioSource AudioSource { get; private set; }

    private bool _isDead;

    private HitEffectManager _hitEffectManager;

    #region State Machine Variables

    public NewEnemyStateMachine StateMachine { get; private set; }
    public EnemyIdleState IdleState { get; private set; }
    public EnemyChaseState ChaseState { get; private set; }
    public EnemyAttackState AttackState { get; private set; }
    
    public EnemyDamagedState DamagedState { get; private set; }
    public EnemyDeathState DeathState { get; private set; }

    #endregion

    #region Scriptable Object Variables

    [SerializeField] private EnemyIdleSOBase enemyIdleBase;
    [SerializeField] private EnemyChaseSOBase enemyChaseBase;
    [SerializeField] private EnemyAttackSOBase enemyAttackBase;
    [SerializeField] private EnemyDamagedSOBase enemyDamagedBase;
    [SerializeField] private EnemyDeathSOBase enemyDeathBase;
    

    public EnemyIdleSOBase EnemyIdleBaseInstance { get; private set; }
    public EnemyChaseSOBase EnemyChaseBaseInstance { get; private set; }
    public EnemyAttackSOBase EnemyAttackBaseInstance { get; private set; }
    public EnemyDamagedSOBase EnemyDamagedBaseInstance { get; private set; }
    public EnemyDeathSOBase EnemyDeathBaseInstance { get; private set; }
    
    #endregion
    
    [Header("Audio Enemy")]
    public AudioClip alertSfx;
    public AudioClip attackSfx;
    public AudioClip deathSfx;
    public AudioClip runSfx;
    public AudioClip damageSfx;
    public AudioClip blockedSfx;
    
    private static readonly int IsBlocking = Animator.StringToHash("IsBlocking");

    private PlayerController _playerController;
    
    private void Awake()
    {
        EnemyIdleBaseInstance = Instantiate(enemyIdleBase);
        EnemyChaseBaseInstance = Instantiate(enemyChaseBase);
        EnemyAttackBaseInstance = Instantiate(enemyAttackBase);
        EnemyDamagedBaseInstance = Instantiate(enemyDamagedBase);
        EnemyDeathBaseInstance = Instantiate(enemyDeathBase);
        
        StateMachine = new NewEnemyStateMachine();
        IdleState = new EnemyIdleState(this, StateMachine);
        ChaseState = new EnemyChaseState(this, StateMachine);
        AttackState = new EnemyAttackState(this, StateMachine);
        DamagedState = new EnemyDamagedState(this, StateMachine);
        DeathState = new EnemyDeathState(this, StateMachine);
    }

    private void Start()
    {
        CurrentHealth = MaxHealth;
        Animator = GetComponent<Animator>();
        Rigidbody2D = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        CapsuleCollider2D = GetComponent<CapsuleCollider2D>();
        PlayerDetectionCollider = GetComponentInChildren<CircleCollider2D>();
        AudioSource = GetComponent<AudioSource>();
        _playerController = PlayerController.Instance;
        _hitEffectManager = HitEffectManager.Instance;

        EnemyIdleBaseInstance.Initialize(gameObject, this);
        EnemyChaseBaseInstance.Initialize(gameObject, this);
        EnemyAttackBaseInstance.Initialize(gameObject, this);
        EnemyDamagedBaseInstance.Initialize(gameObject, this);
        EnemyDeathBaseInstance.Initialize(gameObject, this);
        
        StateMachine.Initialize(IdleState);

    }

    private void Update()
    {
        StateMachine.CurrentEnemyState.FrameUpdate();

    }

    private void FixedUpdate()
    {
        StateMachine.CurrentEnemyState.PhysicsUpdate();
    }

    public void Damage(float damageAmount)
    {
        if (!_isDead)
        {
            CurrentHealth -= damageAmount;
            var hitEffect = _hitEffectManager.GetPooledHitParticle();
            hitEffect.transform.position = transform.position;
        }
        if (CurrentHealth <= 0f)
        {
            _isDead = true;
            StartDeathState();
            return;
        }
        StateMachine.ChangeState(DamagedState);
    }

    public void StartDeathState()
    {
        StateMachine.ChangeState(DeathState);
    }

    #region Animation Triggers

    private void AnimationTriggerEvent(AnimationTriggerType triggerType)
    {
        StateMachine.CurrentEnemyState.AnimationTriggerEvent(triggerType);
    }
    
    public enum AnimationTriggerType
    {
        EnemyDamaged,
        EnemyAttack,
        EnemyAttackHit,
        EnemyAttackEnd,
    }

    #endregion

    public float CheckPlayerDistance(Vector3 playerPos)
    {
        return (playerPos - transform.position).sqrMagnitude;
        
    }
    
    public void StartAttack(PlayerController playerController)
    {
        if (alertSfx)
        {
            AudioSource.PlayOneShot(alertSfx);
        }
        StateMachine.ChangeState(ChaseState);
    }

    public void StopAttack()
    {
        if (!_isDead)
        {
            StateMachine.ChangeState(IdleState);
        }
    }

    public void BlockAttack()
    {
        StateMachine.ChangeState(IdleState);
        Animator.SetBool(IsBlocking, true);
        if (blockedSfx)
        {
            AudioSource.Stop();
            AudioSource.PlayOneShot(blockedSfx);
        }
    }

    public void AfterBlockTransition()
    {
        if (_isDead) return;
        if (IsInRangeForAttack())
        {
            StateMachine.ChangeState(AttackState);
        }
        else
        {
            StateMachine.ChangeState(ChaseState);
        }
    }

    private bool IsInRangeForAttack()
    {
        var playerPos = _playerController.transform.position;
        var distance = CheckPlayerDistance(playerPos);
        return distance <= attackDistance * attackDistance;
    }

    public void ResetBlock()
    {
        Animator.SetBool(IsBlocking, false);
        AfterBlockTransition();
    }

    public void ResetDamageAnimation()
    {
        StateMachine.ChangeState(ChaseState);
    }

    public bool IsDead()
    {
        return _isDead;
    }
    
    public void FlipSprite(Vector3 playerPos)
    {
        SpriteRenderer.flipX = playerPos.x < transform.position.x;
    }

    public int BlockChancePercentage()
    {
        return blockChance;
    }
    
    public bool HasBlockChance()
    {
        return hasBlockChance;
    }

    public float GetDamageAmount()
    {
        return damageAmount;
    }
}