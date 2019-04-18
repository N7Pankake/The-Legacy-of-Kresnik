using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : NPC
{
    [SerializeField]
    private Quest[] quests;
    public Quest[] MyQuests
    {
        get
        {
            return quests;
        }
    }

    [SerializeField]
    private Sprite question, questionSilver, exclamation;

    [SerializeField]
    private SpriteRenderer statusRenderer;

    [SerializeField]
    private int questGiverID;
    public int MyQuestGiverID
    {
        get
        {
            return questGiverID;
        }
    }

    private List<string> completedQuest = new List<string>();
    public List<string> MyCompletedQuest
    {
        get
        {
            return completedQuest;
        }

        set
        {
            completedQuest = value;

            foreach (string title in completedQuest)
            {
                for (int i = 0; i < quests.Length; i++)
                {
                    if(quests[i] != null && quests[i].MyTitle == title)
                    {
                        quests[i] = null;
                    }
                }
            }
        }
    }

    private void Start()
    {
        foreach (Quest quest in quests)
        {
            quest.MyQuestGiver = this;
        }
    }

    public void UpdateQuestStatus()
    {
        int count = 0;

        foreach (Quest quest in quests)
        {
            if (quest != null)
            {
                if (quest.IsComplete && QuestLog.MyInstance.HasQuest(quest))
                {
                    statusRenderer.sprite = question;
                    break;
                }

                else if (!QuestLog.MyInstance.HasQuest(quest))
                {
                    statusRenderer.sprite = exclamation;
                    break;
                }

                else if (!quest.IsComplete && QuestLog.MyInstance.HasQuest(quest))
                {
                    statusRenderer.sprite = questionSilver;
                    break;
                }
                else
                {
                    count++;

                    if(count == quests.Length)
                    {
                        statusRenderer.enabled = false;
                    }
                }
            }
        }
    }
}
