using System;
using System.Collections;
using UnityEngine;

public class EnemyHitParticleBehaviour : MonoBehaviour
{
    private HitEffectManager _hitEffectManager;
    private ParticleSystem _particleSystem;

    private bool _isInitialized;

    private void Awake()
    {
        _hitEffectManager = HitEffectManager.Instance;
        _particleSystem = GetComponent<ParticleSystem>();
    }

    private void Start()
    {
    }

    private void OnEnable()
    {
        if (_isInitialized)
        {
            StartCoroutine(PlayAndDetectEnd());
        }
        else
        {
            _isInitialized = true;
        }
    }

    private IEnumerator PlayAndDetectEnd()
    {
        _particleSystem.Play();

        yield return new WaitForSeconds(_particleSystem.main.duration - 0.1f);
        _particleSystem.Stop();
        _hitEffectManager.ReturnHitParticleToPool(gameObject);
    }
}
