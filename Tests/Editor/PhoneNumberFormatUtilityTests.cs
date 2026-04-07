using NUnit.Framework;

public class PhoneNumberFormatUtilityTests
{
    [TestCase("", "")]
    [TestCase("1", "+1")]
    [TestCase("1234", "+12(34)")]
    [TestCase("12345678901", "+12(34)56789-01")]
    [TestCase("5511998765432", "+55(11)99876-5432")]
    public void FormatPhoneNumber_ReturnsExpectedFormat(string digits, string expected)
    {
        var result = PhoneNumberFormatUtility.FormatPhoneNumber(digits);
        Assert.AreEqual(expected, result);
    }
}
