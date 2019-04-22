using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyStat : MonoBehaviour
{
    [SerializeField]
    private Text moneyValue;

    private int currentGold;

    public int MyCurrentGold
    {
        get
        {
            return currentGold;
        }

        set
        {
            moneyValue.text = currentGold + "";
        }
    }
}
