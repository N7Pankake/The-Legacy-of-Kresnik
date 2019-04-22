using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootableChest : MonoBehaviour, IInteractable
{
    [SerializeField]
    private SpriteRenderer spriteRender;

    [SerializeField]
    private Sprite openSprite, closedSprite;

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
    private bool lootExist;

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
            isOpen = true;
            spriteRender.sprite = openSprite;
            
            List<Drop> drops = new List<Drop>();
            foreach (IInteractable interactable in Player.MyInstance.MyInteractables)
            {
                drops.AddRange((interactable as LootableChest).lootTable.GetLoot());
            }
            LootWindow.MyInstance.CreatePages(drops);
        }
    }

    public void StopInteract()
    {
        if (isOpen)
        {
            isOpen = false;
            spriteRender.sprite = closedSprite;
        }
    }
}

