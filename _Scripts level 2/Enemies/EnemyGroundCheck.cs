using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroundCheck : MonoBehaviour
{
    [SerializeField] private float groundDistance = 0.1f;
    [SerializeField] private LayerMask groundLayer;
    

    public bool IsGrounded()
    {
        var hit = Physics2D.Raycast(transform.position, Vector2.down, groundDistance, groundLayer);

        return hit.collider;
    }
    
}
