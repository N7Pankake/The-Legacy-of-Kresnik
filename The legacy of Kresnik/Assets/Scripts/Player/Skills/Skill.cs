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

    [SerializeField]
    private Type type;

    [SerializeField]
    private Effect effect;

    [SerializeField]
    private string description;

    [SerializeField]
    private int damage;

    [SerializeField]
    private int heal;

    [SerializeField]
    private Sprite icon;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float castTime;

    [SerializeField]
    private GameObject skillPrefab;

    [SerializeField]
    private Color barColor;

    public string MyName
    {
        get
        {
            return name;
        }
    }

    public int MyDamage
    {
        get
        {
            return damage;
        }
    }

    public int MyHeal
    {
        get
        {
            return heal;
        }
    }

    public Sprite MyIcon
    {
        get
        {
            return icon;
        }
    }

    public float MySpeed
    {
        get
        {
            return speed;
        }
    }

    public float MyCastTime
    {
        get
        {
            return castTime;
        }
    }

    public GameObject MySkillPrefab
    {
        get
        {
            return skillPrefab;
        }
    }

    public Color MyBarColor
    {
        get
        {
            return barColor;
        }
    }
    
    public virtual string GetDescription()
    {
        string color = string.Empty;

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
            return string.Format("<color={0}>{1}</color>\nCast time: {2} second(s)\n{3}\nthat deals {4} damage", color, name, castTime, description, damage);
        }

        else if(effect == Effect.Heal)
        {
            return string.Format("<color={0}>{1}</color>\nCast time: {2} second(s)\n{3}\nthat heals {4}hp", color, name, castTime, description, damage);
        }

        else if (effect == Effect.Regen)
        {
            return string.Format("<color={0}>{1}</color>\nCast time: {2} second(s)\n{3}\nthat heals {4}hp per second", color, name, castTime, description, damage);
        }

        else if (effect == Effect.Dps)
        {
            return string.Format("<color={0}>{1}</color>\nCast time: {2} second(s)\n{3}\nthat deals {4} dps", color, name, castTime, description, damage);
        }

        else if (effect == Effect.Stun)
        {
            return string.Format("<color={0}>{1}</color>\nCast time: {2} second(s)\n{3}\nthat stuns for {4} second(s)", color, name, castTime, description, damage);
        }

        return null;
    }


    public void Use()
    {
        Player.MyInstance.CastSkill(MyName);
    }
}