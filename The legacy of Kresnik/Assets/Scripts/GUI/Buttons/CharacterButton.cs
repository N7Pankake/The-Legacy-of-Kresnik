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

    private Armor equippedArmor;
    public Armor MyEquippedArmor
    {
        get
        {
            return equippedArmor;
        }
    }

    public void EquipArmor(Armor armor)
    {
        armor.Remove();

        if (MyEquippedArmor != null)
        {
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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Armor tmp = (Armor)HandScript.MyInstance.MyMoveable;

            if (tmp.MyArmorType == armoryType)
            {
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


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (MyEquippedArmor != null)
        {
            UIManager.MyInstance.ShowTooltip(new Vector2(0,0),transform.position, MyEquippedArmor);
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

        equippedArmor.MyCharButton = null;
        equippedArmor = null;
    }
}
