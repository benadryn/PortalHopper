
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    private PlayerController _target;
    private IEnemyAttackBehaviour _attackBehaviour;

    private void Start()
    {
        _attackBehaviour = GetComponentInParent<IEnemyAttackBehaviour>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _target = other.GetComponent<PlayerController>();
            if (_target)
            {
                _attackBehaviour?.StartAttack(_target);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _attackBehaviour?.StopAttack();
        }
    }
}
