using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SCTType { Damage, Heal}

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

        string operation = string.Empty;

        switch (type)
        {
            case SCTType.Damage:
                operation += "-";
                sct.color = Color.red;
                break;
            case SCTType.Heal:
                operation += "+";
                sct.color = Color.green;
                break;
        }

        sct.text = operation + text;

        if (crit)
        {
            sct.GetComponent<Animator>().SetBool("Crit", crit);
        }
    }
}
