using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class  Skill : IUseable, IMoveable
{
    [SerializeField]
    private string name;

    [SerializeField]
    private int damage;

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

    public void Use()
    {
        Player.MyInstance.CastSkill(MyName);
    }
}