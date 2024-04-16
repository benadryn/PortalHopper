using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultDeath : MonoBehaviour, IDeathBehaviour
{
    [SerializeField] private float deathDelay = 3.0f;

    public void StartDeath()
    {
        StartCoroutine(Die(deathDelay));
    }

    private IEnumerator Die(float delay) {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
        
    }
}
