using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ManaPotion", menuName = "Items/Potion", order = 1)]
public class ManaPotion : Item, IUseable
{
    [SerializeField]
    private int mana;

    public void Use()
    {
        Remove();
        //Player.MyInstance.MyHealth.MyCurrentValue += mana;
    }
}
