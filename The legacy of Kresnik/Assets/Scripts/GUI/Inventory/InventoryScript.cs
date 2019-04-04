using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ItemCountChanged(Item item);

public class InventoryScript : MonoBehaviour
{
    public event ItemCountChanged itemCountChangedEvent;

    private static InventoryScript instance;

    public static InventoryScript MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<InventoryScript>();
            }
            return instance;
        }
    }

    public int MyEmptySlotCount
    {
        get
        {
            int count = 0;

            foreach(Bag bag in bags)
            {
                count += bag.MyBagScript.MyEmptySlotCount;
            }

            return count;
        }
    }

    public int MyTotalSlotCount
    {
        get
        {
            int count = 0;

            foreach (Bag bag in bags)
            {
                count += bag.MyBagScript.MySlots.Count;
            }
            return count;
        }
    }

    public int MyFullSlotCount
    {
        get
        {
            return MyTotalSlotCount - MyEmptySlotCount;
        }
    }

    private SlotScript fromSlot;

    public SlotScript FromSlot
    {
        get
        {
            return fromSlot;
        }

        set
        {
            fromSlot = value;
            if (value != null)
            {
                fromSlot.MyIcon.color = Color.grey;
            }
        }
    }

    private List<Bag> bags = new List<Bag>();

    [SerializeField]
    private BagButton[] bagButtons;

    //Debugging
    [SerializeField]
    private Item[] items;

    public bool CanAddBag
    {
        get { return bags.Count < 2; }
    }

    private void Awake()
    {
        Bag bag = (Bag)Instantiate(items[0]);
        bag.Initialize(16);
        bag.Use();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            Armor armorA = (Armor)Instantiate(items[1]);
            Armor armorP = (Armor)Instantiate(items[2]);
            Armor armorB = (Armor)Instantiate(items[3]);
            Armor armorE = (Armor)Instantiate(items[4]);
            Armor armorG = (Armor)Instantiate(items[5]);
            Armor armorC = (Armor)Instantiate(items[6]);
            Armor armorS = (Armor)Instantiate(items[7]);
            Item regenP = (Item)Instantiate(items[8]);
            AddItem(armorS);
            AddItem(armorA);
            AddItem(armorP);
            AddItem(armorB);
            AddItem(armorE);
            AddItem(armorG);
            AddItem(armorC);
            AddItem(regenP);

        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            Bag bag = (Bag)Instantiate(items[0]);
            bag.Initialize(16);
            AddItem(bag);
        }
    }

    public void AddBag(Bag bag)
    {
        foreach (BagButton bagButton in bagButtons)
        {
            if (bagButton.MyBag == null)
            {
                bagButton.MyBag = bag;
                bags.Add(bag);
                bag.MyBagButton = bagButton;
                bag.MyBagScript.transform.SetSiblingIndex(bagButton.MyBagIndex);
                break;
            }
        }
    }

    public void AddBag(Bag bag, BagButton bagButton)
    {
        bags.Add(bag);
        bagButton.MyBag = bag;
        bag.MyBagScript.transform.SetSiblingIndex(bagButton.MyBagIndex);
    }

    public void RemoveBag(Bag bag)
    {
        bags.Remove(bag);
        Destroy(bag.MyBagScript.gameObject);
    }

    public void SwapBags(Bag oldBag, Bag newBag)
    {
        int newSlotCount = (MyTotalSlotCount - oldBag.Slots) + newBag.Slots;

        if(newSlotCount - MyFullSlotCount >= 0)
        {
            List<Item> bagItems = oldBag.MyBagScript.GetItems();

            RemoveBag(oldBag);

            newBag.MyBagButton = oldBag.MyBagButton;

            newBag.Use();

            foreach (Item item in bagItems) // Makes sure we don't duplicate
            {
                if (item != newBag)
                {
                    AddItem(item);
                }
            }

            AddItem(oldBag);

            HandScript.MyInstance.Drop();

            MyInstance.fromSlot = null;
        }
    }

    public bool AddItem(Item item)
    {
        if(item.MyStackSize > 0)
        {
            if(PlaceInStack(item))
            {
                return true;
            }
        }
       return PlaceInEmpty(item);
    }

    private bool PlaceInEmpty(Item item)
    {
        foreach (Bag bag in bags)
        {
            if(bag.MyBagScript.AddItem(item))
            {
                OnItemCountChanged(item);
                return true;
            }
        }
        return false;
    }

    private bool PlaceInStack(Item item)
    {
        foreach(Bag bag in bags)
        {
            foreach(SlotScript slots in bag.MyBagScript.MySlots)
            {
                if(slots.StackItem(item))
                {
                    OnItemCountChanged(item);
                    return true;
                }
            }
        }
        return false;
    }

    public void OpenClose()
    {
        bool closedBag = bags.Find(x => !x.MyBagScript.IsOpen);

        foreach (Bag bag in bags)
        {
            if (bag.MyBagScript.IsOpen != closedBag)
            {
                bag.MyBagScript.OpenClose();
            }
        }
    }

    public Stack<IUseable> GetUseables (IUseable type)
    {
        Stack<IUseable> useables = new Stack<IUseable>();

        foreach (Bag bag in bags)
        {
            foreach(SlotScript slot in bag.MyBagScript.MySlots)
            {
                if(!slot.IsEmpty && slot.MyItem.GetType() == type.GetType())
                {
                    foreach(Item item in slot.MyItems)
                    {
                        useables.Push(item as IUseable);
                    }
                }
            }
        }

        return useables;
    }

    public void OnItemCountChanged(Item item)
    {
        if(itemCountChangedEvent != null)
        {
            itemCountChangedEvent.Invoke(item);
        }
    }
}
