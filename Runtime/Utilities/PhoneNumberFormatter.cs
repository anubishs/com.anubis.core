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
    
    // Public property to access the unformatted number
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
        
        // Store previous text for comparison
        string previousUnformatted = unformattedNumber;
        
        // Extract digits and store unformatted number
        unformattedNumber = Regex.Replace(input, @"[^\d]", "");
        
        // Format the phone number
        string formatted = FormatPhoneNumber(unformattedNumber);
        
        // Handle deletion specifically
        if (input.Length < formattedNumber.Length && formatted == formattedNumber)
        {
            // User is trying to delete a formatting character - allow it
            inputField.text = input;
            inputField.caretPosition = Mathf.Min(inputField.caretPosition, input.Length);
        }
        else if (inputField.text != formatted)
        {
            // Apply formatting if it changed
            inputField.text = formatted;
            inputField.caretPosition = formatted.Length;
        }
        
        // Update previous text
        formattedNumber = inputField.text;
        
        isFormatting = false;
    }

    private string FormatPhoneNumber(string digits)
    {
        if (string.IsNullOrEmpty(digits)) return "";
        
        string formatted = "+";
        int index = 0;
        
        // Country code (1-3 digits)
        int countryLen = Mathf.Min(2, digits.Length);
        formatted += digits.Substring(0, countryLen);
        index += countryLen;
        
        if (index >= digits.Length) return formatted;
        
        // Area code (2 digits)
        formatted += "(";
        int areaLen = Mathf.Min(2, digits.Length - index);
        formatted += digits.Substring(index, areaLen);
        index += areaLen;
        
        if (areaLen == 2) formatted += ")";
        if (index >= digits.Length) return formatted;
        
        // First part (4 digits)
        int firstPartLen = Mathf.Min(5, digits.Length - index);
        formatted += digits.Substring(index, firstPartLen);
        index += firstPartLen;
        
        if (index >= digits.Length) return formatted;
        
        // Second part (with dash)
        formatted += "-";
        int secondPartLen = Mathf.Min(4, digits.Length - index);
        formatted += digits.Substring(index, secondPartLen);
        
        return formatted;
    }
    
    // Optional: Reset method to clear both values
    public void ResetPhoneNumber()
    {
        unformattedNumber = "";
        inputField.text = "";
        formattedNumber = "";
    }
}