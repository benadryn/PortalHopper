using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalRealmChange : MonoBehaviour
{
    private LevelManager _levelManager;
    [SerializeField] private GameObject endPortal;
    private Collider _portalCollider;

    private void Start()
    {
        _portalCollider = GetComponent<Collider>();
        _levelManager = FindObjectOfType<LevelManager>();
    }

    private void OnEnable()
    {
        QuestManager.OnPortalQuestReceived += ActivatePortal;
    }

    private void OnDisable()
    {
        QuestManager.OnPortalQuestReceived -= ActivatePortal;
    }

    private void ActivatePortal(string questId)
    {
        if (CompareTag(questId))
        {
            if (!endPortal) return;
            // _portalCollider.enabled = true;
            endPortal.gameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        QuestManager.Instance.AdvancePortalQuest(tag);
       _levelManager.LoadNextLevel();
    }
}
