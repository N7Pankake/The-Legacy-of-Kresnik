using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField]
    private int damage;

    private Transform source;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("I'm Damaging OwO "+ damage);

        if (collision.tag == "Player")
        {
            Character c = collision.GetComponentInParent<Character>();
            c.DamageTaken(damage , source);
            Enemy.MyInstance.AttackOff();
        }
    }
}
