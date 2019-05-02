using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    private int strength;
    public int MyStrength
    {
        get
        {
            return strength;
        }

        set
        {
            strength = value;
        }
    }

    private int intellect;
    public int MyIntellect
    {
        get
        {
            return intellect;
        }

        set
        {
            intellect = value;
        }
    }

    private int vitality;
    public int MyVitality
    {
        get
        {
            return vitality;
        }

        set
        {
            vitality = value;
        }
    }

    private float myOldManaValues;
    public float MyOldManaValues { get => myOldManaValues; set => myOldManaValues = value; }

    private float myOldHealthValues;
    public float MyOldHealthValues { get => myOldHealthValues; set => myOldHealthValues = value; }

    private float myNewManaValues;
    public float MyNewManaValues { get => myNewManaValues; set => myNewManaValues = value; }

    private float myNewHealthValues;
    public float MyNewHealthValues { get => myNewHealthValues; set => myNewHealthValues = value; }

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
    private TextMeshProUGUI levelText;
    
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
    
    [SerializeField]
    private int arcanaDamage;
    public int MyArcanaDamage { get => arcanaDamage; set => arcanaDamage = value; }

    private float playerCD = 0.4f;
    private float nextAttack;
    
    //Exit Points for skills
    [SerializeField]
    private Transform[] exitPoints;
    private int exitIndex = 2;

    //Game Limits
    private Vector3 min, max;

    private int skillManaCost;
    
    [SerializeField]
    private Transform minimapIndicator;

    [SerializeField]
    private AudioClip[] swordSounds;

    private SkillScript ss;

    [SerializeField]
    private MonoBehaviour[] skills;

    private int defaultCost;
    public int MyDefaultCost { get => defaultCost; set => defaultCost = value; }

    private int copyManaCost;
    public int CopyManaCost { get => copyManaCost; set => copyManaCost = value; }

    // Update is called once per frame
    protected override void Update ()
    {
        GetInput();
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, min.x, max.x), Mathf.Clamp(transform.position.y, min.y, max.y), transform.position.z);

        if (!Player.MyInstance.IsAlive)
        {
            StartCoroutine(Die());
        }
        base.Update();
    }

    public void SetDefaults()
    {
        UIManager.MyInstance.MyGold = 0;
        
        health.Initialize(initHealth, initHealth);
        mana.Initialize(initMana, initMana);
        
        MyXP.Initialize(0, Mathf.Floor(100 * MyLevel * Mathf.Pow(MyLevel, 0.5f)));
        levelText.text = MyLevel.ToString();
    }

    private void GetInput()
    {
        Direction = Vector2.zero;
         
        if (Input.GetKey(KeybindManager.MyInstance.Keybinds["Up"]) && IsAlive && !IsAttacking)
        {
            Direction += Vector2.up;
            exitIndex = 0;
            exitSword = 0;
            minimapIndicator.eulerAngles = new Vector3(0, 0, -45);
        }

        if (Input.GetKey(KeybindManager.MyInstance.Keybinds["Left"]) && IsAlive && !IsAttacking)
        {
            Direction += Vector2.left;
            exitIndex = 3;
            exitSword = 3;
            if (Direction.y == 0)
            {
                minimapIndicator.eulerAngles = new Vector3(0, 0, 45);
            }
        }

        if (Input.GetKey(KeybindManager.MyInstance.Keybinds["Down"]) && IsAlive && !IsAttacking)
        {
            Direction += Vector2.down;
            exitIndex = 2;
            exitSword = 2;
            minimapIndicator.eulerAngles = new Vector3(0, 0, 135);
        }

        if (Input.GetKey(KeybindManager.MyInstance.Keybinds["Right"]) && IsAlive && !IsAttacking)
        {
            Direction += Vector2.right;
            exitIndex = 1;
            exitSword = 1;
            if (Direction.y == 0)
            {
                minimapIndicator.eulerAngles = new Vector3(0, 0, -135);
            }
        }

        if (Input.GetKeyDown(KeybindManager.MyInstance.Keybinds["Melee"]) && !IsMoving && Time.time > nextAttack && IsAlive && !IsAttacking && !isUsingSkill)
        {
            nextAttack = Time.time + playerCD;
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

    private void Attack()
    {
        int randomClip = Random.Range(0, 10);

        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = swordSounds[randomClip];
        audio.Play();

        IsAttacking = true;
        MyAnimator.SetTrigger("attack");

        foreach (AttackTrigger at in attackTriger)
        {
            at.Deactivate();
        }
        attackTriger[exitSword].Activate();
    }

    public IEnumerator StopAttack()
    {
        yield return new WaitForSeconds(0.3f);
        IsAttacking = false;
    }

    public void CastSkill(string skillName, Effect effect, int manaCost)
    {
        Block();

        if (effect == Effect.Damage)
        {
            if (Player.MyInstance.MyArcanaDamage > 0)
            {
                manaCost = manaCost + (2 * (Player.MyInstance.MyArcanaDamage));
            }
        }

        if (effect == Effect.Dps)
        {
            if (Player.MyInstance.MyArcanaDamage > 0)
            {
                manaCost = manaCost + (6 * (Player.MyInstance.MyArcanaDamage / 3));
            }
        }

        if (effect == Effect.Stun)
        {
            if (Player.MyInstance.MyArcanaDamage > 0)
            {
                manaCost = manaCost + (18 * (Player.MyInstance.MyArcanaDamage / 9));
            }
        }

        if (effect == Effect.Heal)
        {
            if (Player.MyInstance.MyArcanaDamage > 0)
            {
                manaCost = manaCost + (2 * (Player.MyInstance.MyArcanaDamage / 2));
            }
        }

        if (effect == Effect.Regen)
        {
            if (Player.MyInstance.MyArcanaDamage > 0)
            {
                manaCost = manaCost + (6 * (Player.MyInstance.MyArcanaDamage / 3));
            }
        }


        if (effect == Effect.Heal && !isUsingSkill && !IsMoving && Player.MyInstance.MyMana.MyCurrentValue >= manaCost && !IsAttacking)
        {
            actionRoutine = StartCoroutine(Skills(skillName, effect, manaCost));
        }

        if (effect == Effect.Regen && !isUsingSkill && !IsMoving && Player.MyInstance.MyMana.MyCurrentValue >= manaCost && !IsAttacking)
        {
            actionRoutine = StartCoroutine(Skills(skillName, effect, manaCost));
        }

        if (MyTarget != null && MyTarget.GetComponentInParent<Character>().IsAlive && !isUsingSkill && !IsMoving && !IsAttacking && InLineOfSight() && Player.MyInstance.MyMana.MyCurrentValue >= manaCost)
        {
            actionRoutine = StartCoroutine(Skills(skillName, effect, manaCost));
        }
    }

    private IEnumerator Skills(string skillName, Effect effect, int manaCost)
    {
        Transform currentTarget = MyTarget;

        Skill newSkill = SkillBook.MyInstance.CastSkill(skillName);

        float speed = SkillBook.MyInstance.MySkillSpeed;

        isUsingSkill = true;

        MyAnimator.SetBool("skill", isUsingSkill);

        yield return new WaitForSeconds(newSkill.MyCastTime);

        if (effect == Effect.Heal)
        {
            Player.MyInstance.MyMana.MyCurrentValue -= manaCost;
            SkillScript s = Instantiate(newSkill.MySkillPrefab, exitPoints[exitIndex].position, Quaternion.identity).GetComponent<SkillScript>();
            s.Initialize(currentTarget, (newSkill.MyDamage + (arcanaDamage / 2)), manaCost, speed, transform);
        }

        if (effect == Effect.Regen)
        {
            Player.MyInstance.MyMana.MyCurrentValue -= manaCost;
            SkillScript s = Instantiate(newSkill.MySkillPrefab, exitPoints[exitIndex].position, Quaternion.identity).GetComponent<SkillScript>();
            s.Initialize(currentTarget, (newSkill.MyDamage + (arcanaDamage / 3)), manaCost, speed, transform);
        }

        if (currentTarget != null && InLineOfSight() && effect == Effect.Damage)
        {
            Player.MyInstance.MyMana.MyCurrentValue -= manaCost;
            SkillScript s = Instantiate(newSkill.MySkillPrefab, exitPoints[exitIndex].position, Quaternion.identity).GetComponent<SkillScript>();
            s.Initialize(currentTarget, (newSkill.MyDamage + arcanaDamage), manaCost, speed, transform);
        }

        if (currentTarget != null && InLineOfSight() && effect == Effect.Dps)
        {
            Player.MyInstance.MyMana.MyCurrentValue -= manaCost;
            SkillScript s = Instantiate(newSkill.MySkillPrefab, exitPoints[exitIndex].position, Quaternion.identity).GetComponent<SkillScript>();
            s.Initialize(currentTarget, (newSkill.MyDamage + (arcanaDamage / 3)), manaCost, speed, transform);
        }

        if (currentTarget != null && InLineOfSight() && effect == Effect.Stun)
        {
            Player.MyInstance.MyMana.MyCurrentValue -= manaCost;
            SkillScript s = Instantiate(newSkill.MySkillPrefab, exitPoints[exitIndex].position, Quaternion.identity).GetComponent<SkillScript>();
            s.Initialize(currentTarget, (newSkill.MyDamage + (arcanaDamage / 9)), manaCost, speed, transform);
        }


        StopSkill();
    }

    public void Loot(string actionName, List<Drop> items)
    {
        if (!isUsingSkill && !IsAttacking)
        {
            actionRoutine = StartCoroutine(LootRoutine(actionName, items));
        }
        
    }

    private IEnumerator LootRoutine(string actionName, List<Drop> items)
    {
        Transform currentTarget = MyTarget;

        Skill newSkill = SkillBook.MyInstance.CastSkill(actionName);

        isUsingSkill = true;

        yield return new WaitForSeconds(newSkill.MyCastTime);

        StopSkill();

        LootWindow.MyInstance.CreatePages(items);
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

        if (actionRoutine != null)
        {
            StopCoroutine(actionRoutine);
        }
    }

    public void SetLimits(Vector3 min, Vector3 max)
    {
        this.min = min;
        this.max = max;
    }

    public void DamageHealed(float damage)
    {
        if (IsAlive)
        {
            Player.MyInstance.MyHealth.MyCurrentValue += damage;
            CombatTextManager.MyInstance.CreateText(transform.position, damage.ToString(), SCTType.Heal, true);
        }
    }

    public void DamageHealedRegen(float damage)
    {
        if (IsAlive)
        {
            StartCoroutine(RegenHPSkill(damage));
        }
    }

    public void GetMana(int mana)
    {
        MyMana.MyCurrentValue += mana;
        CombatTextManager.MyInstance.CreateText(transform.position, mana.ToString(), SCTType.Mana, true);
    }

    public IEnumerator RegenHPSkill(float heal)
    {
        int time = 10;

        if (IsAlive)
        {
            for (int i = 0; i < time; ++i)
            {
                Player.MyInstance.MyHealth.MyCurrentValue += heal;
                CombatTextManager.MyInstance.CreateText(transform.position, heal.ToString(), SCTType.Heal, true);
                yield return new WaitForSeconds(1);
            }
        }
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
            PlayerStats.MyInstance.MyExtraSpeed = speed;
            CombatTextManager.MyInstance.CreateText(transform.position, speed.ToString(), SCTType.Buff, false);
            yield return new WaitForSeconds(time);
            Player.MyInstance.MySpeed = 4;
            PlayerStats.MyInstance.MyExtraSpeed = 0;
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

        MyVitality += 1;
        MyIntellect += 1;
        MyStrength += 1;

        MyMana.MyMaxValue += 5;
        MyHealth.MyMaxValue += 10;
        MyAttackDamage += (MyStrength / 3);
        MyArcanaDamage += (MyIntellect / 3);


        MyHealth.MyCurrentValue = MyHealth.MyMaxValue;
        MyMana.MyCurrentValue = MyMana.MyMaxValue;

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

    public IEnumerator Die()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(2);
    }
}