using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mp regen potion", menuName = "Items/Regen Mana Potion", order = 4)]
public class RegenManaPotion : Item, IUseable
{

    [SerializeField]
    private int mana;

    [SerializeField]
    private int regenTime;

    public void Use()
    {
        if (Player.MyInstance.MyMana.MyCurrentValue < Player.MyInstance.MyMana.MyMaxValue)
        {
            Remove();
            Player.MyInstance.StartCoroutine(Player.MyInstance.RegenMP(regenTime, mana));
        }
    }

    public override string GetDescription()
    {
        return base.GetDescription() + string.Format("\n<color=#00ff00ff>Use: Restores {0} mana \nper {1} second(s)</color>", mana, regenTime);
    }
}
