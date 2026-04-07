using UnityEngine;
using System;

public class DateManager : MonoBehaviour
{
    [Header("Date Range")]
    [Tooltip("Start date in DD/MM/YYYY format")]
    public string startDateString = "30/05/2025";

    [Tooltip("End date in DD/MM/YYYY format")]
    public string endDateString = "04/06/2025";

    private DateTime startDate;
    private DateTime endDate;
    private DateTime currentDate;

    void Start()
    {
        ParseDates();
        CheckCurrentDate();
        if (!PlayerPrefs.HasKey(CorePrefsKeys.CanProceed))
            PlayerPrefs.SetInt(CorePrefsKeys.CanProceed, 0);
    }

    void ParseDates()
    {
        try
        {
            startDate = DateTime.ParseExact(startDateString, "dd/MM/yyyy", null);
            endDate = DateTime.ParseExact(endDateString, "dd/MM/yyyy", null);
            endDate = endDate.AddDays(1).AddSeconds(-1);
        }
        catch (FormatException)
        {
            Debug.LogError("Invalid date format! Use DD/MM/YYYY format.");
        }
    }

    public bool IsWithinDateRange()
    {
        currentDate = DateTime.Now;
        return currentDate >= startDate && currentDate <= endDate;
    }

    void CheckCurrentDate()
    {
        if (PlayerPrefs.GetInt(CorePrefsKeys.CanProceed) == 1) return;

        if (GameManager.instance == null || GameManager.instance.lockScreen == null)
        {
            Debug.LogWarning("[DateManager] Missing GameManager or lockScreen reference.");
            return;
        }

        if (IsWithinDateRange())
        {
            Debug.Log("Current date is WITHIN the specified range");
            GameManager.instance.lockScreen.SetActive(false);
        }
        else
        {
            Debug.Log("Current date is NOT in the specified range");
            GameManager.instance.lockScreen.SetActive(true);
        }
    }

    void OnValidate()
    {
        ParseDates();
    }
}
