using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyManager : MonoBehaviour
{
    public static KeyManager instance;
    public GameObject debugPanel;

    [Serializable]
    public class KeyBinding
    {
        public string actionName;
        public KeyCode defaultKey;
        public KeyCode currentKey;
        public Button targetButton;
    }

    [Header("Bindings")]
    public List<KeyBinding> bindings = new List<KeyBinding>();

    void Awake()
    {
        instance = this;
        LoadKeys();
    }

    void Update()
    {
        foreach (var binding in bindings)
        {
            if (Input.GetKeyDown(binding.currentKey))
            {
                if (binding.targetButton != null && binding.targetButton.interactable)
                    binding.targetButton.onClick.Invoke();
            }
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            debugPanel.SetActive(!debugPanel.activeInHierarchy);
            Cursor.visible = !Cursor.visible;
        }
    }
    public string GetActionDown()
    {
        foreach (var binding in bindings)
        {
            if (Input.GetKeyDown(binding.currentKey))
            {
                return binding.actionName;
            }
        }

        return null;
    }

    // ================= LOAD =================

    void LoadKeys()
    {
        foreach (var binding in bindings)
        {
            string pref = "KEY_" + binding.actionName;

            if (PlayerPrefs.HasKey(pref))
            {
                binding.currentKey =
                    (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(pref));
            }
            else
            {
                binding.currentKey = binding.defaultKey;
            }
        }
    }

    // ================= SAVE =================

    public void SetKey(string actionName, KeyCode newKey)
    {
        KeyBinding target = null;

        foreach (var b in bindings)
        {
            if (b.actionName == actionName)
                target = b;
        }

        if (target == null)
            return;

        // Duplicate prevention (swap keys)
        foreach (var b in bindings)
        {
            if (b != target && b.currentKey == newKey)
            {
                b.currentKey = target.currentKey;
                SaveKey(b);
            }
        }

        target.currentKey = newKey;
        SaveKey(target);
    }

    void SaveKey(KeyBinding binding)
    {
        PlayerPrefs.SetString("KEY_" + binding.actionName, binding.currentKey.ToString());
        PlayerPrefs.Save();
    }

    // ================= GET =================

    public KeyCode GetKey(string actionName)
    {
        foreach (var b in bindings)
        {
            if (b.actionName == actionName)
                return b.currentKey;
        }

        return KeyCode.None;
    }

    // ================= FRIENDLY DISPLAY =================

    public string GetKeyDisplay(KeyCode key)
    {
        string k = key.ToString();

        if (k.StartsWith("Alpha"))
            return k.Replace("Alpha", "");

        if (k.StartsWith("Keypad"))
            return "Numpad " + k.Replace("Keypad", "");

        return k;
    }
}