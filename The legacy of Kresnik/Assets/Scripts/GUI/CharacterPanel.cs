using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPanel : MonoBehaviour
{
    private static CharacterPanel instance;

    public static CharacterPanel MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<CharacterPanel>();
            }

            return instance;
        }
    }

    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    private CharacterButton armor, boots, gloves, helmet, necklace, pants, rings, shield, skillbook, sword, twohand;

    public CharacterButton MySelectedButton { get; set; }


    public void OpenClose()
    {
        if(canvasGroup.alpha <= 0)
        {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1;
        }
        else
        {
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0;
        }
    }

    public void EquipArmor(Armor gear)
    {
        switch (gear.MyArmorType)
        {
            case GearType.Armor:
                armor.EquipArmor(gear);
                break;

            case GearType.Boots:
                boots.EquipArmor(gear);
                break;

            case GearType.Gloves:
                gloves.EquipArmor(gear);
                break;

            case GearType.Helmet:
                helmet.EquipArmor(gear);
                break;

            case GearType.Necklace:
                necklace.EquipArmor(gear);
                break;

            case GearType.Pants:
                pants.EquipArmor(gear);
                break;

            case GearType.Ring:
                rings.EquipArmor(gear);
                break;

            case GearType.Shield:
                shield.EquipArmor(gear);
                break;

            case GearType.Skillbook:
                skillbook.EquipArmor(gear);
                break;

            case GearType.Sword:
                sword.EquipArmor(gear);
                break;

            case GearType.TwoHand:
                twohand.EquipArmor(gear);
                break;
        }
    }
}
