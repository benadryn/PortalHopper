using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
   [Header("Attack state")]
   [SerializeField] private float attackDistance = 2f;
   public float AttackDistance => attackDistance;
   [SerializeField] private float moveSpeed = 10f;

   [field: Header("Death State")]
   public CircleCollider2D PlayerDetectionCollider { get; private set; }

   private EnemyState _currentState;
   private EnemyState _nextState;

   private PlayerController _playerController;
   private Animator _animator;
   private Rigidbody2D _rigidbody2D;
   private SpriteRenderer _spriteRenderer;
   private Collider2D _collider2D;

   private void Awake()
   {
      _animator = GetComponent<Animator>();
      _rigidbody2D = GetComponent<Rigidbody2D>();
      _playerController = FindObjectOfType<PlayerController>();
      _spriteRenderer = GetComponent<SpriteRenderer>();
      PlayerDetectionCollider = GetComponentInChildren<CircleCollider2D>();
      _collider2D = GetComponent<Collider2D>();
   }

   private void Start()
   {
      TransitionToState(new IdleState(this));
   }

   private void Update()
   {
      _currentState.UpdateState();
   }

   public void TransitionToState(EnemyState nextState)
   {
      if (_currentState != null)
      {
         _currentState.ExitState();
      }

      _currentState = nextState;
      _currentState.EnterState();
   }

   public PlayerController GetPlayerController()
   {
      return _playerController;
   }

   public Animator GetAnimator()
   {
      return _animator;
   }

   // public Rigidbody2D GetRigidbody()
   // {
   //    return _rigidbody2D;
   // }
   //
   // public SpriteRenderer GetSpriteRenderer()
   // {
   //    return _spriteRenderer;
   // }

   public void TransitionToChaseState()
   {
      if (_currentState != null)
      {
         _currentState.ExitState();
      }
      _currentState = new ChasePlayerState(this, _rigidbody2D, _spriteRenderer, attackDistance, moveSpeed, _collider2D);
      _currentState.EnterState();
   }

   public void ReturnToBaseState(EnemyState baseState)
   {
      if (_currentState != null)
      {
         _currentState.ExitState();
      }

      _currentState = baseState;
      _currentState.EnterState();
   }

   public void BeginEndState(EnemyState endState)
   {
      if (_currentState != null)
      {
         _currentState.ExitState();
      }

      _currentState = endState;
      _currentState.EnterState();
   }
}
