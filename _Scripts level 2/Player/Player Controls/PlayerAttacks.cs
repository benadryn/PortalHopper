using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class PlayerAttacks : MonoBehaviour
{
    private PlayerControls _playerControls;
    private InputAction _attackLight;
    private InputAction _attackBlock;

    [SerializeField] private int rayCount = 30;
    [SerializeField] private float lightAttackArcAngle = 20.0f;
    [SerializeField] private float blockAttackArcAngle = 90.0f;
    [SerializeField] private float rayLength = 2f;
    [SerializeField] private LayerMask enemyLayerMask;
    [SerializeField] private float attackDelay = 0.75f;
    private float _attackTimer = 0f;
    private float _arcAngle;
    
    private bool _isLightAttacking;
    private bool _isBlockAttacking;
    private bool _isAttacking;

    private HitEffectManager _hitEffectManager;
    
    private AudioSource _audioSource;
    [SerializeField] private AudioClip blockAttackSfx;
    [SerializeField] private AudioClip lightAttackSfx;

    
    void Awake()
    {
        InitializeInputActions();
        _hitEffectManager = HitEffectManager.Instance;
    }

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void InitializeInputActions()
    {
        _playerControls = new PlayerControls();
        _attackLight = _playerControls._2dPlayer.Attack1;
        _attackBlock = _playerControls._2dPlayer.Attack2;
        
        _attackLight.performed += OnLightAttack;
        _attackBlock.performed += OnBlockAttack;

    }
    

    private void OnBlockAttack(InputAction.CallbackContext obj)
    {
        if (!_isAttacking && _attackTimer <= 0f)
        {
            _audioSource.PlayOneShot(blockAttackSfx);
            _isBlockAttacking = true;
            _isAttacking = true;
            
            _attackTimer = attackDelay;
        }
    }


    private void OnLightAttack(InputAction.CallbackContext ctx)
    {
        if (!_isAttacking && _attackTimer <= 0f)
        {
            _audioSource.PlayOneShot(lightAttackSfx);
            _isLightAttacking = true;
            _isAttacking = true;
            
            _attackTimer = attackDelay;
        }
        
    }

    private void Update()
    {
        if (_attackTimer > 0f)
        {
            _attackTimer -= Time.deltaTime;
            if (_attackTimer <= 0f)
            {
                _isLightAttacking = false;
                _isBlockAttacking = false;
                _isAttacking = false;
            }
        }
    }

    public void CastArcRay()
    {
            
        _arcAngle = _isLightAttacking ? lightAttackArcAngle : (_isBlockAttacking ? blockAttackArcAngle : lightAttackArcAngle);

        var angleStep = _arcAngle / rayCount;
        
        var forwardDirection = Vector3.right;
        if (transform.localRotation.y > 0)
        {
            forwardDirection = -forwardDirection;
        }

        var hitEnemies = new HashSet<Enemy>();
        var hitProjectiles = new List<ProjectileBehaviour>();

        for (var i = 0; i < rayCount; i++)
        {
            
            var angle = i * angleStep - _arcAngle / 2.0f;
            var direction = Quaternion.Euler(0, 0, angle) * forwardDirection;

            var endPosition = transform.position + direction * rayLength;
            
            var hit = Physics2D.Raycast(transform.position, direction, rayLength, enemyLayerMask);

            if (hit.collider)
            {
                Debug.DrawLine(transform.position, endPosition, Color.red, 3f);
                var enemy = hit.collider.GetComponent<Enemy>();
                var projectile = hit.collider.GetComponent<ProjectileBehaviour>();
                if (enemy && _isLightAttacking)
                {
                    
                    hitEnemies.Add(enemy);
                }
                
                if (projectile && _isBlockAttacking)
                {
                    hitProjectiles.Add(projectile);
                }
            }

        }
        
        foreach (var enemy in hitEnemies)
        {
            var chance = Random.Range(1, 11);
            if (enemy.HasBlockChance() && chance <= enemy.BlockChancePercentage() * 0.1f)
            {
                enemy.BlockAttack();
            }
            else
            {
                enemy.Damage(1);
                
            }
        }

        foreach (var projectile in hitProjectiles)
        {
            projectile.BlockedProjectile();
        }


    }
    
    public bool IsBlockAttacking()
    {
        return _isBlockAttacking;
    }

    public void ResetBlockAttack()
    {
        _isBlockAttacking = false;
        _isAttacking = false;
    }
    
    public bool IsLightAttacking()
    {
        return _isLightAttacking;
    }
    
    public void ResetLightAttack()
    {
        _isLightAttacking = false;
        _isAttacking = false;
    }
    
    private void OnEnable()
    {
        _playerControls.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Disable();
    }
}
