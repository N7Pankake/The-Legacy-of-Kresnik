using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    public static UIManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
            }
            return instance;
        }
    }
    
    [SerializeField]
    private CanvasGroup pauseMenu;

    [SerializeField]
    private CanvasGroup keybindMenu;

    [SerializeField]
    private CanvasGroup saveMenu;

    [SerializeField]
    private CanvasGroup skillBook;

    [SerializeField]
    private CanvasGroup questLog;

    [SerializeField]
    private CharacterPanel charPanel;

    [SerializeField]
    private GameObject tooltip;

    private TextMeshProUGUI tooltipText;

    [SerializeField]
    private RectTransform tooltipRect;

    [SerializeField]
    private GameObject targetFrame;

    [SerializeField]
    private TextMeshProUGUI levelText;

    [SerializeField]
    private TextMeshProUGUI currentGold;
    public TextMeshProUGUI MyCurrentGold
    {
        get
        {
            return currentGold;
        }

        set
        {
            currentGold = value;
        }
    }

    private int gold;
    public int MyGold
    {
        get
        {
            return gold;
        }

        set
        {
            this.gold = value;
            this.MyCurrentGold.text = value.ToString();
        }
    }

    [SerializeField]
    private Image face;

    [SerializeField]
    private ActionButton[] actionButtons;

    private GameObject[] keybindButtons;

    [SerializeField]
    private CanvasGroup[] menus;

    private Stat healthStat;

    private bool pause = false;

    private void Awake()
    {
        keybindButtons = GameObject.FindGameObjectsWithTag("Keybind");
        tooltipText = tooltip.GetComponentInChildren<TextMeshProUGUI>();
    }

    // Start is called before the first frame update
    void Start()
    {
        healthStat = targetFrame.GetComponentInChildren<Stat>();
    }

    // Update is called once per frame
    void Update()
    {
        //Pause
        if (Input.GetKeyDown(KeybindManager.MyInstance.Keybinds["Pause"]))
        {
            if (pause)
            {
                Resume(pauseMenu);
            }

            else
            {
                Pause(pauseMenu);
            }
        }

        //Quest log
        if (Input.GetKeyDown(KeybindManager.MyInstance.Keybinds["Questlog"]))
        {
            OpenClose(menus[4]);
        }

        //Skill book
        if (Input.GetKeyDown(KeybindManager.MyInstance.Keybinds["Skillbook"]))
        {
            OpenClose(menus[3]);
        }

        //Inventory
        if (Input.GetKeyDown(KeybindManager.MyInstance.Keybinds["Inventory"]))
        {
            InventoryScript.MyInstance.OpenClose();
        }

        //Character Panel
        if (Input.GetKeyDown(KeybindManager.MyInstance.Keybinds["Character"]))
        {
            charPanel.OpenClose();
        }
    }

    public void ShowTargetFrame(Enemy target)
    {
        targetFrame.SetActive(true);

        healthStat.Initialize(target.MyHealth.MyCurrentValue, target.MyHealth.MyMaxValue);

        face.sprite = target.MyPortrait;

        levelText.text = target.MyLevel.ToString();

        target.healthChanged += new HealthChanged(UpdateTargetFrame);

        target.characterRemoved += new CharacterRemoved(HideTargetFrame);

        if (target.MyLevel >= Player.MyInstance.MyLevel + 5)
        {
            levelText.color = Color.red;
        }

        else if (target.MyLevel == Player.MyInstance.MyLevel + 3 || target.MyLevel == Player.MyInstance.MyLevel + 4)
        {
            levelText.color = new Color32(255, 124, 0, 255);
        }

        else if (target.MyLevel >= Player.MyInstance.MyLevel - 2 && target.MyLevel <= Player.MyInstance.MyLevel + 2)
        {
            levelText.color = Color.yellow;
        }

        else if (target.MyLevel <= Player.MyInstance.MyLevel - 3 && target.MyLevel > XPManager.CalculateGreyLevel())
        {
            levelText.color = Color.green;
        }
        else
        {
            levelText.color = Color.grey;
        }
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
        TextMeshProUGUI tmp = Array.Find(keybindButtons, x => x.name == key).GetComponentInChildren<TextMeshProUGUI>();
        tmp.text = code.ToString();
    }

    public void ClickActionButton(string buttonName)
    {
        Array.Find(actionButtons, x => x.gameObject.name == buttonName).MyButton.onClick.Invoke();
    }

    public void OpenClose(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
        canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true;
    }

    public void OpenSingle(CanvasGroup canvasGroup)
    {
        foreach (CanvasGroup canvas in menus)
        {
            CloseSingle(canvas);
        }

        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
        canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true;
    }

    public void CloseSingle(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }

    //Updates the stacksize on a slot
    public void UpdateStackSize(IClickable clickable)
    {
        if (clickable.MyCount > 1) //more than one
        {
            clickable.MyStackText.text = clickable.MyCount.ToString();
            clickable.MyStackText.enabled = true;
            clickable.MyIcon.enabled = true;
        }

        else // only one
        {
            clickable.MyStackText.enabled = false;
            clickable.MyIcon.enabled = true;
        }

        if (clickable.MyCount == 0) // empty slot
        {
            clickable.MyStackText.enabled = false;
            clickable.MyIcon.enabled = false;
        }
    }

    public void ClearStackCount(IClickable clickable)
    {
        clickable.MyStackText.enabled = false;
        clickable.MyIcon.enabled = true;
    }

    public void ShowTooltip(Vector2 pivot, Vector3 position, IDescribable description)
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

    public void Resume(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        Time.timeScale = 1;
        pause = false;
    }

    public void Pause(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
        Time.timeScale = 0;
        pause = true;
    }


    public void CloseSingleUnpause(CanvasGroup canvasGroup)
    {
        Time.timeScale = 1;
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        pause = false;
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
