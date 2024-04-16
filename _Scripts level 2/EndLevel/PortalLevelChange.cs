using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


public class PortalLevelChange : MonoBehaviour
{
    private AudioSource _audioSource;
    [SerializeField] private AudioClip portalContinuous;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        StartCoroutine(nameof(PlayPortalSound), 1f);
        
    }

    private IEnumerator PlayPortalSound(float time)
    {
        yield return new WaitForSeconds(time);
        _audioSource.clip = portalContinuous;
        _audioSource.loop = true;
        _audioSource.Play();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            LevelManager.Instance.LoadNextLevel();
        }
    }
}
