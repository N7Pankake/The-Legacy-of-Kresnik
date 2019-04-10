﻿using System;
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
    private CanvasGroup keybindMenu;

    [SerializeField]
    private CanvasGroup skillBook;

    [SerializeField]
    private CharacterPanel charPanel;

    [SerializeField]
    private GameObject tooltip;

    private Text tooltipText;

    [SerializeField]
    private RectTransform tooltipRect;

    [SerializeField]
    private GameObject targetFrame;

   [SerializeField]
    private Image face;

    [SerializeField]
    private ActionButton[] actionButtons;
    
    private Stat healthStat;

    private GameObject[] keybindButtons;

    private void Awake()
    {
        keybindButtons = GameObject.FindGameObjectsWithTag("Keybind");
        tooltipText = tooltip.GetComponentInChildren<Text>();
    }

    // Start is called before the first frame update
    void Start()
    {
        healthStat = targetFrame.GetComponentInChildren<Stat>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            OpenClose(keybindMenu);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            OpenClose(skillBook);
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            InventoryScript.MyInstance.OpenClose();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            charPanel.OpenClose();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Player.MyInstance.MyMana.MyCurrentValue -= 5;
            Player.MyInstance.MyHealth.MyCurrentValue -= 10;
        }
    }

    public void ShowTargetFrame(Enemy target)
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

    public void UpdateKeyText(string key, KeyCode code)
    {
        Text tmp = Array.Find(keybindButtons, x => x.name == key).GetComponentInChildren<Text>();
        tmp.text = code.ToString();
    }

    public void ClickActionButton(string buttonName)
    {
        Array.Find(actionButtons, x => x.gameObject.name == buttonName).MyButton.onClick.Invoke();
    }

    public void OpenClose (CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
        canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true;
    }

    public void UpdateStackSize(IClickable clickable)
    {
        if(clickable.MyCount > 1)
        {
            clickable.MyStackText.text = clickable.MyCount.ToString();
            clickable.MyStackText.color = Color.white;
            clickable.MyIcon.color = Color.white;
        }

        else
        {
            clickable.MyStackText.color = new Color(0,0,0,0);
            clickable.MyIcon.color = Color.white;
        }

        if(clickable.MyCount == 0)
        {
            clickable.MyIcon.color = new Color(0, 0, 0, 0);
            clickable.MyStackText.color = new Color(0, 0, 0, 0);
        }
    }

    public void ShowTooltip(Vector2 pivot,Vector3 position, IDescribable description)
    {
        tooltipRect.pivot = pivot;
        tooltip.SetActive(true);
        tooltip.transform.position = position;
        tooltipText.text = description.GetDescription();
    }

    public void HideToolTip()
    {
        tooltip.SetActive(false);
    }

    public void RefreshTooltip(IDescribable description)
    {
        tooltipText.text = description.GetDescription();
    }
}
