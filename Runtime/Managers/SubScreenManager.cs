using UnityEngine;

public class SubScreenManager : MonoBehaviour
{
    [SerializeField] GameObject[] subScreens;
    private int currentSubScreenIndex;

    public void InitializeSubScreens()
    {
        if (subScreens == null)
            return;

        foreach (var screen in subScreens)
            if (screen != null)
                screen.SetActive(false);
    }

    public void ChangeSubScreen(int index)
    {
        if (subScreens == null || subScreens.Length == 0)
        {
            Debug.LogWarning("[SubScreenManager] No sub screens configured.");
            return;
        }

        if (index < 0 || index >= subScreens.Length)
        {
            Debug.LogWarning($"[SubScreenManager] Invalid sub screen index: {index}");
            return;
        }

        InitializeSubScreens();
        currentSubScreenIndex = index;

        if (subScreens[currentSubScreenIndex] != null)
            subScreens[currentSubScreenIndex].SetActive(true);
    }

    public void PlayScreenAnimation()
    {
        if (subScreens == null || currentSubScreenIndex < 0 || currentSubScreenIndex >= subScreens.Length)
            return;

        subScreens[currentSubScreenIndex]?.GetComponent<Animator>()?.SetTrigger("play");
    }
}
