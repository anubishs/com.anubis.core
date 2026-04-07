using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public delegate void ChangeScreen();
    public static event ChangeScreen OnChanged;

    public enum MainScreen { Home, Main, Result }
    public MainScreen currentMainScreen;

    [SerializeField] GameObject[] mainScreens;
    public GameObject lockScreen;
    public SubScreenManager currentSubScreenManager;

    MainScreen lastMainScreen;
    [SerializeField] float idleTimer, idleRef;
    public bool isIdle;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        if (!PlayerPrefs.HasKey(CorePrefsKeys.IdleTimer))
            PlayerPrefs.SetFloat(CorePrefsKeys.IdleTimer, 60);

        Cursor.visible = false;
    }

    void Start()
    {
        idleTimer = PlayerPrefs.GetFloat(CorePrefsKeys.IdleTimer);
        idleRef = idleTimer;
        InitializeScreens();
    }

    void InitializeScreens()
    {
        if (mainScreens == null || mainScreens.Length == 0)
        {
            Debug.LogError("[GameManager] Main screens are not configured.");
            return;
        }

        foreach (var screen in mainScreens)
            if (screen != null)
                screen.SetActive(false);

        ChangeMainScreen((int)currentMainScreen);
        OnChangedEvent();
    }

    void Update()
    {
        HandleIdleTimer();

        if (Input.GetKeyDown(KeyCode.C))
            Cursor.visible = !Cursor.visible;

        if (Input.GetKey(KeyCode.P) && Input.GetKeyDown(KeyCode.L))
            ToggleValue(CorePrefsKeys.CanProceed);
    }

    public void ToggleValue(string prefsKey)
    {
        int currentValue = PlayerPrefs.GetInt(prefsKey, 0);
        int newValue = 1 - currentValue;

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
        else
        {
            if (!isIdle)
            {
                idleTimer -= Time.deltaTime;
                if (idleTimer <= 0)
                {
                    currentSubScreenManager?.PlayScreenAnimation();
                    ChangeMainScreen(0);

                    if (mainScreens != null && mainScreens.Length > 1 && mainScreens[1] != null)
                        mainScreens[1].GetComponent<Animator>()?.SetTrigger("play");

                    idleTimer = idleRef;
                    isIdle = true;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
            RestartScene();

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        if (Input.anyKey)
            idleTimer = idleRef;
    }

    public void ChangeMainScreen(int screenIndex)
    {
        if (mainScreens == null || screenIndex < 0 || screenIndex >= mainScreens.Length)
        {
            Debug.LogError($"[GameManager] Invalid main screen index: {screenIndex}");
            return;
        }

        currentMainScreen = (MainScreen)screenIndex;
        OnChangedEvent();
    }

    public void ChangeSubScreen(int subScreenIndex)
    {
        if (mainScreens == null || mainScreens.Length == 0)
            return;

        var currentScreen = mainScreens[(int)currentMainScreen];
        if (currentScreen == null)
            return;

        currentSubScreenManager = currentScreen.GetComponent<SubScreenManager>();
        currentSubScreenManager?.InitializeSubScreens();
        currentSubScreenManager?.ChangeSubScreen(subScreenIndex);
        idleTimer = idleRef;
    }

    public void ScreenControl()
    {
        if (mainScreens == null || mainScreens.Length == 0)
            return;

        foreach (var screen in mainScreens)
            if (screen != null)
                screen.SetActive(false);

        GameObject currentScreen = mainScreens[(int)currentMainScreen];
        if (currentScreen != null)
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
