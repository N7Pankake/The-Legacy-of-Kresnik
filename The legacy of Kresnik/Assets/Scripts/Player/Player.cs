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

    //PlayerStats
    //Player Mana
    [SerializeField]
    private Stat mana;
    private float initMana = 50;

    public Stat MyMana
    {
        get { return mana; }
        set { mana = value; }
    }

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

    public int MyGold { get; set; }
    
    //Block skills if I can't see the Target
    [SerializeField]
    private Block[] blocks;

    //Exit Points for skills
    [SerializeField]
    private Transform[] exitPoints;
    private int exitIndex = 2;

    //A default basic attack that doesn't follow targets
    [SerializeField]
    private GameObject nonTargetSkill;

    //Game Limits
    private Vector3 min, max;

    // Prototype Direction for Non Target Skill see if it works
    public bool upD = false;
    public bool downD = true;
    public bool leftD = false;
    public bool rightD = false;

	
	// Update is called once per frame
	protected override void Update ()
    {
            GetInput();

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, min.x, max.x), Mathf.Clamp(transform.position.y, min.y, max.y), transform.position.z);

            base.Update();
    }

    public void SetDefaults()
    {
        MyGold = 1000;

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

            upD = true;
            downD = false;
            leftD = false;
            rightD = false;
        }

        if (Input.GetKey(KeybindManager.MyInstance.Keybinds["Left"]))
        {
            Direction += Vector2.left;
            exitIndex = 3;

            upD = false;
            downD = false;
            leftD = true;
            rightD = false;
        }

        if (Input.GetKey(KeybindManager.MyInstance.Keybinds["Down"]))
        {
            Direction += Vector2.down;
            exitIndex = 2;

            upD = false;
            downD = true;
            leftD = false;
            rightD = false;
        }

        if (Input.GetKey(KeybindManager.MyInstance.Keybinds["Right"]))
        {
            Direction += Vector2.right;
            exitIndex = 1;

            upD = false;
            downD = false;
            leftD = false;
            rightD = true;
        }

        if (Input.GetKeyDown(KeybindManager.MyInstance.Keybinds["Melee"]))
        {
            Attack();
        }

        if (Input.GetKeyDown(KeybindManager.MyInstance.Keybinds["Default"]))
        {
            NonTargetSkill();
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

        isUsingSkill = true;

        MyAnimator.SetBool("skill", isUsingSkill);

        yield return new WaitForSeconds(newSkill.MyCastTime);

        if (currentTarget != null && InLineOfSight())
        {
            SkillScript s = Instantiate(newSkill.MySkillPrefab, exitPoints[exitIndex].position, Quaternion.identity).GetComponent<SkillScript>();
            s.Initialize(currentTarget, newSkill.MyDamage, transform);
        }

        StopSkill();
    }

    public void CastSkill(string skillName)
    {
        Block();

        if (MyTarget != null && MyTarget.GetComponentInParent<Character>().IsAlive && !isUsingSkill && !IsMoving && InLineOfSight())
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
    }

    public void AddAttacker(Enemy enemy)
    {
        if (!MyAttackers.Contains(enemy))
        {
            MyAttackers.Add(enemy);
        }
    }

    private void NonTargetSkill()
    {
        Instantiate (nonTargetSkill, exitPoints[exitIndex].position, exitPoints[exitIndex].rotation);  
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

    public IEnumerator RegenHP(int regenTime, int health)
    {
        if (IsAlive)
        {
            for (int i = 0; i < regenTime; ++i)
            {
                Player.MyInstance.MyHealth.MyCurrentValue += health;
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
                yield return new WaitForSeconds(1);
            }
        }
    }

    public IEnumerator IncreaseSpeed(int speed, int time)
    {
        if (IsAlive)
        {
            Player.MyInstance.MySpeed += speed;
            yield return new WaitForSeconds(time);
            Player.MyInstance.MySpeed = 5;
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