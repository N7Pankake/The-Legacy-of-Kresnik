using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Character {

    //Player Mana
    [SerializeField]
    private Stat mana;
    private float initMana = 50;

    //Block skills if I can't see the Target
    [SerializeField]
    private Block[] blocks;

    //Exit Points for skills
    [SerializeField]
    private Transform[] exitPoints;
    private int exitIndex = 2;

    private SkillBook skillBook;

    //A default basic attack that doesn't follow targets
    [SerializeField]
    private GameObject nonTargetSkill;

    //Game Limits
    private Vector3 min, max;

    
    protected override void Start()
    {
        skillBook = GetComponent<SkillBook>();
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
         
        if (Input.GetKey(KeyCode.W))
        {
            Direction += Vector2.up;
            exitIndex = 0;
        }

        if (Input.GetKey(KeyCode.A))
        {
            Direction += Vector2.left;
            exitIndex = 3;
        }

        if (Input.GetKey(KeyCode.S))
        {
            Direction += Vector2.down;
            exitIndex = 2;
        }

        if (Input.GetKey(KeyCode.D))
        {
            Direction += Vector2.right;
            exitIndex = 1;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            NonTargetSkill();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Attack();
        }

        if(IsMoving)
        {
            StopSkill();
        }
    }

    private IEnumerator Skills(int skillIndex)
    {
        Transform currentTarget = MyTarget;

        Skill newSkill = skillBook.CastSkill(skillIndex);

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

    public void CastSkill(int skillIndex)
    {
        Block();

        if (MyTarget != null && MyTarget.GetComponentInParent<Character>().IsAlive && !isUsingSkill && !IsMoving && InLineOfSight())
        {
            skillRoutine = StartCoroutine(Skills(skillIndex));
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
        skillBook.StopCasting();

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
}