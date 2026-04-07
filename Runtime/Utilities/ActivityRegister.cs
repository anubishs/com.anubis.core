using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class ActivityRegister : MonoBehaviour
{
    [Header("Status")]
    public static int totalActivities = 0;
    public static List<string> knownTimes = new List<string>();

    [Header("File Settings")]
    public static string dataFolderName = "Dados";
    public static string dataSubfolderName = "";
    public static bool registerHours = false;

    public void Awake()
    {
        dataSubfolderName = Application.productName;
    }

    private static string GetCurrentDate()
    {
        var date = System.DateTime.Now;
        return date.Day + "_" + date.Month + "_" + date.Year;
    }

    private static string GetCurrentTime()
    {
        var data = System.DateTime.Now;
        return data.Hour + ":" + data.Minute + ":" + data.Second;
    }

    private static int GetTotalActivities()
    {
        return PlayerPrefs.GetInt(CorePrefsKeys.ActivityCountKey(GetCurrentDate()), 0);
    }

    private static void SetTotalActivities(int val)
    {
        PlayerPrefs.SetInt(CorePrefsKeys.ActivityCountKey(GetCurrentDate()), val);
    }

    public static void RegisterActivity()
    {
        ReadActivityFile();

        totalActivities = GetTotalActivities();
        SetTotalActivities(totalActivities + 1);

        WriteActivityFile();
    }

    private static void ReadActivityFile()
    {
        string path = GetActivityFilePath();
        Debug.Log("[ActivityRegister - Read] Reading from: " + path);

        knownTimes.Clear();

        if (!File.Exists(path))
        {
            Debug.LogWarning("[ActivityRegister] " + path + " does not exist!");
            return;
        }

        var lines = File.ReadAllLines(path);

        for (int i = 0; i < lines.Length; i++)
        {
            if (!lines[i].Contains("//") && !lines[i].Contains("Total de jogadas:"))
                knownTimes.Add(lines[i]);
        }
    }

    private static void WriteActivityFile()
    {
        string desktop = GetDesktopDir();
        string folderPath = GetActivityFolderPath();
        string path = GetActivityFilePath();

        Debug.Log("[ActivityRegister - Write] Writing at: " + path);

        if (File.Exists(path))
            File.Delete(path);

        if (!Directory.Exists(Path.Combine(desktop, dataFolderName)))
            Directory.CreateDirectory(Path.Combine(desktop, dataFolderName));

        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        using (var file = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write))
        using (var writer = new StreamWriter(file))
        {
            writer.WriteLine("Total de jogadas: " + GetTotalActivities());
            if (registerHours)
            {
                writer.WriteLine("//HORÁRIOS");
                knownTimes.Add(GetCurrentTime());

                for (int i = 0; i < knownTimes.Count; i++)
                    writer.WriteLine(knownTimes[i]);
            }
        }

        Debug.Log("[ActivityRegister - Write] Saved Sucessfully - " + System.DateTime.Now);
    }

    private static string GetDesktopDir()
    {
        return System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
    }

    private static string GetActivityFolderPath()
    {
        return Path.Combine(GetDesktopDir(), dataFolderName, dataSubfolderName);
    }

    private static string GetActivityFilePath()
    {
        return Path.Combine(GetActivityFolderPath(), GetCurrentDate() + ".txt");
    }
}
