using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KeybindManager : MonoBehaviour
{
    private static KeybindManager instance;

    public static KeybindManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<KeybindManager>();
            }

            return instance;
        }
    }

    public Dictionary<string, KeyCode> Keybinds { get; private set; }

    public Dictionary<string, KeyCode> ActionBinds { get; set; }

    private string bindName;
    
    void Start()
    {
        Keybinds = new Dictionary<string, KeyCode>();
        ActionBinds = new Dictionary<string, KeyCode>();

        BindKey("Up", KeyCode.W);
        BindKey("Right", KeyCode.D);
        BindKey("Down", KeyCode.S);
        BindKey("Left", KeyCode.A);

        BindKey("Melee", KeyCode.F);
        BindKey("Default", KeyCode.Space);

        BindKey("Act1", KeyCode.Alpha1);
        BindKey("Act2", KeyCode.Alpha2);
        BindKey("Act3", KeyCode.Alpha3);
        BindKey("Act4", KeyCode.Alpha4);
        BindKey("Act5", KeyCode.Alpha5);
        BindKey("Act6", KeyCode.Alpha6);
        BindKey("Act7", KeyCode.Alpha7);
        BindKey("Act8", KeyCode.Alpha8);
        BindKey("Act9", KeyCode.Alpha9);
        BindKey("Act10", KeyCode.Alpha0);
        BindKey("Act11", KeyCode.Minus);
        BindKey("Act12", KeyCode.Equals);
    }
    
    public void BindKey(string key, KeyCode keyBind)
    {
        Dictionary<string, KeyCode> currentDictionary = Keybinds;

        if(key.Contains("Act"))
        {
            currentDictionary = ActionBinds;
        }

        if(!currentDictionary.ContainsKey(key))
        {
            currentDictionary.Add(key, keyBind);
            UIManager.MyInstance.UpdateKeyText(key, keyBind);
        }

        else if (currentDictionary.ContainsValue(keyBind))
        {
            string myKey = currentDictionary.FirstOrDefault(x => x.Value == keyBind).Key;
            currentDictionary[myKey] = KeyCode.None;
            UIManager.MyInstance.UpdateKeyText(key, KeyCode.None);

        }

        currentDictionary[key] = keyBind;
        UIManager.MyInstance.UpdateKeyText(key, keyBind);
        bindName = string.Empty;
    }

    public void KeyBindOnClick(string bindName)
    {
        this.bindName = bindName;
    }

    private void OnGUI()
    {
        if (bindName != string.Empty)
        {
            Event e = Event.current;

            if(e.isKey)
            {
                BindKey(bindName, e.keyCode);
            }
        }
    }
}
