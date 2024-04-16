using System.Collections;
using UnityEngine;

public class FlyingDeath : MonoBehaviour, IDeathBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float fallSpeed = 0.2f;

    
    [SerializeField] private float distanceFromGround = 2.0f;

    private CircleCollider2D _circleCollider2D;

    [HideInInspector] public bool IsDead { private set; get; }

    private void Awake()
    {
        _circleCollider2D = GetComponentInChildren<CircleCollider2D>();
    }

    public void StartDeath()
    {
        if (IsDead) return;
        _circleCollider2D.gameObject.SetActive(false);
        
        StartCoroutine(DeathCoroutine());
    }    
    private IEnumerator DeathCoroutine()
    {
        var isFalling = true;

        while (isFalling)
        {
            isFalling = Fall();
            yield return new WaitForFixedUpdate();
        }

        Die(3.0f);
    }
    

    private bool Fall()
    {
        RaycastHit2D groundHit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, groundLayer);
        
        if (groundHit.collider)
        {
            
            var distanceToGround = groundHit.distance;
            var effectiveFallSpeed = fallSpeed *  Mathf.Clamp01(distanceToGround / distanceFromGround);

            transform.position += Vector3.down * (effectiveFallSpeed * Time.deltaTime);
            

            if (distanceToGround <= distanceFromGround)
            {
                IsDead = true;
                return false;
            }
        }

        return true;
    }

    private void Die(float delay)
    {
        Destroy(gameObject, delay);
    }
}
