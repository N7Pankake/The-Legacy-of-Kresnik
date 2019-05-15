using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVoice : MonoBehaviour
{
    private static EnemyVoice instance;

    public static EnemyVoice MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<EnemyVoice>();
            }
            return instance;
        }
    }

    [SerializeField]
    private AudioClip[] voiceClips;
    

    // NORMAL ENEMIES AUDIOS

    public void EnemyInRange()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.PlayOneShot(voiceClips[0]);
    }

    public void EnemyAttack()
    {
        int randomChance = Random.Range(0, 99);
        int randomClip = Random.Range(2, 4);

        if (randomChance <= 50)
        {
            AudioSource audio = GetComponent<AudioSource>();
            audio.PlayOneShot(voiceClips[randomClip]);
        }
    }

    public void EnemyAttacked()
    {
        int randomChance = Random.Range(0, 99);
        int randomClip = Random.Range(4, 6);

        if (randomChance <= 50)
        {
            AudioSource audio = GetComponent<AudioSource>();
            audio.PlayOneShot(voiceClips[randomClip]);
        }
    }

    public void EnemyDeath()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.PlayOneShot(voiceClips[1]);
    }
}
