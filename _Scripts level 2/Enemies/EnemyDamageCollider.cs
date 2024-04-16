using UnityEngine;

public class EnemyDamageCollider : MonoBehaviour
{
    private BoxCollider2D _boxCollider;
    private void Start()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        SetDamageColliderActive(false);
    }

    public void SetDamageColliderActive(bool active)
    {
        _boxCollider.enabled = active;
    }
    
}
