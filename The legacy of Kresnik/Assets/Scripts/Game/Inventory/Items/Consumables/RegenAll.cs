using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Regen potion", menuName = "Items/Regen All", order = 5)]
public class RegenAll : Item, IUseable
{

    [SerializeField]
    private int mana;

    [SerializeField]
    private int health;

    [SerializeField]
    private int regenTime;

    public void Use()
    {
        if (Player.MyInstance.MyMana.MyCurrentValue < Player.MyInstance.MyMana.MyMaxValue || Player.MyInstance.MyHealth.MyCurrentValue < Player.MyInstance.MyHealth.MyMaxValue)
        {
            Remove();
            Player.MyInstance.StartCoroutine(Player.MyInstance.RegenAll(regenTime, mana, health));
        }
    }

    public override string GetDescription()
    {
        return base.GetDescription() + string.Format("\n<color=#00ff00ff>Use: Restores {0} mana and {1} health\nper {2} second(s)</color>", mana, health, regenTime);
    }
}
