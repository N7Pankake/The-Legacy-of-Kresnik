using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SavedGame : MonoBehaviour
{
    [SerializeField]
    private Text dateTime;

    [SerializeField]
    private Image health;

    [SerializeField]
    private Image mana;

    [SerializeField]
    private Text levelText;

    [SerializeField]
    private GameObject visuals;

    [SerializeField]
    private int index;

    public int MyIndex
    {
        get
        {
            return index;
        }
    }

    private void Awake()
    {
        visuals.SetActive(false);
    }

    public void ShowInfo(SaveData saveData)
    {
        visuals.SetActive(true);
        dateTime.text = "Date: " + saveData.MyDateTime.ToString("dd/MM/yyy") + " - Time: " + saveData.MyDateTime.ToString("H: mm");

        health.fillAmount = saveData.MyPlayerData.MyHealth / saveData.MyPlayerData.MyMaxHealth;
        mana.fillAmount = saveData.MyPlayerData.MyMana / saveData.MyPlayerData.MyMaxMana;
        levelText.text = saveData.MyPlayerData.MyLevel.ToString();
    }

    public void HideVisuals()
    {
        visuals.SetActive(false);
    }
}
