using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider))]
public class Harvest : MonoBehaviour, IHarvestable
{
    
    private BoxCollider _collider;
    private QuestManager _questManager;
    [SerializeField] private float harvestTime = 1.5f;
    private string _id;
    [SerializeField] private Image minimapImage;
    

    private void Start()
    {
        _collider = GetComponent<BoxCollider>();
        _id = gameObject.tag;
        _collider.enabled = false;
        _questManager = QuestManager.Instance;

        _questManager.OnReceiveCollectQuest += OnReceiveCollectQuest;
    }

    private void OnReceiveCollectQuest(string id)
    {
        if (id == _id)
        {
            SetHarvestable();
        }
    }
    

    private void SetHarvestable()
    {
        _collider.enabled = true;
        minimapImage.gameObject.SetActive(true);
    }

    public (bool, float) Harvestable(Vector3 playerPosition, float harvestDistance)
    {
        float distance = Vector3.Distance(transform.position, playerPosition);
        if (distance <= harvestDistance)
        {
            return (true, harvestTime);
        }

        return (false, 0.0f);
    }
}
