using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private int projectilePoolSize = 8;

    private readonly Queue<GameObject> _projectilePool = new Queue<GameObject>();
    
    // singleton Pattern
    public static ProjectileManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializeProjectilePool();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeProjectilePool()
    {
        for (int i = 0; i < projectilePoolSize; i++)
        {
            var projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            projectile.SetActive(false);
            _projectilePool.Enqueue(projectile);
        }
    }

    public GameObject GetPooledProjectile()
    {
        if (_projectilePool.Count > 0)
        {
            var pooledProjectile = _projectilePool.Dequeue();
            
            pooledProjectile.SetActive(true);
            return pooledProjectile;
        }

        var newProjectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        newProjectile.SetActive(false);
        _projectilePool.Enqueue(newProjectile);
        return newProjectile;
    }

    public void ReturnProjectileToPool(GameObject projectile)
    {
        projectile.SetActive(false);
        projectile.GetComponent<ProjectileBehaviour>().ResetIsBlocked();
        _projectilePool.Enqueue(projectile);
    }
}
