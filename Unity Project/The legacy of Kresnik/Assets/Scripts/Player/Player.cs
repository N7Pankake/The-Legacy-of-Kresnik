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
    
    protected override void Start()
    {
        health.Initialize(initHealth, initHealth);
        mana.Initialize(initMana, initMana);
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
        }

        if (Input.GetKey(KeyCode.A))
        {
            direction += Vector2.left;
        }

        if (Input.GetKey(KeyCode.S))
        {
            direction += Vector2.down;
        }

        if (Input.GetKey(KeyCode.D))
        {
            direction += Vector2.right;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            skillRoutine = StartCoroutine(Skills());
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
           isAttacking = true;
           attackTimer = attackCd;
           Attack();
        }
    }

    private IEnumerator Skills()
    {
        if (!isUsingSkill && !IsMoving)
        {
            isUsingSkill = true;
            myAnimator.SetBool("skill", isUsingSkill);
            yield return new WaitForSeconds(1);
            StopSkill();
        }
    }
}
