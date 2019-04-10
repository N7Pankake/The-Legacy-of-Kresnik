using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mana potion", menuName = "Items/Mana Potion", order = 3)]
public class ManaPotion : Item, IUseable
{
    [SerializeField]
    private int mana;

    public void Use()
    {
        if (Player.MyInstance.MyMana.MyCurrentValue < Player.MyInstance.MyMana.MyMaxValue)
        {
            Remove();
            Player.MyInstance.MyMana.MyCurrentValue += mana;
        }
    }

    public override string GetDescription()
    {
        return base.GetDescription() + string.Format("\n<color=#0000ff>Use: Restores {0} mana</color>", mana);
    }
}
