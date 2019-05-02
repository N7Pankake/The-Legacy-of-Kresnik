using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private static PlayerStats instance;

    public static PlayerStats MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerStats>();
            }
            return instance;
        }
    }

    [SerializeField]
    private TextMeshProUGUI health, mana, damage, arcana, level, intellect, vitality, strength, Speed;
    private int myArmorStrength;
    public int MyArmorStrength { get => myArmorStrength; set => myArmorStrength = value; }

    private int myArmorIntellect;
    public int MyArmorIntellect { get => myArmorIntellect; set => myArmorIntellect = value; }

    private int myArmorVitality;
    public int MyArmorVitality { get => myArmorVitality; set => myArmorVitality = value; }

    private int myExtraSpeed;
    public int MyExtraSpeed { get => myExtraSpeed; set => myExtraSpeed = value; }


    // Update is called once per frame
    void Update()
    {

        health.text = "Health: " + Player.MyInstance.MyHealth.MyMaxValue;
        mana.text = "Mana: " + Player.MyInstance.MyMana.MyMaxValue;
        damage.text = "Damage: " + (10 + (Player.MyInstance.MyStrength / 3)) + " +[" + (Player.MyInstance.MyAttackDamage - (10 + (Player.MyInstance.MyStrength / 3)))+ "]";
        arcana.text ="Arcana: "+ (Player.MyInstance.MyIntellect/3) + " +[" + Player.MyInstance.MyArcanaDamage + "]";
        level.text = "Level: " + Player.MyInstance.MyLevel;

        intellect.text = string.Format("Intellect: [{0}] ", Player.MyInstance.MyIntellect) + "<color=#0000FF>+[" + MyArmorIntellect + "]</color>";
        strength.text = string.Format("Strength: [{0}] ", Player.MyInstance.MyStrength) + "<color=#FF0000>+[" + MyArmorStrength + "]</color>";
        vitality.text = string.Format("Vitality: [{0}] ", Player.MyInstance.MyVitality) + "<color=#00FF00>+[" + MyArmorVitality + "]</color>";

        Speed.text = "Speed: " + 4 + " +[" + MyExtraSpeed + "]";
    }
}

//Intellect: <color=#0000FF>({0})</color>
//    Strength: <color=#FF0000>({0})</color>
//    Vitality: <color=#00FF00>({0})</color>
