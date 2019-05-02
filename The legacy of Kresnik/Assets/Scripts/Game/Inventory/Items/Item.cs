using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : ScriptableObject, IMoveable, IDescribable
{
    [SerializeField]
    private Sprite icon;

    public Sprite MyIcon
    {
        get
        {
            return icon;
        }
    }

    [SerializeField]
    private int stackSize;

    public int MyStackSize
    {
        get
        {
            return stackSize;
        }
    }

    [SerializeField]
    private string title;

    public string MyTitle
    {
        get
        {
            return title;
        }
    }



    [SerializeField]
    private Quality quality;

    public Quality MyQuality
    {
        get
        {
            return quality;
        }
    }

    private SlotScript slot;

    public SlotScript MySlot
    {
        get
        {
            return slot;
        }

        set
        {
            slot = value;
        }
    }

    private CharacterButton charButton;

    public CharacterButton MyCharButton
    {
        get
        {
            return charButton;
        }

        set
        {

            MySlot = null;
            charButton = value;
        }
    }

    [SerializeField]
    private int price;

    public int MyPrice
    {
        get
        {
            return price;
        }
    }

    public virtual string GetDescription()
    {
        return string.Format("<color={0}>{1}</color>", QualityColor.MyColors[MyQuality], MyTitle);
    }

    public void Remove()
    {
        if(MySlot != null)
        {
            MySlot.RemoveItem(this);
        }
    }
}
