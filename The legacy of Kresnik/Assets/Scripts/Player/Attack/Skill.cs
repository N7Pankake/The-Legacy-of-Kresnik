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
    private int heal;
    public int MyHeal
    {
        get
        {
            return heal;
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

    //public SkillScript ss;
    
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
            Debug.Log(effect);
            //SkillScript.MyInstance.myEffect = effect;
            return string.Format("<color={0}>{1}</color>\nCast time: {2} second(s)\n{3}\nthat deals {4} damage", color, name, castTime, description, damage);
        }

        else if(effect == Effect.Heal)
        {
            Debug.Log(effect);
            //SkillScript.MyInstance.myEffect = effect;
            return string.Format("<color={0}>{1}</color>\nCast time: {2} second(s)\n{3}\nthat heals {4}hp", color, name, castTime, description, heal);
        }

        else if (effect == Effect.Regen)
        {
            Debug.Log(effect);
            //SkillScript.MyInstance.myEffect = effect;
            return string.Format("<color={0}>{1}</color>\nCast time: {2} second(s)\n{3}\nthat heals {4}hp per second", color, name, castTime, description, heal);
        }

        else if (effect == Effect.Dps)
        {
            Debug.Log(effect);
            //SkillScript.MyInstance.myEffect = effect;
            return string.Format("<color={0}>{1}</color>\nCast time: {2} second(s)\n{3}\nthat deals {4} for {5} seconds", color, name, castTime, description, damage, time);
        }

        else if (effect == Effect.Stun)
        {
            Debug.Log(effect);
            //SkillScript.MyInstance.myEffect = effect;
            return string.Format("<color={0}>{1}</color>\nCast time: {2} second(s)\n{3}\nthat stuns for {4} second(s)", color, name, castTime, description, damage);
        }

        return null;
    }


    public void Use()
    {
        Player.MyInstance.CastSkill(MyName);
    }
}