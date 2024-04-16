using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    
    [SerializeField] private float moveSpeed = 20.0f;
    [SerializeField] private float jumpForce = 200.0f;
    [SerializeField] private float fallJumpForce = 7.5f;
    [SerializeField] private LayerMask layerToCheckGrounded;
    [SerializeField] private float jumpBufferTime = 0.2f;
    [SerializeField] private float fallJumpBufferTime = 0.2f;
 
    private float _currentMovement;

    private AudioSource _audioSource;
    private PlayerControls _playerControls;
    private InputAction _moveAction;
    private InputAction _jumpAction;
    private bool _isJumping;
    private bool _isGrounded;
    // private bool _jumpBuffered;
    private bool _fallJumpBuffered;
    private float _lastGroundedTime;
    
    private Rigidbody2D _rb;
    private CircleCollider2D _groundCheckCollider;

    
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        InitializeInputActions();
    }

    private void InitializeInputActions()
    {
        _playerControls = new PlayerControls();
        _moveAction = _playerControls._2dPlayer.Movement;
        _jumpAction = _playerControls._2dPlayer.Jump;

        _moveAction.performed += ctx => OnMovePerformed(ctx.ReadValue<float>());
        _moveAction.canceled += _ => OnMoveCanceled();

        _jumpAction.performed += _ => OnJumpPerformed();
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _groundCheckCollider = GetComponent<CircleCollider2D>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnJumpPerformed()
    {
        if (_isGrounded && !_isJumping)
        {
            _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            _isJumping = true;
        }
        else if (_fallJumpBuffered && !_isJumping)
        {
            _rb.AddForce(Vector2.up * fallJumpForce, ForceMode2D.Impulse);
            _isJumping = true;
            _fallJumpBuffered = false;
        }
        else
        {
            StartCoroutine(JumpBuffer());
        }

        _isJumping = true;
    }

    private IEnumerator JumpBuffer()
    {
        // _jumpBuffered = true;
        yield return new WaitForSeconds(jumpBufferTime);
        if (_isGrounded)
        {
            _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            _isJumping = true;
        }
        // _jumpBuffered = false;
    }


    private void OnMovePerformed(float movementValue)
    {
        _currentMovement = movementValue;
    }


    private void OnMoveCanceled()
    {
        _currentMovement = 0.0f;
        _audioSource.Stop();
    }


    private void FixedUpdate()
    {
        HandleMovement();
        CheckIfFallingOffPlatform();
        if (_rb.velocity.y < 0 && _isGrounded)
        {
            _isJumping = false;
        }
    }
    

    private void HandleMovement()
    {
        _rb.velocity = new Vector2(_currentMovement * moveSpeed, _rb.velocity.y);
    }

    public bool IsGrounded()
    {
        var bounds = _groundCheckCollider.bounds;
        _isGrounded = Physics2D.Raycast(bounds.center, Vector2.down,
            bounds.extents.y + 0.1f, layerToCheckGrounded);

        if (_isGrounded)
        {
            _lastGroundedTime = Time.time;
        }

        return _isGrounded;
    }

    private void CheckIfFallingOffPlatform()
    {
        if (!_isGrounded)
        {
            if (!_isJumping && Time.time - _lastGroundedTime <= fallJumpBufferTime)
            {
                _fallJumpBuffered = true;
            }
            else
            {
                _fallJumpBuffered = false;
            }
        }
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
