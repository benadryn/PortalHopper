using System.Collections;
using UnityEngine;

public class ProjectileAttack : MonoBehaviour, IEnemyAttackBehaviour, IAttackable
{
    private EnemyBehaviour _enemyBehaviour;
    private PlayerController _playerController;
    private ProjectileManager _projectileManager;
    private ProjectileBehaviour _projectileBehaviour;
    private HandleDamage _handleDamage;
    
    [SerializeField] private float projectileSpeed = 10.0f;
    [SerializeField] private float secondsForNextAttack = 3.0f;
    [SerializeField] private float projectileBlockedMultiplier = 5.0f;
    
    private bool _isAttacking;
    private bool _isResetCoroutineRunning;
    private bool _isDead;
    private bool _playerLeftRange;
    
    private Rigidbody2D _projectileRb;
    private GameObject _projectile;
    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxCollider;
    private Transform _shooter;
    

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _enemyBehaviour = GetComponent<EnemyBehaviour>();


    }

    private void Start()
    {
        _projectileManager = ProjectileManager.Instance;
        _handleDamage = HandleDamage.Instance;
        
        _handleDamage.OnProjectileReturnedToPool += OnProjectileReturnedToPool;
    }

    public void StartAttack(PlayerController playerController)
    {
        if (_isAttacking || _isDead) return;
        
        _playerController = playerController;
        _playerLeftRange = false;
        FlipSprite();
        
        _isAttacking = true;

        StartCoroutine(PerformAttack());
    }

    public void StopAttack()
    {
        _playerLeftRange = true;
    }

    private IEnumerator PerformAttack()
    {
            var position = transform.position;

            _projectile = _projectileManager.GetPooledProjectile();
            _projectile.transform.position = position;
        
            _projectileRb = _projectile.GetComponent<Rigidbody2D>();
        
            // get the projectileBehaviour from the projectile
            _projectileBehaviour = _projectile.GetComponent<ProjectileBehaviour>();
            if (_projectileBehaviour)
            {
                _projectileBehaviour.OnBlockProjectile += BlockedProjectile;
            }
        
            
            var playerControllerTransform = _playerController.transform;
            var direction = (playerControllerTransform.position - position).normalized;
        
            _projectileRb.velocity = direction * projectileSpeed;

            _shooter = transform;
            
            
            yield return StartCoroutine(FollowPlayerProjectile(playerControllerTransform));
            
            if (!_isResetCoroutineRunning  && !_isDead && !_projectileBehaviour.IsBlocked())
            {
                StartCoroutine(ResetAttack(secondsForNextAttack));
            }
    }


    private IEnumerator FollowPlayerProjectile(Transform playerTransform)
    {
        while (_projectile != null && !_projectileBehaviour.IsBlocked())
        {
            var direction = (playerTransform.position - _projectile.transform.position).normalized;

            _projectileRb.velocity = direction * projectileSpeed;
            FlipSprite();
            yield return new WaitForFixedUpdate();
        }
    }
    
    private IEnumerator ResetAttack(float delay)
    {
        _isResetCoroutineRunning = true;
        FlipSprite();
       
        yield return new WaitForSeconds(delay);
                
        if (_projectile && !_projectileBehaviour.IsBlocked())
        {
            _projectileManager.ReturnProjectileToPool(_projectile);
            OnProjectileReturnedToPool();
        }
        _isAttacking = false;
        _isResetCoroutineRunning = false;
        if (!_playerLeftRange)
        {
            StartAttack(_playerController);
        }
    }

    public void BlockedProjectile()
    {
        if (_projectile != null && this)
        {
            StartCoroutine(UpdateBlockedProjectile());
        }

    }

    private IEnumerator UpdateBlockedProjectile()
    {
        while (_projectile && gameObject && _projectileBehaviour.IsBlocked())
        {
            var direction = (_shooter.position - _projectile.transform.position).normalized;
            _projectileRb.velocity = direction * (projectileSpeed * projectileBlockedMultiplier);
            
            yield return new WaitForFixedUpdate();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == _projectile && _projectileBehaviour.IsBlocked())
        {
            Die();
        }
    }

    private void Die()
    {
        _isDead = true;
        _boxCollider.enabled = false;
        _enemyBehaviour.Die();
        _projectileManager.ReturnProjectileToPool(_projectile);
    }

    private void FlipSprite()
    {
        _spriteRenderer.flipX = _playerController.transform.position.x < transform.position.x;
    }

    private void OnProjectileReturnedToPool()
    {
        _projectileRb = null;
        _projectile = null;
    }
    
}

