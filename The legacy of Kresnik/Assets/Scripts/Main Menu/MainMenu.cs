using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public GameObject mainMenu;

    [SerializeField]
    private CanvasGroup loadCanvas;

    public void Tutorial()
    {
        SceneManager.LoadScene(1);
    }

    public void NewGame()
    {
        SceneManager.LoadScene(2);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void LoadGame()
    {
        loadCanvas.alpha = 1;
        loadCanvas.blocksRaycasts = true;
    }

    public void CloseLoadGame()
    {
        loadCanvas.alpha = 0;
        loadCanvas.blocksRaycasts = false;
    }

}
