using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealthPotion", menuName = "Items/Potion", order = 1)]
public class HealthPotion : Item, IUseable
{
    [SerializeField]
    private int health;
    public void Use()
    {
        //Remove if statement to punish the player for using the potions at full hp
        if (Player.MyInstance.MyHealth.MyCurrentValue < Player.MyInstance.MyHealth.MyMaxValue)
        {
            Remove();
            Player.MyInstance.MyHealth.MyCurrentValue += health;
        }
    }
}
