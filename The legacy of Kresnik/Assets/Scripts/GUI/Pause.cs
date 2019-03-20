using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour {

    public GameObject PauseUI;
    private Scene scene;

    private bool paused = false;

    void Start()
    {
        scene = SceneManager.GetActiveScene();
        PauseUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            paused = !paused;
        }

        if (paused)
        {
            PauseUI.SetActive(true);
            Time.timeScale = 0;
        }

        if (!paused)
        {
            PauseUI.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void Resume()
    {
        paused = false;
    }

    public void Options()
    {
        SceneManager.LoadScene(1);
    }

    public void Menu()
     {
        SceneManager.LoadScene(0);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
