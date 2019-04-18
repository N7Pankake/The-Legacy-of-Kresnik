using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    [SerializeField]
    private string title;
    public string MyTitle
    {
        get
        {
            return title;
        }

        set
        {
            title = value;
        }
    }

    [SerializeField]
    private string description;
    public string MyDescription
    {
        get
        {
            return description;
        }

        set
        {
            description = value;
        }
    }

    [SerializeField]
    private int level;
    public int MyLevel
    {
        get
        {
            return level;
        }

        set
        {
            level = value;
        }
    }

    [SerializeField]
    private int xp;
    public int MyXp
    {
        get
        {
            return xp;
        }
    }

    [SerializeField]
    private CollectObjective[] collectObjectives;
    public CollectObjective[] MyCollectObjectives
    {
        get
        {
            return collectObjectives;
        }
    }

    [SerializeField]
    private KillObjective[] killObjectives;
    public KillObjective[] MyKillObjectives
    {
        get
        {
            return killObjectives;
        }

        set
        {
            killObjectives = value;
        }
    }

    public QuestScript MyQuestScript { get; set;}
    public QuestGiver MyQuestGiver { get; set;}

    public bool IsComplete
    {
        get
        {
            foreach (Objective objective in collectObjectives)
            {
                if(!objective.IsComplete)
                {
                    return false;
                }
            }

            foreach (Objective objective in killObjectives)
            {
                if (!objective.IsComplete)
                {
                    return false;
                }
            }

            return true;
        }
    }
}

[System.Serializable]
public abstract class Objective
{
    [SerializeField]
    private int amount;
    public int MyAmount
    {
        get
        {
            return amount;
        }
    }

    private int currentAmount;
    public int MyCurrentAmount
    {
        get
        {
            return currentAmount;
        }

        set
        {
            currentAmount = value;
        }
    }

    [SerializeField]
    private string type;
    public string MyType
    {
        get
        {
            return type;
        }
    }

    public bool IsComplete
    {
        get
        {
            return MyCurrentAmount >= MyAmount;
        }
    }
}

[System.Serializable]
public class CollectObjective : Objective
{
    public void UpdateItemCount(Item item)
    {
        if(MyType.ToLower() == item.MyTitle.ToLower())
        {
            MyCurrentAmount = InventoryScript.MyInstance.GetItemCount(item.MyTitle);

            if(MyCurrentAmount <= MyAmount)
            {
                MessageFeedManager.MyInstance.WriteMessage(string.Format("{0}: {1}/{2}", item.MyTitle, MyCurrentAmount, MyAmount));
            }

            QuestLog.MyInstance.CheckCompletion();
            QuestLog.MyInstance.UpdateSelected();
        }
    }

    public void UpdateItemCount()
    {
        MyCurrentAmount = InventoryScript.MyInstance.GetItemCount(MyType);
        QuestLog.MyInstance.CheckCompletion();
        QuestLog.MyInstance.UpdateSelected();
    }

    public void Complete()
    {
        Stack<Item> items = InventoryScript.MyInstance.GetItems(MyType, MyAmount);

        foreach (Item item in items)
        {
            item.Remove();
        }
    }
}

[System.Serializable]
public class KillObjective : Objective
{
    public void UpdateKillCount(Character character)
    {
        if (MyType == character.MyType)
        {
            if (MyCurrentAmount < MyAmount)
            {
                MyCurrentAmount++;

                MessageFeedManager.MyInstance.WriteMessage(string.Format("{0}: {1}/{2}", character.MyType, MyCurrentAmount, MyAmount));

                QuestLog.MyInstance.CheckCompletion();
                QuestLog.MyInstance.UpdateSelected();
            }
        }
    }
}

