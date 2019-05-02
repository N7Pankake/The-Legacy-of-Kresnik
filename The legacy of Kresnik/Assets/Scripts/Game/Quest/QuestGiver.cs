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
    private Sprite miniquestion, miniQuestionSilver, miniExclamation;

    [SerializeField]
    private SpriteRenderer statusRenderer;

    [SerializeField]
    private SpriteRenderer minimapRenderer;

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
                    minimapRenderer.sprite = miniquestion;
                    break;
                }

                else if (!QuestLog.MyInstance.HasQuest(quest))
                {
                    statusRenderer.sprite = exclamation;
                    minimapRenderer.sprite = miniExclamation;
                    break;
                }

                else if (!quest.IsComplete && QuestLog.MyInstance.HasQuest(quest))
                {
                    statusRenderer.sprite = questionSilver;
                    minimapRenderer.sprite = miniQuestionSilver;
                }
            }
            else
            {
                count++;
                if (count == quests.Length)
                {
                    statusRenderer.enabled = false;
                    minimapRenderer.enabled = false;
                }
            }
        }
    }
}
