using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Collect : MonoBehaviour
{
    private Collider _collider;
    private QuestManager _questManager;
    private string _id;
    [SerializeField] private Image minimapImage;

    private void Start()
    {
        _collider = GetComponent<Collider>();
        _questManager = QuestManager.Instance;
        _id = gameObject.tag;
        
        _questManager.OnReceiveCollectQuest += OnReceiveCollectQuest;
    }
    

    private void OnReceiveCollectQuest(string id)
    {
        if (id == _id)
        {
            SetCollectable();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            QuestManager.Instance.AdvanceCollectQuest(GetQuestId());
            Destroy(gameObject);
        }
    }

    private string GetQuestId()
    {
        return gameObject.tag;
    }

    private void SetCollectable()
    {
        _collider.enabled = true;
        minimapImage.gameObject.SetActive(true);
    }
}
