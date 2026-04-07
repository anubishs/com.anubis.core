using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

[RequireComponent(typeof(TMP_InputField))]
public class PhoneNumberFormatter : MonoBehaviour
{
    private TMP_InputField inputField;
    private bool isFormatting;
    public string unformattedNumber = "";
    public string formattedNumber = "";

    public string UnformattedPhoneNumber => unformattedNumber;

    void Start()
    {
        inputField = GetComponent<TMP_InputField>();
        inputField.onValueChanged.AddListener(FormatText);
        formattedNumber = inputField.text;
    }

    void FormatText(string input)
    {
        if (isFormatting) return;

        isFormatting = true;

        unformattedNumber = Regex.Replace(input, @"[^\d]", "");
        string formatted = PhoneNumberFormatUtility.FormatPhoneNumber(unformattedNumber);

        if (input.Length < formattedNumber.Length && formatted == formattedNumber)
        {
            inputField.text = input;
            inputField.caretPosition = Mathf.Min(inputField.caretPosition, input.Length);
        }
        else if (inputField.text != formatted)
        {
            inputField.text = formatted;
            inputField.caretPosition = formatted.Length;
        }

        formattedNumber = inputField.text;

        isFormatting = false;
    }

    public void ResetPhoneNumber()
    {
        unformattedNumber = "";
        inputField.text = "";
        formattedNumber = "";
    }
}
