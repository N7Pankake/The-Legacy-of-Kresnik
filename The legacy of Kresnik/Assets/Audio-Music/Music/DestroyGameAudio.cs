using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyGameAudio : MonoBehaviour
{
    void Awake()
    {
        GameObject A = GameObject.FindGameObjectWithTag("MusicGame");
        Destroy(A);
    }
}
