using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character
{
    private static Player instance;

    public static Player MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Player>();
            }
            return instance;
        }
    }

    private List<IInteractable> interactables = new List<IInteractable>();

    public List<IInteractable> MyInteractables
    {
        get
        {
            return interactables;
        }

        set
        {
            interactables = value;
        }
    }

    private List<Enemy> attackers = new List<Enemy>();
    public List<Enemy> MyAttackers
    {
        get
        {
            return attackers;
        }

        set
        {
            attackers = value;
        }
    }

    //Player Mana
    [SerializeField]
    private float initMana = 50;

    [SerializeField]
    private Stat mana;
    
    public Stat MyMana
    {
        get { return mana; }
        set { mana = value; }
    }

    // In the future you will be able to add some points to you character for now just the gear can add points
    //[SerializeField]
    //private int strength;
    //public int MyStrength
    //{
    //    get
    //    {
    //        return strength;
    //    }

    //    set
    //    {
    //        strength = value;
    //    }
    //}

    //[SerializeField]
    //private int intelligence;
    //public int MyIntelligence
    //{
    //    get
    //    {
    //        return intelligence;
    //    }

    //    set
    //    {
    //        intelligence = value;
    //    }
    //}

    //[SerializeField]
    //private int vitality;
    //public int MyVitality
    //{
    //    get
    //    {
    //        return vitality;
    //    }

    //    set
    //    {
    //        vitality = value;
    //    }
    //}

    //Is it buffed (? (Only works for speed potion cause is the only buff atm.)
    private bool buffed = false;
    public bool ImBuffed
    {
        get
        {
            return buffed;
        }

        set
        {
            buffed = value;
        }
    }

    //Player exp
    [SerializeField]
    private Stat xpStat;
    public Stat MyXP
    {
        get
        {
            return xpStat;
        }

        set
        {
            xpStat = value;
        }
    }

    [SerializeField]
    private Text levelText;
    
    //Block skills if I can't see the Target
    [SerializeField]
    private Block[] blocks;

    [SerializeField]
    private AttackTrigger[] attackTriger;
    private int exitSword = 1;

    [SerializeField]
    private int attackDamage;
    public int MyAttackDamage
    {
        get
        {
            return attackDamage;
        }
        set
        {
            attackDamage = value;
        }
    }

    //Exit Points for skills
    [SerializeField]
    private Transform[] exitPoints;
    private int exitIndex = 2;

    //Game Limits
    private Vector3 min, max;

    private int skillManaCost;
    
    [SerializeField]
    private Transform minimapIndicator;
    
	// Update is called once per frame
	protected override void Update ()
    {
        GetInput();
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, min.x, max.x), Mathf.Clamp(transform.position.y, min.y, max.y), transform.position.z);
        base.Update();
    }

    public void SetDefaults()
    {
        UIManager.MyInstance.MyGold = 100;
        
        health.Initialize(initHealth, initHealth);
        mana.Initialize(initMana, initMana);
        
        MyXP.Initialize(0, Mathf.Floor(100 * MyLevel * Mathf.Pow(MyLevel, 0.5f)));
        levelText.text = MyLevel.ToString();
    }

    private void GetInput()
    {
        Direction = Vector2.zero;
         
        if (Input.GetKey(KeybindManager.MyInstance.Keybinds["Up"]))
        {
            Direction += Vector2.up;
            exitIndex = 0;
            exitSword = 0;
            minimapIndicator.eulerAngles = new Vector3(0, 0, -45);
        }

        if (Input.GetKey(KeybindManager.MyInstance.Keybinds["Left"]))
        {
            Direction += Vector2.left;
            exitIndex = 3;
            exitSword = 3;
            if (Direction.y == 0)
            {
                minimapIndicator.eulerAngles = new Vector3(0, 0, 45);
            }

        }

        if (Input.GetKey(KeybindManager.MyInstance.Keybinds["Down"]))
        {
            Direction += Vector2.down;
            exitIndex = 2;
            exitSword = 2;
            minimapIndicator.eulerAngles = new Vector3(0, 0, 135);
        }

        if (Input.GetKey(KeybindManager.MyInstance.Keybinds["Right"]))
        {
            Direction += Vector2.right;
            exitIndex = 1;
            exitSword = 1;
            if (Direction.y == 0)
            {
                minimapIndicator.eulerAngles = new Vector3(0, 0, -135);
            }
        }

        if (Input.GetKeyDown(KeybindManager.MyInstance.Keybinds["Melee"]) && !IsMoving)
        {
            Attack();
        }
        
        if(IsMoving)
        {
            StopSkill();
        }

        foreach (string action in KeybindManager.MyInstance.ActionBinds.Keys)
        {
            if (Input.GetKeyDown(KeybindManager.MyInstance.ActionBinds[action]))
            {
                UIManager.MyInstance.ClickActionButton(action);
            }
        }

    }

    private IEnumerator Skills(string skillName)
    {
        Transform currentTarget = MyTarget;

        Skill newSkill = SkillBook.MyInstance.CastSkill(skillName);

        int manaCost = SkillBook.MyInstance.MyManaCost;

        float speed = SkillBook.MyInstance.MySkillSpeed;

        isUsingSkill = true;

        MyAnimator.SetBool("skill", isUsingSkill);

        yield return new WaitForSeconds(newSkill.MyCastTime);

        if (currentTarget != null && InLineOfSight())
        {
            Player.MyInstance.MyMana.MyCurrentValue -= manaCost;
            SkillScript s = Instantiate(newSkill.MySkillPrefab, exitPoints[exitIndex].position, Quaternion.identity).GetComponent<SkillScript>();
            s.Initialize(currentTarget, newSkill.MyDamage, manaCost, speed, transform);
        }

        StopSkill();
    }
    
    public void CastSkill(string skillName)
    {
        Block();

            if (MyTarget != null && MyTarget.GetComponentInParent<Character>().IsAlive && !isUsingSkill && !IsMoving && InLineOfSight() && Player.MyInstance.MyMana.MyCurrentValue >= SkillBook.MyInstance.MyManaCost)
            {
            skillRoutine = StartCoroutine(Skills(skillName));
            }  
    }

    private bool InLineOfSight()
    {
        if (MyTarget != null)
        {
            Vector3 targetDirection = (MyTarget.transform.position - transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection, Vector2.Distance(transform.position, MyTarget.transform.position), 256);

            if (hit.collider == null)
            {
                return true;
            }
        }

        return false;
    }

    private void Attack()
    {
        IsAttacking = true;
        MyAnimator.SetTrigger("attack");

        foreach (AttackTrigger at in attackTriger)
        {
            at.Deactivate();
        }
        attackTriger[exitSword].Activate();
    }

    private void AttackOff()
    {
        IsAttacking = false;
    }

    public void AddAttacker(Enemy enemy)
    {
        if (!MyAttackers.Contains(enemy))
        {
            MyAttackers.Add(enemy);
        }
    }

    private void Block()
    {
        foreach (Block b in blocks)
        {
            b.Deactivate();
        }

        blocks[exitIndex].Activate();
    }

    public void StopSkill()
    {
        SkillBook.MyInstance.StopCasting();

        isUsingSkill = false;

        MyAnimator.SetBool("skill", isUsingSkill);

        if (skillRoutine != null)
        {
            StopCoroutine(skillRoutine);
        }
    }

    public void SetLimits(Vector3 min, Vector3 max)
    {
        this.min = min;
        this.max = max;
    }

    public void GetMana(int mana)
    {
        MyMana.MyCurrentValue += mana;
        CombatTextManager.MyInstance.CreateText(transform.position, mana.ToString(), SCTType.Mana, true);
    }

    public IEnumerator RegenHP(int regenTime, int health)
    {
        if (IsAlive)
        {
            for (int i = 0; i < regenTime; ++i)
            {
                Player.MyInstance.MyHealth.MyCurrentValue += health;
                CombatTextManager.MyInstance.CreateText(transform.position, health.ToString(), SCTType.Heal, true);
                yield return new WaitForSeconds(1);
            }
        }
    }

    public IEnumerator RegenMP(int regenTime, int mana)
    {
        if (IsAlive)
        {
            for (int i = 0; i < regenTime; ++i)
            {
                CombatTextManager.MyInstance.CreateText(transform.position, mana.ToString(), SCTType.Mana, true);
                Player.MyInstance.MyMana.MyCurrentValue += mana;
                yield return new WaitForSeconds(1);
            }
        }
    }

    public IEnumerator RegenAll(int regenTime, int mana, int health)
    {
        if (IsAlive)
        {
            for (int i = 0; i < regenTime; ++i)
            {
                Player.MyInstance.MyHealth.MyCurrentValue += health;
                Player.MyInstance.MyMana.MyCurrentValue += mana;
                CombatTextManager.MyInstance.CreateText(transform.position, health.ToString(), SCTType.Heal, true);
                yield return new WaitForSeconds(0.2f);
                CombatTextManager.MyInstance.CreateText(transform.position, mana.ToString(), SCTType.Mana, true);
                yield return new WaitForSeconds(1);
            }
        }
    }

    public IEnumerator IncreaseSpeed(int speed, int time)
    {
        if (IsAlive)
        {
            Player.MyInstance.MySpeed += speed;
            CombatTextManager.MyInstance.CreateText(transform.position, speed.ToString(), SCTType.Buff, false);
            yield return new WaitForSeconds(time);
            Player.MyInstance.MySpeed = 5;
            ImBuffed = false;
        }
    }

    public void GainXp(int xp)
    {
        MyXP.MyCurrentValue += xp;
        CombatTextManager.MyInstance.CreateText(transform.position, xp.ToString(), SCTType.XP, false);

        if(MyXP.MyCurrentValue >= MyXP.MyMaxValue)
        {
            StartCoroutine(Ding());
        }
    }

    public IEnumerator GainGold(int gold)
    {
        UIManager.MyInstance.MyGold += gold;
        yield return new WaitForSeconds(0.2f);
        CombatTextManager.MyInstance.CreateText(transform.position, gold.ToString(), SCTType.Gold, false);
    }

    private IEnumerator Ding()
    {
        while (!MyXP.IsFull)
        {
            yield return null;
        }

        MyLevel++;
        levelText.text = MyLevel.ToString();
        MyXP.MyMaxValue = 100 * MyLevel * Mathf.Pow(MyLevel, 0.5f);
        MyXP.MyMaxValue = Mathf.Floor(MyXP.MyMaxValue);
        MyXP.MyCurrentValue = MyXP.MyOverflow;
        MyXP.Reset();

        if (MyXP.MyCurrentValue >= MyXP.MyMaxValue)
        {
            StartCoroutine(Ding());
        }
    }

    public void UpdateLevel()
    {
        levelText.text = MyLevel.ToString();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" || collision.tag == "Interactable")
        {
            IInteractable interactable = collision.GetComponent<IInteractable>();

            if(!MyInteractables.Contains(interactable))
            {
                MyInteractables.Add(interactable);
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" || collision.tag == "Interactable")
        {
            if(MyInteractables.Count > 0)
            {
                IInteractable interactable = MyInteractables.Find(x => x == collision.GetComponent<IInteractable>());

                if(interactable != null)
                {
                    interactable.StopInteract();
                }

                MyInteractables.Remove(interactable);
            }
        }
    }
}