using System.Text;
using System.Text.RegularExpressions;

namespace FastCheckout;

public class Helper
{
    // Convert hex string to binary
public static string HexStringToBinary(string hexString)
{
    hexString = Regex.Replace(hexString, @"\s+", ""); // Remove spaces
    
    StringBuilder binary = new StringBuilder();
    foreach (char c in hexString)
    {
        binary.Append(Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0'));
    }
    return binary.ToString();
}

// Convert hex bytes to decimal
public static long HexBytesToDecimal(string hexStr)
{
    return Convert.ToInt64(hexStr, 16);
}

// Calculate GTIN check digit
public static int CalculateCheckDigit(string gtinWithoutCheckDigit)
{
    int totalSum = 0;
    for (int i = 0; i < gtinWithoutCheckDigit.Length; i++)
    {
        int digit = int.Parse(gtinWithoutCheckDigit[i].ToString());
        if (i % 2 == 0) // Odd positions
            totalSum += digit;
        else // Even positions
            totalSum += digit * 3;
    }
    int mod = totalSum % 10;
    return mod == 0 ? 0 : 10 - mod;
}
}