using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyMenu : MonoBehaviour
{
    void Awake()
    {
        GameObject[] objs1 = GameObject.FindGameObjectsWithTag("Music");
        GameObject[] objs2 = GameObject.FindGameObjectsWithTag("MusicGame");
        GameObject[] objs = objs1.Concat(objs2).ToArray();
        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(transform.gameObject);
    }
}
