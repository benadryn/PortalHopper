using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ProjectileBehaviour : MonoBehaviour, IAttackable
{
    [SerializeField] private float rotationModifier;
    [SerializeField] private float projectileSpeed = 5f;
    [SerializeField] private float bossProjectileSpeed = 8.2f;
    [SerializeField] private float blockedSpeedMultiplier = 2.2f;
    private float _normalRotationModifier;
    private float _negativeRotationModifier;
    private Transform _playerTransform;
    private PlayerController _playerController;
    private Rigidbody2D _projectileRb;
    
    public event Action OnBlockProjectile;

    private bool _isBlocked;

    private Enemy _owner;


    private void Start()
    {
        _playerController = PlayerController.Instance;
        _playerTransform = _playerController.transform;
        _projectileRb = GetComponent<Rigidbody2D>();
        _negativeRotationModifier = -rotationModifier;
        _normalRotationModifier = rotationModifier;
    }
    private void FixedUpdate()
    {
        if (!_isBlocked)
        {
            MoveTowardsPlayer();
        }

        if (_isBlocked)
        {
            var direction = (_owner.transform.position - transform.position).normalized;
            _projectileRb.velocity = direction * (projectileSpeed * blockedSpeedMultiplier);
        }
        ProjectileFaceDirectionOfTarget();
    }

    private void MoveTowardsPlayer()
    {
        var shootSpeed = _owner.CompareTag("Boss") ? bossProjectileSpeed : projectileSpeed;
        Vector2 direction = (_playerTransform.position - transform.position).normalized;
        _projectileRb.velocity = direction * shootSpeed;
    }
    public void BlockedProjectile()
    {
        _isBlocked = true;
        OnBlockProjectile?.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Ground")
        {
            ProjectileManager.Instance.ReturnProjectileToPool(gameObject);
        }

        if (other.gameObject == _owner.gameObject && _isBlocked)
        {
            _owner.Damage(1);
            ProjectileManager.Instance.ReturnProjectileToPool(gameObject);

        }
    }


    public void SetOwner(Enemy owner)
    {
        _owner = owner;
    }
    public void ResetIsBlocked()
    {
        _isBlocked = false;
    }
    
    public bool IsBlocked()
    {
        return _isBlocked;
    }
    
    private void ProjectileFaceDirectionOfTarget()
    {
        rotationModifier = _isBlocked ? _negativeRotationModifier : _normalRotationModifier;
        var directionToTarget = _playerTransform.position - transform.position;
        
        var angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg - rotationModifier;
        var q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * 20f);
        
    }

    public float GetProjectileDamageFromRange()
    {
        return Random.Range(1f, 2.5f);
    }
}
