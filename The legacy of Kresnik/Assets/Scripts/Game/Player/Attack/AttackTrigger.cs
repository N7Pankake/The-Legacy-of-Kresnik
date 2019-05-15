using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class AttackTrigger: MonoBehaviour
{

    [SerializeField]
    private GameObject first;
    private Transform source;

    public void Deactivate()
    {
        first.SetActive(false);
        Player.MyInstance.StartCoroutine(Player.MyInstance.StopAttack());
    }

    public void Activate()
    {
        first.SetActive(true);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" || collision.tag == "Boss")
            {
            Character c = collision.GetComponentInParent<Character>();
            c.DamageTaken(Player.MyInstance.MyAttackDamage, source);
            Deactivate();
            }

        else
        {
            Deactivate();
        }
    }
}
