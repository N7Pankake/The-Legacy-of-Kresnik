﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum GearType {Helmet, Necklace, Armor, Gloves, Pants, Boots, Ring, Sword, TwoHand, Skillbook, Shield}

[CreateAssetMenu(fileName = "Gear", menuName = "Items/Gear", order = 4)]
public class Armor : Item
{
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

    [SerializeField]
    private int strength;

    [SerializeField]
    private int vitality;

    public override string GetDescription()
    {
        string stats = string.Empty;

        if(intellect > 0)
        {
            stats += string.Format("\n +{0} intellect", intellect);
        }

        if (strength > 0)
        {
            stats += string.Format("\n +{0} strength", strength);
        }

        if (vitality > 0)
        {
            stats += string.Format("\n +{0} vitality", vitality);
        }

        if ((vitality & intellect & strength) == 0)
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
    }
}