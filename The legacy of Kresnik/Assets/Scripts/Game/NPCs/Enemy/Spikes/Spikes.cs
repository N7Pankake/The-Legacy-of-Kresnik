using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField]
    private GameObject spikesCollision;

    private Animator MyAnimator { get; set; }

    private IEnumerator mySpikesOn;

    [SerializeField]
    private float spikesCD;

    private bool spikesOn = true;

    public void Start()
    {
        MyAnimator = GetComponent<Animator>();
        mySpikesOn = SpikesOn(spikesCD, spikesOn);
        StartCoroutine(mySpikesOn);
    }

    private IEnumerator SpikesOn(float cd, bool spikesOn)
    {
        while (spikesOn)
        {
            yield return new WaitForSeconds(cd);

            MyAnimator.SetBool("SpikesOn", true);
            spikesCollision.SetActive(true);

            yield return new WaitForSeconds(cd);

            MyAnimator.SetBool("SpikesOn", false);
            spikesCollision.SetActive(false);
        }
    }

}

