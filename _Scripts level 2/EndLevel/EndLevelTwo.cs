using System;
using UnityEngine;

public class EndLevelTwo : MonoBehaviour
{
    public static EndLevelTwo Instance;

    [SerializeField] private GameObject endPortal;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OpenPortal()
    {
        endPortal.SetActive(true);
    }
}
