using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossVoice : MonoBehaviour
{
    private static BossVoice instance;

    public static BossVoice MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<BossVoice>();
            }
            return instance;
        }
    }

    [SerializeField]
    private AudioClip[] voiceClips;

    // BOSS AUDIOS

    public void BossInRange()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.PlayOneShot(voiceClips[1]);
    }

    public void BossAttack()
    {
        int randomChance = Random.Range(0, 99);
        int randomClip = Random.Range(3, 5);

        if (randomChance <= 50)
        {
            AudioSource audio = GetComponent<AudioSource>();
            audio.PlayOneShot(voiceClips[randomClip]);
        }
    }

    public void BossAttackedVoice()
    {
        int randomChance = Random.Range(0, 99);
        int randomClip = Random.Range(1, 3);

        if (randomChance <= 50)
        {
            AudioSource audio = GetComponent<AudioSource>();
            audio.PlayOneShot(voiceClips[randomClip]);
        }
    }

    public void BossDeath()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.PlayOneShot(voiceClips[6]);
    }
}
