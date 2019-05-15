using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Range : MonoBehaviour
{
    private Enemy parent;

    private void Start()
    {
        parent = GetComponentInParent<Enemy>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && parent.IsAlive)
        {
            if (parent.gameObject.tag == "Boss")
            {
                BossVoice.MyInstance.BossInRange();
            }

            if (parent.gameObject.tag == "Enemy")
            {
                EnemyVoice.MyInstance.EnemyInRange();
            }
            parent.SetTarget(collision.transform);
        }
    }
}
