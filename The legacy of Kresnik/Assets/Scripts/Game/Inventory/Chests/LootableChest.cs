using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootableChest : LootTable, IInteractable
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Sprite nonLootable;

    [SerializeField]
    private Sprite spriteLootable;

    private void Start()
    {
        RollLoot();
    }

    protected override void RollLoot()
    {
        MyDroppedItems = new List<Drop>();

        foreach (Loot l in loot)
        {
            int roll = Random.Range(0, 100);

            if (roll <= l.MyDropChance)
            {
                MyDroppedItems.Add(new Drop(l.MyItem, this));
                spriteRenderer.sprite = spriteLootable;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void Interact()
    {
        Player.MyInstance.Loot("Loot", MyDroppedItems);
        LootWindow.MyInstance.MyInteractable = this;
    }

    public void StopInteract()
    {
        LootWindow.MyInstance.MyInteractable = null;

        if(MyDroppedItems.Count == 0)
        {
            spriteRenderer.sprite = nonLootable;
            gameObject.SetActive(false);
        }

        LootWindow.MyInstance.Close();
    }
}