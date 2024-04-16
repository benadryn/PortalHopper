using System;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private GameObject cameraFollowGo;
    
    private PlayerAttacks _playerAttacks;
    private PlayerController _playerController;
    private Animator _animator;
    private Rigidbody2D _rb;
    private CameraFollowObject _cameraFollowObject;

    
    private float _movementSpeed;
    private float _lastMovementDirection = 1f;
    
    private static readonly int IsMoving = Animator.StringToHash("IsMoving");
    private static readonly int IsJumping = Animator.StringToHash("IsJumping");
    private static readonly int IsFalling = Animator.StringToHash("IsFalling");
    private static readonly int IsLightAttacking = Animator.StringToHash("IsLightAttacking");
    private static readonly int IsHeavyAttacking = Animator.StringToHash("IsHeavyAttacking");
    private static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
    private static readonly int IsDamaged = Animator.StringToHash("IsDamaged");
    private static readonly int IsDead = Animator.StringToHash("IsDead");

    public bool IsFacingRight { get; private set; }

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _playerAttacks = GetComponent<PlayerAttacks>();
        _playerController = GetComponent<PlayerController>();
        _cameraFollowObject = cameraFollowGo.gameObject.GetComponent<CameraFollowObject>();
    }
    


    private void Update()
    {
        UpdateAnimations();
    }


    private void UpdateAnimations()
    {
        // check grounded
        _animator.SetBool(IsGrounded, _playerController.IsGrounded());
        
        // check if Moving
        _movementSpeed = _rb.velocity.x;

        FlipSprite();
        
        _animator.SetBool(IsMoving, Mathf.Abs(_movementSpeed) > 0.1f);
        
        // check if jumping
        var jumping = _rb.velocity.y;
        if (_playerController.IsGrounded())
        {
            jumping = 0f;
        }
        
        _animator.SetBool(IsJumping, jumping > 0.1f);
        _animator.SetBool(IsFalling, jumping < -0.1f);
        
        // check Attacking

        var isLightAttacking = _playerAttacks.IsLightAttacking();
        _animator.SetBool(IsLightAttacking, isLightAttacking);
    
        var isHeavyAttacking = _playerAttacks.IsBlockAttacking();
        _animator.SetBool(IsHeavyAttacking, isHeavyAttacking);
    }

    private void FlipSprite()
    {
        if (Mathf.Abs(_movementSpeed) > 0.1f)
        {
            _lastMovementDirection = Mathf.Sign(_movementSpeed);
        }

        var newRotation = transform.rotation.eulerAngles;

        if (_lastMovementDirection < 0)
        {
            newRotation.y = 180f;
            IsFacingRight = true;
            _cameraFollowObject.CallTurn();
        }
        else if (_lastMovementDirection > 0)
        {
            newRotation.y = 0f;
            IsFacingRight = false;


            _cameraFollowObject.CallTurn();
        }
        
        transform.rotation = Quaternion.Euler(newRotation);
        
    }
    

    public void PlayDamageAnimation()
    {
        _animator.SetBool(IsDamaged, true);
    }

    public void ResetDamagedBool()
    {
        _animator.SetBool(IsDamaged, false);
    }

    public void PlayDeathAnimation()
    {
        _animator.SetBool(IsDead, true);
    }
    
}
