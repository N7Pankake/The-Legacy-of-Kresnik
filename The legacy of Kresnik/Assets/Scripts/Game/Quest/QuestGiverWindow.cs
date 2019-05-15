using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestGiverWindow : Window
{
    private static QuestGiverWindow instance;
    public static QuestGiverWindow MyInstance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<QuestGiverWindow>();
            }
            return instance;
        }
    }

    [SerializeField]
    private GameObject backBtn, acceptBtn, completeBtn, questDescription;

    private QuestGiver questGiver;

    [SerializeField]
    private Transform questArea;

    [SerializeField]
    private GameObject questPrefab;

    private List<GameObject> quests = new List<GameObject>();

    private Quest selectedQuest;
    
    public void ShowQuests(QuestGiver questGiver)
    {
        this.questGiver = questGiver;

        foreach(GameObject go in quests)
        {
            Destroy(go);
        }

        questArea.gameObject.SetActive(true);
        questDescription.SetActive(false);

        foreach (Quest quest in questGiver.MyQuests)
        {
            if(quest != null)
            {
                GameObject go = Instantiate(questPrefab, questArea);
                go.GetComponent<TextMeshProUGUI>().text = "<size=50>[" + quest.MyLevel+ "] " + quest.MyTitle + "<color=#ffbb04> !</color></size>";

                go.GetComponent<QGQScript>().MyQuest = quest;

                quests.Add(go);

                if (QuestLog.MyInstance.HasQuest(quest) && quest.IsComplete)
                {
                    go.GetComponent<TextMeshProUGUI>().text = "<size=50>[" + quest.MyLevel + "] " + quest.MyTitle + "<color=#FFFF00> ?</color></size>";
                }

                else if (QuestLog.MyInstance.HasQuest(quest))
                {
                    Color color = go.GetComponent<TextMeshProUGUI>().color;
                    color.a = 0.5f;

                    go.GetComponent<TextMeshProUGUI>().color = color;
                    go.GetComponent<TextMeshProUGUI>().text = "<size=50>[" + quest.MyLevel + "] " + quest.MyTitle + "<color=#C0C0C0> ?</color></size>";
                }
            }
        }
    }

    public override void OpenWindow(NPC npc)
    {
        ShowQuests(npc as QuestGiver);
        base.OpenWindow(npc);
    }

    public void ShowQuestInfo(Quest quest)
    {
        this.selectedQuest = quest;

        if (QuestLog.MyInstance.HasQuest(quest) && quest.IsComplete)
        {
            acceptBtn.SetActive(false);
            completeBtn.SetActive(true);
        }
        else if (!QuestLog.MyInstance.HasQuest(quest))
        {
            acceptBtn.SetActive(true);
        }

        backBtn.SetActive(true);
        questArea.gameObject.SetActive(false);
        questDescription.SetActive(true);

        string objectives = string.Empty;
  
        foreach (Objective obj in quest.MyCollectObjectives)
        {
            objectives += "<size=40>" + obj.MyType + "</size>: \n<size=35>" + obj.MyCurrentAmount + "/" + obj.MyAmount + " </size>\n";
        }

        foreach (KillObjective obj in quest.MyKillObjectives)
        {
            objectives += "<size=40>" + obj.MyType + "</size>: \n<size=35>" + obj.MyCurrentAmount + "/" + obj.MyAmount + " </size>\n";
        }

        questDescription.GetComponent<TextMeshProUGUI>().text = string.Format("<b>{0}\n</b>\n{1}\n\n{2}", quest.MyTitle, quest.MyDescription, objectives);
    }

    public void Back()
    {
        backBtn.SetActive(false);
        acceptBtn.SetActive(false);

        ShowQuests(questGiver);

        completeBtn.SetActive(false);
    }

    public void Accept()
    {
        QuestLog.MyInstance.AcceptQuest(selectedQuest);
        Back();
    }

    public override void CloseWindow()
    {
        completeBtn.SetActive(false);
        base.CloseWindow();
    }

    public void CompleteQuest()
    {
        if (selectedQuest.IsComplete)
        {
            for (int i = 0; i < questGiver.MyQuests.Length; i++)
            {
                if (selectedQuest == questGiver.MyQuests[i])
                {
                    questGiver.MyCompletedQuest.Add(selectedQuest.MyTitle);
                    questGiver.MyQuests[i] = null;
                    selectedQuest.MyQuestGiver.UpdateQuestStatus();
                }
            }

            foreach (CollectObjective objective in selectedQuest.MyCollectObjectives)
            {
                InventoryScript.MyInstance.itemCountChangedEvent -= new ItemCountChanged(objective.UpdateItemCount);
                objective.Complete();
            }

            foreach (KillObjective objective in selectedQuest.MyKillObjectives)
            {
                GameManager.MyInstance.killConfirmedEvent -= new KillConfirmed(objective.UpdateKillCount);
            }

            Player.MyInstance.GainXp(XPManager.CalculateXP(selectedQuest));

            QuestLog.MyInstance.RemoveQuest(selectedQuest.MyQuestScript);
            Back();
        }
    }
}
