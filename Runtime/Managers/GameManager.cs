using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public delegate void ChangeScreen();
    public static event ChangeScreen OnChanged;

    public enum MainScreen { Home, Main, Result}
    public MainScreen currentMainScreen;

    [SerializeField] GameObject[] mainScreens;
    public GameObject lockScreen;
    public SubScreenManager currentSubScreenManager;

    MainScreen lastMainScreen;
    [SerializeField] float idleTimer, idleRef;
    public bool isIdle;

    void Awake()
    {
        instance = this;
        if (!PlayerPrefs.HasKey("IdleTimer"))
            PlayerPrefs.SetFloat("IdleTimer", 60);
        Cursor.visible = false;
    }

    void Start()
    {
        idleTimer = PlayerPrefs.GetFloat("IdleTimer");
        idleRef = idleTimer;
        InitializeScreens();
    }

    void InitializeScreens()
    {
        foreach (var screen in mainScreens) screen.SetActive(false);

        ChangeMainScreen((int)currentMainScreen);
        OnChangedEvent();
    }

    void Update()
    {
        // if (lastMainScreen != currentMainScreen) OnChangedEvent();

        HandleIdleTimer();
        if (Input.GetKeyDown(KeyCode.C))
        {
            Cursor.visible = !Cursor.visible;
        }
        if (Input.GetKey(KeyCode.P))
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                ToggleValue("canProceed");
            }
        }
    }
    

    public void ToggleValue(string prefsKey)
    {
        // Get current value (0 if not set)
        int currentValue = PlayerPrefs.GetInt(prefsKey, 0);
        
        // Toggle between 0 and 1
        int newValue = 1 - currentValue;
        
        // Save new value
        PlayerPrefs.SetInt(prefsKey, newValue);
        PlayerPrefs.Save();
        
        Debug.Log($"Key '{prefsKey}' toggled from {currentValue} to {newValue}");
    }

    void HandleIdleTimer()
    {
        if (currentMainScreen == MainScreen.Home)
        {
            idleTimer = idleRef;
            isIdle = false;
        }
        else if (currentMainScreen != MainScreen.Home)
        {
            if (!isIdle)
            {
                idleTimer -= Time.deltaTime;
                if (idleTimer <= 0)
                {
                    ChangeMainScreen(0);
                    mainScreens[1].GetComponent<Animator>().SetTrigger("play");
                    idleTimer = idleRef;
                    isIdle = true;
                }
            }
        }
        else
        {
            if (!isIdle)
            {
                idleTimer -= Time.deltaTime;
                if (idleTimer <= 0)
                {
                    currentSubScreenManager?.PlayScreenAnimation();
                    ChangeMainScreen(0);
                    idleTimer = idleRef;
                    isIdle = true;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartScene();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (Input.anyKey)
        {
            idleTimer = idleRef;
        }
    }

    public void ChangeMainScreen(int screenIndex)
    {
        currentMainScreen = (MainScreen)screenIndex;
        OnChangedEvent();
    }

    public void ChangeSubScreen(int subScreenIndex)
    {
        currentSubScreenManager = mainScreens[(int)currentMainScreen].GetComponent<SubScreenManager>();
        currentSubScreenManager?.InitializeSubScreens();
        currentSubScreenManager?.ChangeSubScreen(subScreenIndex);
        idleTimer = idleRef;
    }

    public void ScreenControl()
    {
        foreach (var screen in mainScreens) screen.SetActive(false);
        GameObject currentScreen = mainScreens[(int)currentMainScreen];
        currentScreen.SetActive(true);


        lastMainScreen = currentMainScreen;
        idleTimer = idleRef;
    }

    public void OnChangedEvent()
    {
        ScreenControl();
        OnChanged?.Invoke();
    }

    void OnEnable() => OnChanged += HandleScreenChange;
    void OnDisable() => OnChanged -= HandleScreenChange;

    void HandleScreenChange()
    {
        Debug.Log($"Changed to {currentMainScreen}");
    }
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}