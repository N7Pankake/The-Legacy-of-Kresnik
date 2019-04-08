using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VendorItem
{
    [SerializeField]
    private Item item;

    public Item MyItem
    {
        get
        {
            return item;
        }
    }

    [SerializeField]
    private int quantity;

    public int MyQuantity
    {
        get
        {
            return quantity;
        }

        set
        {
            quantity = value;
        }
    }

    [SerializeField]
    private bool unlimited;

    public bool Unlimited
    {
        get
        {
            return unlimited;
        }
    }
}
