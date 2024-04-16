using System.Collections;
using UnityEngine;


[CreateAssetMenu(fileName = "Falling Death", menuName = "Enemy Logic/Death/Falling-Death")]
public class FallingDeath : EnemyDeathSOBase
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float fallSpeed = 4f;
    [SerializeField] private float distanceToStopFromGround = 0.6f;
    [SerializeField] private float deathDelay = 3f;

    private CircleCollider2D _circleCollider2D;
    private bool _isDead;
    private static readonly int IsDead = Animator.StringToHash("IsDead");

    public override void Initialize(GameObject gameObject, Enemy enemy)
    {
        base.Initialize(gameObject, enemy);
        _circleCollider2D = enemy.GetComponentInChildren<CircleCollider2D>();
    }

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        if (_isDead) return;
        if (Enemy.deathSfx)
        {
            Enemy.AudioSource.PlayOneShot(Enemy.deathSfx);
        }
        Enemy.Animator.SetBool(IsDead, true);
        if (_circleCollider2D)
        {
            _circleCollider2D.gameObject.SetActive(false);
        }
        Enemy.StartCoroutine(DeathCoroutine());
    }

    private IEnumerator DeathCoroutine()
    {
        var isFalling = true;

        while (isFalling)
        {
            isFalling = Fall();
            yield return new WaitForFixedUpdate();
        }
        Die(deathDelay);
    }

    private bool Fall()
    {
        RaycastHit2D groundHit = Physics2D.Raycast(Enemy.transform.position, UnityEngine.Vector2.down, Mathf.Infinity,
            groundLayer);

        if (groundHit.collider)
        {
            var distanceToGround = groundHit.distance;
            var effectiveFallSpeed = fallSpeed * Mathf.Clamp01(distanceToGround / distanceToStopFromGround);

            Enemy.transform.position += Vector3.down * (effectiveFallSpeed * Time.deltaTime);

            if (distanceToGround <= distanceToStopFromGround)
            {
                _isDead = true;
                return false;
            }
        }

        return true;
    }

    private void Die(float delay)
    {
        Destroy(Enemy.gameObject, delay);
    }
}


