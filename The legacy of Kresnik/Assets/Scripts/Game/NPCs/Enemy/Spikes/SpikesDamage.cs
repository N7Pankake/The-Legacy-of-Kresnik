using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesDamage : MonoBehaviour
{
    [SerializeField]
    private int damage;

    private Transform source;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "SpikesHitbox")
        {
            Character c = collision.GetComponentInParent<Character>();
            c.DamageTaken(damage, source);
        }
    }
}
