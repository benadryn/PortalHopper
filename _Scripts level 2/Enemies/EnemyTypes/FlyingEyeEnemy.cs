using UnityEngine;

public class FlyingEyeEnemy : Enemy
{
    private GameObject _projectile;
    private ProjectileBehaviour _projectileBehaviour;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == _projectile && _projectileBehaviour.IsBlocked())
        {
            Damage(1);
            ProjectileManager.Instance.ReturnProjectileToPool(_projectile);
        }
    }
}
