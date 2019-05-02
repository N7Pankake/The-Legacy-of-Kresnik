using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum Type { Void, Ice, Earth, Fire}
public enum Effect { Damage, Heal, Regen, Dps, Stun}

[Serializable]
public class  Skill : IUseable, IMoveable, IDescribable
{

    [SerializeField]
    private string name;
    public string MyName
    {
        get
        {
            return name;
        }
    }

    [SerializeField]
    private Type type;

    [SerializeField]
    private Effect effect;

    [SerializeField]
    private string description;

    [SerializeField]
    private int damage;
    public int MyDamage
    {
        get
        {
            return damage;
        }
    }

    [SerializeField]
    private int myManaCost;
    public int MyManaCost
    {
        get
        {
            return myManaCost;
        }
    }

    [SerializeField]
    private int skillSpeed;
    public int MySkillSpeed
    {
        get
        {
            return skillSpeed;
        }
    }

    [SerializeField]
    private Sprite icon;
    public Sprite MyIcon
    {
        get
        {
            return icon;
        }
    }

    [SerializeField]
    private float castTime;
    public float MyCastTime
    {
        get
        {
            return castTime;
        }
    }

    [SerializeField]
    private GameObject skillPrefab;
    public GameObject MySkillPrefab
    {
        get
        {
            return skillPrefab;
        }
    }

    [SerializeField]
    private Color barColor;
    public Color MyBarColor
    {
        get
        {
            return barColor;
        }
    }

    private int time = 5;

    [SerializeField]
    public SkillScript skillScript;

    public virtual string GetDescription()
    {
        string color = string.Empty;
        int extraManaCost = 0;

        int arcanaDamage = (Player.MyInstance.MyArcanaDamage + (Player.MyInstance.MyIntellect / 3));

        switch (type)
        {
            case Type.Void:
                color = "#FF6DDE";
                break;
            case Type.Ice:
                color = "#38ACE7";
                break;
            case Type.Earth:
                color = "#00800B";
                break;
            case Type.Fire:
                color = "#E43700";
                break;
        }

        if(effect == Effect.Damage)
        {
            //Debug.Log(effect);
            skillScript.myEffect = effect;
            return string.Format("<color={0}>{1}</color>\nCast time: [{2}] second(s)\nMana cost: {6} +[{7}]\n{3}\nthat deals [{4}] damage \n+ Arcana [{5}] ", color, name, castTime, description, damage, arcanaDamage, myManaCost, (extraManaCost = (2 * (Player.MyInstance.MyArcanaDamage))));
        }

        else if(effect == Effect.Heal)
        {
            skillScript.myEffect = effect;
            return string.Format("<color={0}>{1}</color>\nCast time: [{2}] second(s)\nMana cost: {6} +[{7}]\n{3}\nthat heals [{4}] hp \n+ Arcana [{5}]", color, name, castTime, description, damage, (arcanaDamage / 2), myManaCost, (extraManaCost = extraManaCost + (2 * (Player.MyInstance.MyArcanaDamage / 2))));
        }

        else if (effect == Effect.Regen)
        {
            skillScript.myEffect = effect;
            return string.Format("<color={0}>{1}</color>\nCast time: [{2}] second(s)\nMana cost: {6} +[{7}]\n{3}\nthat heals [{4}] hp every 10(s) \n+ Arcana [{5}]", color, name, castTime, description, damage, (arcanaDamage / 3), myManaCost, (extraManaCost = extraManaCost + (6 * (Player.MyInstance.MyArcanaDamage / 3))));
        }

        else if (effect == Effect.Dps)
        {
            skillScript.myEffect = effect;
            return string.Format("<color={0}>{1}</color>\nCast time: [{2}] second(s)\nMana cost: {6} +[{7}]\n{3}\nthat deals [{4}] \n+ Arcana [{5}] for every 5(s)", color, name, castTime, description, damage, (arcanaDamage / 3), myManaCost, (extraManaCost = extraManaCost + (6 * (Player.MyInstance.MyArcanaDamage / 3))));
        }

        else if (effect == Effect.Stun)
        {
            skillScript.myEffect = effect;
            return string.Format("<color={0}>{1}</color>\nCast time: [{2}] second(s)\nMana cost: {6} +[{7}]\n{3}\nthat stuns for [{4}] \n+ Arcana [{5}] second(s)", color, name, castTime, description, damage, (arcanaDamage / 9), myManaCost, (extraManaCost = extraManaCost + (18 * (Player.MyInstance.MyArcanaDamage / 9))));
        }

        return null;
    }

    public void Use()
    {
        Player.MyInstance.CastSkill(MyName, effect, myManaCost);
    }
}