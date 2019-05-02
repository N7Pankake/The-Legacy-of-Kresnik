using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SCTType { Damage, DamageMana, Heal, Mana, XP, Buff, Gold}

public class CombatTextManager : MonoBehaviour
{
    private static CombatTextManager instance;

    public static CombatTextManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CombatTextManager>();
            }
            return instance;
        }
    }

    [SerializeField]
    private GameObject combatTextPrefab;

    public void CreateText(Vector2 position, string text, SCTType type, bool crit)
    {
        //Offset
        position.y += 0.6f;

         Text sct = Instantiate(combatTextPrefab, transform).GetComponent<Text>();
         sct.transform.position = position;

        string before = string.Empty;
        string after = string.Empty;
        switch (type)
        {
            case SCTType.Damage:
                before += "-";
                sct.color = Color.red;
                break;
            case SCTType.DamageMana:
                before += "-";
                sct.color = Color.blue;
                break;
            case SCTType.Heal:
                before += "+";
                sct.color = Color.green;
                break;
            case SCTType.Mana:
                before += "+";
                sct.color = Color.blue;
                break;
            case SCTType.Buff:
                before += "Speed +";
                sct.color = Color.grey;
                break;
            case SCTType.XP:
                before += "Xp +";
                sct.color = Color.yellow;
                break;
            case SCTType.Gold:
                before += "Gold +";
                sct.color = Color.yellow;
                break;
        }

        sct.text = before + text + after;

        if (crit)
        {
            sct.GetComponent<Animator>().SetBool("Crit", crit);
        }
    }
}
