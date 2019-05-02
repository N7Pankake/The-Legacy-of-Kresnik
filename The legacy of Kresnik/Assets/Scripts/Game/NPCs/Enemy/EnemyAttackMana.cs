using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackMana : MonoBehaviour
{
    [SerializeField]
    private int damage;

    [SerializeField]
    private int damageMana;

    private Transform source;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerHitbox")
        {
            Character c = collision.GetComponentInParent<Character>();

            Player.MyInstance.MyMana.MyCurrentValue -= damageMana;
            StartCoroutine(ManaDamage(damageMana));

            c.DamageTaken(damage , source);
            Enemy.MyInstance.AttackOff();
        }
    }

    private IEnumerator ManaDamage(int damageMana)
    {
        yield return new WaitForSeconds(0.2f);
        CombatTextManager.MyInstance.CreateText((Player.MyInstance.transform.position), damageMana.ToString(), SCTType.DamageMana, false);
    }
}
