using UnityEngine;

public class MultiDisplayActivator : MonoBehaviour
{
    void Start()
    {
        // Check how many displays are connected
        int displayCount = Display.displays.Length;
        
        Debug.Log("Displays connected: " + displayCount);

        // Enable all detected displays
        for (int i = 1; i < displayCount; i++)  // Start from 1 (primary is already active)
        {
            Display.displays[i].Activate();
            Debug.Log("Activated display " + i);
        }
    }

    void Update()
    {
        // Optional: Press F1 to refresh displays during runtime
        if (Input.GetKeyDown(KeyCode.F1))
        {
            RefreshDisplays();
        }
    }

    void RefreshDisplays()
    {
        int displayCount = Display.displays.Length;
        for (int i = 1; i < displayCount; i++)
        {
            if (!Display.displays[i].active)
            {
                Display.displays[i].Activate();
                Debug.Log("Reactivated display " + i);
            }
        }
    }
}