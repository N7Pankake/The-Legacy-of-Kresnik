using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RegenHealthPotion", menuName = "Items/Regen Health Potion", order = 2)]
public class HealthRegenPotion : Item, IUseable
{

    [SerializeField]
    private int health;

    [SerializeField]
    private int regenTime;

    public void Use()
    {
        if (Player.MyInstance.MyHealth.MyCurrentValue < Player.MyInstance.MyHealth.MyMaxValue)
        {
            Remove();

            //This shows 
            Debug.Log("This happens 1");
            Player.MyInstance.StartCoroutine(Player.MyInstance.Regen(regenTime, health));
            //This shows
            Debug.Log("This happens 2");
        }
    }

    public override string GetDescription()
    {
        return base.GetDescription() + string.Format("\n<color=#00ff00ff>Use: Restores {0} health \nper {1} second(s)</color>", health, regenTime);
    }
}
