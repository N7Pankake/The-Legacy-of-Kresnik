using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private IInteractable interactable;

    //Player Mana
    [SerializeField]
    private Stat mana;
    private float initMana = 50;

    public Stat MyMana
    {
        get { return mana; }
    }

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

    protected override void Start()
    {
        mana.Initialize(initMana, initMana);

        base.Start();
    }
	
	// Update is called once per frame
	protected override void Update ()
    {
            GetInput();

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, min.x, max.x), Mathf.Clamp(transform.position.y, min.y, max.y), transform.position.z);

            base.Update();
    }

    private void GetInput()
    {
        Direction = Vector2.zero;
         
        if (Input.GetKey(KeybindManager.MyInstance.Keybinds["Up"]))
        {
            Direction += Vector2.up;
            exitIndex = 0;
        }

        if (Input.GetKey(KeybindManager.MyInstance.Keybinds["Left"]))
        {
            Direction += Vector2.left;
            exitIndex = 3;
        }

        if (Input.GetKey(KeybindManager.MyInstance.Keybinds["Down"]))
        {
            Direction += Vector2.down;
            exitIndex = 2;
        }

        if (Input.GetKey(KeybindManager.MyInstance.Keybinds["Right"]))
        {
            Direction += Vector2.right;
            exitIndex = 1;
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

    public IEnumerator Regen(int regenTime, int healAmount)
    {
        if (IsAlive)
        {
            for (int i = 0; i < regenTime; ++i)
            {
                Player.MyInstance.MyHealth.MyCurrentValue += healAmount;
                yield return new WaitForSeconds(1);
            }
        }
    }

    public void Interact()
    {
        if (interactable != null)
        {
            interactable.Interact();
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" || collision.tag == "Interactable")
        {
            interactable = collision.GetComponent<IInteractable>();
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" || collision.tag == "Interactable")
        {
            if (interactable != null)
            {
                interactable.StopInteract();
                interactable = null;
            }
        }
    }
}