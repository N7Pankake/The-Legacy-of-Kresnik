using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    public static UIManager MyInstance
    {
        get
        {
            if(instance == null)
            { 
                instance = FindObjectOfType<UIManager>();
            }

            return instance;
        }
    }

    [SerializeField]
    private Button[] actionButtons;

    private KeyCode skill1, skill2, skill3, skill4, skill5, skill6, skill7, skill8, skill9, skill10;

    [SerializeField]
    private GameObject targetFrame;

    private Stat healthStat;

    [SerializeField]
    private Image face;

    // Start is called before the first frame update
    void Start()
    {

        healthStat = targetFrame.GetComponentInChildren<Stat>();

        //Keybinds
        skill1 = KeyCode.Alpha1;
        skill2 = KeyCode.Alpha2;
        skill3 = KeyCode.Alpha3;
        skill4 = KeyCode.Alpha4;
        skill5 = KeyCode.Alpha5;
        skill6 = KeyCode.Alpha6;
        skill7 = KeyCode.Alpha7;
        skill8 = KeyCode.Alpha8;
        skill9 = KeyCode.Alpha9;
        skill10 = KeyCode.Alpha0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(skill1))
        {
            ActionButtonOnClick(0);
        }

        if (Input.GetKeyDown(skill2))
        {
            ActionButtonOnClick(1);
        }

        if (Input.GetKeyDown(skill3))
        {
            ActionButtonOnClick(2);
        }

        if (Input.GetKeyDown(skill4))
        {
            ActionButtonOnClick(3);
        }

        if (Input.GetKeyDown(skill5))
        {
            ActionButtonOnClick(4);
        }

        if (Input.GetKeyDown(skill6))
        {
            ActionButtonOnClick(5);
        }

        if (Input.GetKeyDown(skill7))
        {
            ActionButtonOnClick(6);
        }

        if (Input.GetKeyDown(skill8))
        {
            ActionButtonOnClick(7);
        }

        if (Input.GetKeyDown(skill9))
        {
            ActionButtonOnClick(8);
        }

        if (Input.GetKeyDown(skill10))
        {
            ActionButtonOnClick(9);
        }
    }

    private void ActionButtonOnClick(int btIndex)
    {
        actionButtons[btIndex].onClick.Invoke();
    }

    public void ShowTargetFrame(NPC target)
    {
        targetFrame.SetActive(true);

        healthStat.Initialize(target.MyHealth.MyCurrentValue, target.MyHealth.MyMaxValue);

        face.sprite = target.MyPortrait;

        target.healthChanged += new HealthChanged(UpdateTargetFrame);

        target.characterRemoved += new CharacterRemoved(HideTargetFrame);
    }

    public void HideTargetFrame()
    {
        targetFrame.SetActive(false);
    }

    public void UpdateTargetFrame(float health)
    {
        healthStat.MyCurrentValue = health;
    }
}
