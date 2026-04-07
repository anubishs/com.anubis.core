using UnityEngine;

public static class PhoneNumberFormatUtility
{
    public static string FormatPhoneNumber(string digits)
    {
        if (string.IsNullOrEmpty(digits)) return "";

        string formatted = "+";
        int index = 0;

        int countryLen = Mathf.Min(2, digits.Length);
        formatted += digits.Substring(0, countryLen);
        index += countryLen;

        if (index >= digits.Length) return formatted;

        formatted += "(";
        int areaLen = Mathf.Min(2, digits.Length - index);
        formatted += digits.Substring(index, areaLen);
        index += areaLen;

        if (areaLen == 2) formatted += ")";
        if (index >= digits.Length) return formatted;

        int firstPartLen = Mathf.Min(5, digits.Length - index);
        formatted += digits.Substring(index, firstPartLen);
        index += firstPartLen;

        if (index >= digits.Length) return formatted;

        formatted += "-";
        int secondPartLen = Mathf.Min(4, digits.Length - index);
        formatted += digits.Substring(index, secondPartLen);

        return formatted;
    }
}
