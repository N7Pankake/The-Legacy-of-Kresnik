using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] tutorials;

    [SerializeField]
    private CanvasGroup tutorialCanvas;

    [SerializeField]
    private Button[] tutorialbtns;

    private int tutorialOpen;

    public void OpenMovement()
    {
        tutorialCanvas.alpha = 1;
        tutorialCanvas.blocksRaycasts = true;
        tutorials[0].SetActive(true);
        tutorialOpen = 0;
    }

    public void OpenAttack()
    {
        tutorialCanvas.alpha = 1;
        tutorialCanvas.blocksRaycasts = true;
        tutorials[1].SetActive(true);
        tutorialOpen = 1;
    }
    public void OpenTargeting()
    {
        tutorialCanvas.alpha = 1;
        tutorialCanvas.blocksRaycasts = true;
        tutorials[2].SetActive(true);
        tutorialOpen = 2;
    }
    public void OpenStats()
    {
        tutorialCanvas.alpha = 1;
        tutorialCanvas.blocksRaycasts = true;
        tutorials[3].SetActive(true);
        tutorialOpen = 3;
    }
    public void OpenPotions()
    {
        tutorialCanvas.alpha = 1;
        tutorialCanvas.blocksRaycasts = true;
        tutorials[4].SetActive(true);
        tutorialOpen = 4;
    }
    public void OpenEnemies()
    {
        tutorialCanvas.alpha = 1;
        tutorialCanvas.blocksRaycasts = true;
        tutorials[5].SetActive(true);
        tutorialOpen = 5;
    }

    public void GoBack()
    {
        tutorialCanvas.alpha = 0;
        tutorialCanvas.blocksRaycasts = false;
        tutorials[tutorialOpen].SetActive(false);
    }

    public void Next()
    {
        if (tutorialOpen == 5)
        {
            tutorials[tutorialOpen].SetActive(false);

            tutorialOpen = 0;

            tutorials[tutorialOpen].SetActive(true);
        }
        else
        {
            tutorials[tutorialOpen].SetActive(false);

            ++tutorialOpen;

            tutorials[tutorialOpen].SetActive(true);
        }
    }

    public void Previous()
    {
        if (tutorialOpen == 0)
        {
            tutorials[tutorialOpen].SetActive(false);

            tutorialOpen = 5;

            tutorials[tutorialOpen].SetActive(true);
        }
        else
        {
            tutorials[tutorialOpen].SetActive(false);

            --tutorialOpen;

            tutorials[tutorialOpen].SetActive(true);
        }
    }

    public void GoMenu()
    {
        SceneManager.LoadScene(0);
    }
}
