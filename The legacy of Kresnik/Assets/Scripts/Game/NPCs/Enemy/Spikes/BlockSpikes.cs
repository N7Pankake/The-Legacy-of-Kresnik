using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpikes : MonoBehaviour
{
    [SerializeField]
    private GameObject collider2D;

    [SerializeField]
    private GameObject[] enemies;

    [SerializeField]
    public Animator MyAnimator { get; set; }

    public void Start()
    {
        MyAnimator = GetComponent<Animator>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (enemies.Length > 0)
        {
            if (collision.tag == "SpikesHitbox")
            {
                MyAnimator.SetBool("SpikesOn", true);
                collider2D.SetActive(true);
            }
        }

        int nullCounter = 0;

        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] == null)
            {
                nullCounter++;
                if (nullCounter == enemies.Length)
                {
                    MyAnimator.SetBool("SpikesOn", false);
                    collider2D.SetActive(false);
                }
            }
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (enemies.Length > 0)
        {
            if (collision.tag == "SpikesHitbox")
            {
                MyAnimator.SetBool("SpikesOn", true);
                collider2D.SetActive(true);
            }
        }

        int nullCounter = 0;

        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] == null)
            {
                nullCounter++;
                if (nullCounter == enemies.Length)
                {
                    MyAnimator.SetBool("SpikesOn", false);
                    collider2D.SetActive(false);
                }
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        MyAnimator.SetBool("SpikesOn", false);
        collider2D.SetActive(false);
    }
}
