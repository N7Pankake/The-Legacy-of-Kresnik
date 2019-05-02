using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class CharacterButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private GearType armoryType;

    [SerializeField]
    private Image icon;

    [SerializeField]
    private Player player;

    private Armor equippedArmor;
    public Armor MyEquippedArmor
    {
        get
        {
            return equippedArmor;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (HandScript.MyInstance.MyMoveable is Armor)
            {
                Armor tmp = (Armor)HandScript.MyInstance.MyMoveable;

                if (tmp.MyArmorType == armoryType)
                {
                    Player.MyInstance.MyOldHealthValues = Player.MyInstance.MyHealth.MyCurrentValue;
                    Player.MyInstance.MyOldManaValues = Player.MyInstance.MyMana.MyCurrentValue;

                    Player.MyInstance.MyNewHealthValues = Player.MyInstance.MyHealth.MyMaxValue += (MyEquippedArmor.MyVitality * 10);
                    Player.MyInstance.MyNewManaValues = Player.MyInstance.MyMana.MyMaxValue += (MyEquippedArmor.MyIntellect * 5);

                    Player.MyInstance.MyHealth.MyCurrentValue += (MyEquippedArmor.MyVitality * 10);

                    Player.MyInstance.MyMana.MyCurrentValue += (MyEquippedArmor.MyIntellect * 5);

                    Player.MyInstance.MyAttackDamage += (MyEquippedArmor.MyStrength / 3);
                    Player.MyInstance.MyArcanaDamage += (MyEquippedArmor.MyIntellect / 3);

                    PlayerStats.MyInstance.MyArmorStrength += MyEquippedArmor.MyArmorStrength;
                    PlayerStats.MyInstance.MyArmorIntellect += MyEquippedArmor.MyArmorIntellect;
                    PlayerStats.MyInstance.MyArmorVitality += MyEquippedArmor.MyArmorVitality;

                    EquipArmor(tmp);
                }

                UIManager.MyInstance.RefreshTooltip(tmp);
            }

            else if (HandScript.MyInstance.MyMoveable == null && MyEquippedArmor != null)
            {
                HandScript.MyInstance.TakeMoveable(MyEquippedArmor);
                CharacterPanel.MyInstance.MySelectedButton = this;
                icon.color = Color.grey;
            }
        }
    }

    public void EquipArmor(Armor armor)
    {
        armor.Remove();

        if (MyEquippedArmor != null)
        {
            player = GetComponent<Player>();

            Player.MyInstance.MyMana.MyMaxValue -= (MyEquippedArmor.MyArmorIntellect * 5);
            Player.MyInstance.MyHealth.MyMaxValue -= (MyEquippedArmor.MyArmorVitality * 10);

            Player.MyInstance.MyMana.MyCurrentValue = Player.MyInstance.MyMana.MyMaxValue;
            Player.MyInstance.MyMana.MyCurrentValue = Player.MyInstance.MyOldManaValues;

            Player.MyInstance.MyHealth.MyCurrentValue = Player.MyInstance.MyHealth.MyMaxValue;
            Player.MyInstance.MyHealth.MyCurrentValue = Player.MyInstance.MyOldHealthValues;

            Player.MyInstance.MyAttackDamage -= (MyEquippedArmor.MyArmorStrength / 3);
            Player.MyInstance.MyArcanaDamage -= (MyEquippedArmor.MyArmorIntellect / 3);

            PlayerStats.MyInstance.MyArmorStrength -= MyEquippedArmor.MyArmorStrength;
            PlayerStats.MyInstance.MyArmorIntellect -= MyEquippedArmor.MyArmorIntellect;
            PlayerStats.MyInstance.MyArmorVitality -= MyEquippedArmor.MyArmorVitality;

            if (MyEquippedArmor != armor)
            {
                armor.MySlot.AddItem(MyEquippedArmor);
            }

            UIManager.MyInstance.RefreshTooltip(MyEquippedArmor);
        }
        else
        {
            UIManager.MyInstance.HideToolTip();
        }

        icon.enabled = true;
        icon.sprite = armor.MyIcon;
        icon.color = Color.white;
        this.equippedArmor = armor;
        this.equippedArmor.MyCharButton = this;

        if (HandScript.MyInstance.MyMoveable == (armor as IMoveable))
        {
            HandScript.MyInstance.Drop();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (MyEquippedArmor != null)
        {
            UIManager.MyInstance.ShowTooltip(new Vector2(0, 0), transform.position, MyEquippedArmor);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.MyInstance.HideToolTip();
    }

    public void DequipArmor()
    {
        icon.color = Color.white;
        icon.enabled = false;

        player = GetComponent<Player>();

        Player.MyInstance.MyMana.MyMaxValue -= (MyEquippedArmor.MyArmorIntellect * 5);
        Player.MyInstance.MyHealth.MyMaxValue -= (MyEquippedArmor.MyArmorVitality * 10);

        Player.MyInstance.MyMana.MyCurrentValue = Player.MyInstance.MyMana.MyMaxValue;
        Player.MyInstance.MyMana.MyCurrentValue = Player.MyInstance.MyOldManaValues;

        Player.MyInstance.MyHealth.MyCurrentValue = Player.MyInstance.MyHealth.MyMaxValue;
        Player.MyInstance.MyHealth.MyCurrentValue = Player.MyInstance.MyOldHealthValues;

        Player.MyInstance.MyAttackDamage -= (MyEquippedArmor.MyArmorStrength / 3);
        Player.MyInstance.MyArcanaDamage -= (MyEquippedArmor.MyArmorIntellect / 3);

        PlayerStats.MyInstance.MyArmorStrength -= MyEquippedArmor.MyArmorStrength;
        PlayerStats.MyInstance.MyArmorIntellect -= MyEquippedArmor.MyArmorIntellect;
        PlayerStats.MyInstance.MyArmorVitality -= MyEquippedArmor.MyArmorVitality;

        equippedArmor.MyCharButton = null;
        equippedArmor = null;
    }
}