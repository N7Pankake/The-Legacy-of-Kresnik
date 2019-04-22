using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class AttackTrigger: MonoBehaviour
{

    [SerializeField]
    private GameObject first;
    private Transform source;

    private Coroutine attackCoroutineCD;

    public void Deactivate()
    {
        first.SetActive(false);
    }

    public void Activate()
    {
        first.SetActive(true);
        attackCoroutineCD = StartCoroutine(Wait(0.001f));
    }

    public IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
        Deactivate();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
            {
            Debug.Log("I'm Colliding OwO");
            Character c = collision.GetComponentInParent<Character>();
            c.DamageTaken(Player.MyInstance.MyAttackDamage, source);
            Deactivate();
            }
    }
}
