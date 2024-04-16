using System;
using UnityEngine;

public class AoeFireBehaviour : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2.5f;

    private PlayerController _playerController;
    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _spriteRenderer;
    private Vector3 _direction;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _playerController = PlayerController.Instance;
        _direction = (_playerController.transform.position - transform.position).normalized;
    }

    void Update()
    {
        MoveTowardsPlayer();
    }

    private void MoveTowardsPlayer()
    {
        _spriteRenderer.flipX = (_direction.x < 0);
        _rigidbody2D.velocity = _direction * moveSpeed;
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
