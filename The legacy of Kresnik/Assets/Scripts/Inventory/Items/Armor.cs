using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum GearType {Helmet, Necklace, Armor, Gloves, Pants, Boots, Ring, Sword, TwoHand, Skillbook, Shield}

[CreateAssetMenu(fileName = "Gear", menuName = "Items/Gear", order = 7)]
public class Armor : Item
{
    private static Armor instance;

    public static Armor MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Armor>();
            }
            return instance;
        }
    }

    private Quality itemQuality;

    [SerializeField]
    private GearType armorType;

    internal GearType MyArmorType
    {
        get
        {
            return armorType;
        }
    }

    [SerializeField]
    private int intellect;
    public int MyIntellect
    {
        get
        {
            return intellect;
        }
    }

    [SerializeField]
    private int strength;
    public int MyStrength
    {
        get
        {
            return strength;
        }
    }

    [SerializeField]
    private int vitality;
    public int MyVitality
    {
        get
        {
            return vitality;
        }
    }

    public override string GetDescription()
    {
        itemQuality = MyQuality;

        string stats = string.Empty;

        if(MyIntellect > 0)
        {
            stats += string.Format("\n<color=#0000FF> +{0} intellect</color>", MyIntellect);
        }

        if (MyStrength > 0)
        {
            stats += string.Format("\n<color=#FF0000> +{0} strength</color>", MyStrength);
        }

        if (MyVitality > 0)
        {
            stats += string.Format("\n<color=#00FF00> +{0} vitality</color>", MyVitality);
        }

        if ((MyVitality & MyIntellect & MyStrength) == 0)
        {
            return base.GetDescription() + string.Format("\n{0} {1} ", itemQuality, MyArmorType);
        }

        else
        {
            return base.GetDescription() + string.Format("\n{0} {1} \n", itemQuality, MyArmorType) + stats;
        }
    }

    public void Equip()
    {
        CharacterPanel.MyInstance.EquipArmor(this);

        float myOldManaValues = Player.MyInstance.MyMana.MyCurrentValue;
        float myOldHealthValues = Player.MyInstance.MyHealth.MyCurrentValue;

        float myNewManaValues = Player.MyInstance.MyMana.MyMaxValue += (MyIntellect * 10);
        float myNewHealthValues = Player.MyInstance.MyHealth.MyMaxValue += (MyVitality * 10);

        Player.MyInstance.MyMana.MyCurrentValue = Player.MyInstance.MyMana.MyMaxValue;
        Player.MyInstance.MyMana.MyCurrentValue = myOldManaValues;
        Player.MyInstance.MyHealth.MyCurrentValue = Player.MyInstance.MyHealth.MyMaxValue;
        Player.MyInstance.MyHealth.MyCurrentValue = myOldHealthValues;

        Player.MyInstance.MyAttackDamage += (MyStrength / 2);
    }
}