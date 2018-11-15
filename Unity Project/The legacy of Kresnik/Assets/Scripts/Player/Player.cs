using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Character {

    //Player Health
    [SerializeField]
    private Stat health;
    private float initHealth = 100;

    //Player Mana
    [SerializeField]
    private Stat mana;
    private float initMana = 50;

    //Player Skills
    [SerializeField]
    private GameObject[] skillPrefab;

    [SerializeField]
    private Block[] blocks;

    [SerializeField]
    private Transform[] exitPoints;

    private int exitIndex = 2;

    private Transform target;

    
    protected override void Start()
    {
        health.Initialize(initHealth, initHealth);
        mana.Initialize(initMana, initMana);

        target = GameObject.Find("Decoy").transform;

        base.Start();
    }
	
	// Update is called once per frame
	protected override void Update ()
    {
            GetInput();
            base.Update();
    }

    private void GetInput()
    {
        direction = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
        {
            direction += Vector2.up;
            exitIndex = 0;
        }

        if (Input.GetKey(KeyCode.A))
        {
            direction += Vector2.left;
            exitIndex = 3;
        }

        if (Input.GetKey(KeyCode.S))
        {
            direction += Vector2.down;
            exitIndex = 2;
        }

        if (Input.GetKey(KeyCode.D))
        {
            direction += Vector2.right;
            exitIndex = 1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Block();

            if (!isUsingSkill && !IsMoving && InLineOfSight())
            {
                skillRoutine = StartCoroutine(Skills());
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Attack();
        }
    }

    private IEnumerator Skills()
    {
        isUsingSkill = true;

        myAnimator.SetBool("skill", isUsingSkill);

        yield return new WaitForSeconds(0.4f);

        CastSkill();

        StopSkill();
    }

    public void CastSkill()
    {
        Instantiate (skillPrefab[0], exitPoints[exitIndex].position, Quaternion.identity);        
    }

    private bool InLineOfSight()
    {
        Vector3 targetDirection = (target.transform.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection, Vector2.Distance(transform.position, target.transform.position), 256);
           
        if (hit.collider == null)
        {
            return true;
        }

        return false;
    }

    private void Block()
    {
        foreach(Block b in blocks)
        {
            b.Deactivate();
        }

        blocks[exitIndex].Activate();
    }

    private void Attack()
    {
        isAttacking = true;
        myAnimator.SetTrigger("attack");
    }
}
