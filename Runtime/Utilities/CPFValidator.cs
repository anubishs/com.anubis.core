using System.Text.RegularExpressions;

public static class CPFValidator
{
    public static bool IsValid(string cpf, bool mask, out string result)
    {
        result = string.Empty;

        if (string.IsNullOrEmpty(cpf))
            return false;

        // Remove non-numeric characters
        cpf = Regex.Replace(cpf, "[^0-9]", "");

        if (cpf.Length != 11)
            return false;

        if (IsAllSameDigits(cpf))
            return false;

        if (!ValidateDigits(cpf))
            return false;

        // Output formatting
        result = mask ? ApplyMask(cpf) : cpf;
        return true;
    }

    private static bool IsAllSameDigits(string cpf)
    {
        char first = cpf[0];
        for (int i = 1; i < cpf.Length; i++)
        {
            if (cpf[i] != first)
                return false;
        }
        return true;
    }

    private static bool ValidateDigits(string cpf)
    {
        int[] numbers = new int[11];

        for (int i = 0; i < 11; i++)
            numbers[i] = cpf[i] - '0';

        int sum1 = 0;
        for (int i = 0; i < 9; i++)
            sum1 += numbers[i] * (10 - i);

        int digit1 = (sum1 % 11) < 2 ? 0 : 11 - (sum1 % 11);
        if (numbers[9] != digit1)
            return false;

        int sum2 = 0;
        for (int i = 0; i < 10; i++)
            sum2 += numbers[i] * (11 - i);

        int digit2 = (sum2 % 11) < 2 ? 0 : 11 - (sum2 % 11);
        return numbers[10] == digit2;
    }

    private static string ApplyMask(string cpf)
    {
        return $"{cpf.Substring(0, 3)}.{cpf.Substring(3, 3)}.{cpf.Substring(6, 3)}-{cpf.Substring(9, 2)}";
    }
}