using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    private UiManager _uiManager;
    private DisplayInventory _displayInventory;
    public List<QuestInfoSo> activeQuests = new List<QuestInfoSo>();
    public List<QuestInfoSo> completedQuests = new List<QuestInfoSo>();
    public bool questShowingOnUi;

    [SerializeField] private TextMeshProUGUI questDetailSide;

    public event Action<string> OnReceiveCollectQuest;
    
    [Header("Audio")] 
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private AudioClip acceptQuestAudio;
    [SerializeField] private AudioClip completeQuestAudio;
    
    public static event Action<string> OnPortalQuestReceived;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

    }


    private void Start()
    {
        _uiManager = UiManager.instance;
        _displayInventory = DisplayInventory.Instance;
    }
    

    public void ShowQuest(QuestInfoSo quest)
    {
        if (!activeQuests.Contains(quest) && AreRequiredQuestsCompleted(quest) && !questShowingOnUi)
        {
            _uiManager.UpdateQuestText(quest);
            questShowingOnUi = true;
        }
    }
    
    public void StartQuest(QuestInfoSo quest)
    {
        if (!activeQuests.Contains(quest) && questShowingOnUi)
        {
            quest.isActive = true;
            activeQuests.Add(quest);
            UpdateSideQuestDetails(activeQuests);
            questShowingOnUi = false;
            audioSource.PlayOneShot(acceptQuestAudio);
        }

        if (quest.objectiveTask == QuestInfoSo.ObjectiveTask.Collect)
        {
            OnReceiveCollectQuest?.Invoke(quest.id);
        }
        
        // specific to portal opening
        if (quest.id == "005")
        {
            OnPortalQuestReceived?.Invoke(quest.id);
        }
        
    }

    private bool AreRequiredQuestsCompleted(QuestInfoSo quest)
    {
        foreach (var requiredQuest in quest.requiredQuests)
        {
            if (!completedQuests.Contains(requiredQuest))
            {
                return false;
            }
        }

        return true;
    }

    public void AdvanceKillQuest(string questId, EnemyType enemyType)
    {
        foreach (var quest in activeQuests)
        {
            if (questId == quest.id && enemyType == quest.enemyType)
            {
                AdvanceQuest(quest);
            }
        }
    }

    public void AdvanceCollectQuest(string questId)
    {
        CheckIfQuestIsActiveAndAdvance(questId);
    }

    private void CheckIfQuestIsActiveAndAdvance(string questId)
    {
        foreach (var quest in activeQuests)
        {
            if (questId == quest.id)
            {
                AdvanceQuest(quest);
            }
        }
    }

    public void AdvancePortalQuest(string questId)
    {
        CheckIfQuestIsActiveAndAdvance(questId);
    }    
    private void AdvanceQuest(QuestInfoSo quest)
    {
        foreach (var objective in quest.objectives)
        {
            if (objective.currentAmount < objective.targetAmount)
            {
                objective.currentAmount++;
            }

            if (objective.currentAmount >= objective.targetAmount)
            {
                
                CompleteQuest(quest);
            }
        }
        UpdateSideQuestDetails(activeQuests);
    }

    private void CompleteQuest(QuestInfoSo quest)
    {
        if (activeQuests.Contains(quest))
        {
            quest.isCompleted = true;
        }
    }

    public void HandInQuest(QuestInfoSo quest)
    {
        if (quest.isActive)
        {
            PlayerExperience.XpGain?.Invoke(quest.experience);
            NPC.PlayGoodbyeSfx?.Invoke();
            audioSource.PlayOneShot(completeQuestAudio);
        }
        quest.isActive = false;
        quest.isHandedIn = true;
        completedQuests.Add(quest);
        activeQuests.Remove(quest);
        if (quest.questReward)
        {
            Item questItem = quest.questReward.CreateItem();
            _displayInventory.inventory.AddItem(questItem, quest.questRewardAmount);
            DisplayInventory.Instance.UpdateDisplay();
        }
        RemoveSideQuestDetails();
    }

    private void UpdateSideQuestDetails(List<QuestInfoSo> quests)
    {
        string questDetailsText = "";
        
        foreach (var quest in quests)
        {
            if (quest.isCompleted)
            {
                questDetailsText += $"{quest.questName} - Completed \n \n";
            }
            else
            {
                foreach (var objective in quest.objectives)
                {
                    questDetailsText += $"{quest.questName} - {objective.description} - {objective.currentAmount}/{objective.targetAmount}\n \n";
                }
            }
        }

        questDetailSide.text = questDetailsText;
    }

    private void RemoveSideQuestDetails()
    {
        UpdateSideQuestDetails(activeQuests);
    }
    
    
}
