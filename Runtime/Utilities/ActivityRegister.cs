using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ActivityRegister : MonoBehaviour
{
    [Header("Status")]
    public static int totalActivities = 0;
    public static List<string> knownTimes = new List<string>();

    [Header("File Settings")]
    public static string dataFolderName = "Dados";
    public static string dataSubfolderName = ""; //INSERT COMPANY NAME HERE!
    public static bool registerHours = false; //If set to true, will register each hour the register process was called.


    public void Awake()
    {
        dataSubfolderName = Application.productName;
    }
    private static string GetCurrentDate()
    {
        var _date = System.DateTime.Now;
        return _date.Day.ToString() + "_" + _date.Month.ToString() + "_" + _date.Year.ToString();
    }
    private static string GetCurrentTime()
    {
        var _data = System.DateTime.Now;
        return _data.Hour.ToString() + ":" + _data.Minute.ToString() + ":" + _data.Second.ToString();
    }
    private static int GetTotalActivities()
    {
        return PlayerPrefs.GetInt("activities_" + GetCurrentDate(), 0);
    }
    private static void SetTotalActivities(int val)
    {
        PlayerPrefs.SetInt("activities_" + GetCurrentDate(), val);
    }
    //Call this whenever we want to register a finished activity.
    public static void RegisterActivity()
    {
        ReadActivityFile();

        var _time = GetCurrentTime();
        totalActivities = GetTotalActivities();
        SetTotalActivities(totalActivities + 1);

        WriteActivityFile();
    }
    private static void ReadActivityFile()
    {
        string _path = GetDesktopDir() + GetActivityFolderDir() + GetActivityFileName();
        Debug.Log("[ActivityRegister - Read] Reading from: " + _path);

        if (File.Exists(_path))
        {
            var lines = File.ReadAllLines(_path);

            for (int i = 0; i < lines.Length; i++)
            {
                if (!lines[i].Contains("//") && !lines[i].Contains("Total de jogadas:"))
                {
                    knownTimes.Add(lines[i]);
                }
            }
        }
        else
        {
            Debug.LogWarning("[ActivityRegister]" + _path + " does not exist!");
        }
    }
    private static void WriteActivityFile()
    {
        string _path = GetDesktopDir() + GetActivityFolderDir() + GetActivityFileName();
        Debug.Log("[ActivityRegister - Write] Writing at: " + _path);

        if (File.Exists(_path))
        {
            File.Delete(_path);
        }
        if (!Directory.Exists(GetDesktopDir() + @"\" + dataFolderName))
        {
            Directory.CreateDirectory(GetDesktopDir() + @"\" + dataFolderName);
        }
        if (!Directory.Exists(GetDesktopDir() + GetActivityFolderDir()))
        {
            Directory.CreateDirectory(GetDesktopDir() + GetActivityFolderDir());
        }

        FileStream file = File.Open(_path, FileMode.OpenOrCreate, FileAccess.Write);
        StreamWriter writer = new StreamWriter(file);
        writer.WriteLine("Total de jogadas: " + GetTotalActivities());
        if (registerHours)
        {
            writer.WriteLine("//HORÁRIOS");
            knownTimes.Add(GetCurrentTime());
            for (int i = 0; i < knownTimes.Count; i++)
            {
                writer.WriteLine(knownTimes[i]);
            }
        }
        writer.Close();
        Debug.Log("[ActivityRegister - Write] Saved Sucessfully - " + System.DateTime.Now);

    }
    private static string GetDesktopDir()
    {
        return System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
    }
    private static string GetActivityFolderDir()
    {
        return @"\" + dataFolderName + @"\" + dataSubfolderName;
    }
    private static string GetActivityFileName()
    {
        return @"\" + GetCurrentDate() + ".txt";
    }

}
