public static class CorePrefsKeys
{
    public const string IdleTimer = "IdleTimer";
    public const string CanProceed = "CanProceed";
    public const string ActivitiesPrefix = "activities_";
    public const string KeyPrefix = "KEY_";

    public static string ActivityCountKey(string dateKey)
    {
        return ActivitiesPrefix + dateKey;
    }

    public static string BindingKey(string actionName)
    {
        return KeyPrefix + actionName;
    }
}
