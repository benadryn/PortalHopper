using System.Collections.Generic;
using UnityEngine;

public class HitEffectManager : MonoBehaviour
{
    [SerializeField] private GameObject hitParticlePrefab;
    [SerializeField] private int hitParticlePoolSize = 4;

    private readonly Queue<GameObject> _hitParticleEffectPool = new Queue<GameObject>();

    public static HitEffectManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializeParticlePool();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeParticlePool()
    {
        for (int i = 0; i < hitParticlePoolSize; i++)
        {
            var hitParticle = Instantiate(hitParticlePrefab, transform.position, Quaternion.identity);
            hitParticle.SetActive(false);
            _hitParticleEffectPool.Enqueue(hitParticle);
        }
    }

    public GameObject GetPooledHitParticle()
    {
        if (_hitParticleEffectPool.Count > 0)
        {
            var pooledHitParticle = _hitParticleEffectPool.Dequeue();
            
            pooledHitParticle.SetActive(true);
            return pooledHitParticle;
        }
        else
        {
            var newHitParticle = Instantiate(hitParticlePrefab, transform.position, Quaternion.identity);
            newHitParticle.SetActive(false);
            _hitParticleEffectPool.Enqueue(newHitParticle);
            return newHitParticle;
        }
    }

    public void ReturnHitParticleToPool(GameObject hitParticle)
    {
        hitParticle.SetActive(false);
        _hitParticleEffectPool.Enqueue(hitParticle);
    }}
