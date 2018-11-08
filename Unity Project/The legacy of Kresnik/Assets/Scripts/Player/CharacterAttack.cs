using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttack : Character {

    private float attackTimer = 0;
    protected float attackCd; //Not using it yet

    public void Update()
    {
        if (isAttacking)
        {
            if (attackTimer > 0)
            {
                attackTimer -= Time.deltaTime;
            }

            else
            {
                isAttacking = false;
            }
        }

        myAnimator.SetBool("attack", isAttacking);
    }
}
