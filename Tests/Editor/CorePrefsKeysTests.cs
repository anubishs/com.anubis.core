using NUnit.Framework;

public class CorePrefsKeysTests
{
    [Test]
    public void BindingKey_UsesKeyPrefix()
    {
        Assert.AreEqual("KEY_Play", CorePrefsKeys.BindingKey("Play"));
    }

    [Test]
    public void ActivityCountKey_UsesActivitiesPrefix()
    {
        Assert.AreEqual("activities_07_04_2026", CorePrefsKeys.ActivityCountKey("07_04_2026"));
    }

    [Test]
    public void KeyManagerBindingPrefKey_DelegatesToCorePrefsKeys()
    {
        Assert.AreEqual(CorePrefsKeys.BindingKey("OpenMenu"), KeyManager.GetBindingPrefKey("OpenMenu"));
    }
}
