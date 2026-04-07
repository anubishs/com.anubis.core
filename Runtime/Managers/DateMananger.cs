using UnityEngine;
using System;

public class DateMananger : MonoBehaviour
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
        if (!PlayerPrefs.HasKey("CanProceed"))
            PlayerPrefs.SetInt("CanProceed", 0);
    }

    void ParseDates()
    {
        try
        {
            // Parse dates from strings (using European DD/MM/YYYY format)
            startDate = DateTime.ParseExact(startDateString, "dd/MM/yyyy", null);
            endDate = DateTime.ParseExact(endDateString, "dd/MM/yyyy", null);
            
            // Include full end day (until end of day)
            endDate = endDate.AddDays(1).AddSeconds(-1);
        }
        catch (FormatException)
        {
            Debug.LogError("Invalid date format! Use DD/MM/YYYY format.");
            // enabled = false;
        }
    }

    public bool IsWithinDateRange()
    {
        currentDate = DateTime.Now;
        return currentDate >= startDate && currentDate <= endDate;
    }

    void CheckCurrentDate()
    {
        if (PlayerPrefs.GetInt("canProceed") == 1) return;
        if (IsWithinDateRange())
        {
            Debug.Log("Current date is WITHIN the specified range");
            // Add your custom logic here
            GameManager.instance.lockScreen.SetActive(false);
        }
        else
        {
            Debug.Log("Current date is NOT in the specified range");
            GameManager.instance.lockScreen.SetActive(true);
        }
    }

    // For testing in the Inspector
    void OnValidate()
    {
        ParseDates();
    }
}