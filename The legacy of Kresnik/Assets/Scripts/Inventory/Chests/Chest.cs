using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    [SerializeField]
    private SpriteRenderer spriteRender;

    [SerializeField]
    private Sprite openSprite, closedSprite;

    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    private LootTable lootTable;

    [SerializeField]
    private Transform minimapIndicator;

    private List<Item> items = new List<Item>();
    public List<Item> MyItems
    {
        get
        {
            return items;
        }

        set
        {
            items = value;
        }
    }

    private bool isOpen;

    [SerializeField]
    private BagScript bag;
    public BagScript MyBag
    {
        get
        {
            return bag;
        }

        set
        {
            bag = value;
        }
    }

    private void Awake()
    {
        items = new List<Item>();
    }

    public void Interact()
    {
        if (isOpen)
        {
            StopInteract();
        }

        else
        {
            AddItems();
            isOpen = true;
            spriteRender.sprite = openSprite;
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
        }
    }

    public void StopInteract()
    {
        if(isOpen)
        {
            StoreItems();
            MyBag.Clear();
            isOpen = false;
            spriteRender.sprite = closedSprite;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0;
        }
    }

    public void AddItems()
    {
        if (MyItems != null)
        {
            foreach (Item item in MyItems)
            {
                item.MySlot.AddItem(item);
            }
        }
    }

    public void StoreItems()
    {
        MyItems = MyBag.GetItems();
    }


}
