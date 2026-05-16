namespace FastCheckout;
using static Helper;
public class EPCDecoder
{
    public static string ProcessEpcIds(string rfid)
{
    if (string.IsNullOrEmpty(rfid))
        return null;
        
    string firstTwoChars = rfid.Substring(0, 2);
    string barcode = null;
    
    switch (firstTwoChars)
    {
        case "05":
            // Truncate to 96 bits (24 hex characters) if longer
            if (rfid.Length > 24)
                rfid = rfid.Substring(0, 24);
            string lastChars = rfid.Substring(rfid.Length - 10);
            barcode = HexBytesToDecimal(lastChars).ToString();
            break;
            
        case "FA":
            string lastCharsFA = rfid.Substring(rfid.Length - 14);
            barcode = HexBytesToDecimal(lastCharsFA).ToString();
            break;
            
        case "30":
        case "E2":
            barcode = DecodeSgtin96(rfid);
            break;
            
        case "85":
            barcode = ChainlaineDecode(rfid);
            break;
            
        default:
            // Log unhandled RFID scheme
            break;
    }
    
    return barcode;
}
    public static string DecodeSgtin96(string epcHex)
{
    string binaryEpc = HexStringToBinary(epcHex);
    
    int partition = Convert.ToInt32(binaryEpc.Substring(11, 3), 2);
    
    int[] companyPrefixLengths = { 40, 37, 34, 30, 27, 24, 20 };
    int[] itemReferenceLengths = { 4, 7, 10, 14, 17, 20, 24 };
    
    int companyPrefixLength = companyPrefixLengths[partition];
    int itemReferenceLength = itemReferenceLengths[partition];
    
    string companyPrefix = binaryEpc.Substring(14, companyPrefixLength);
    string itemReference = binaryEpc.Substring(14 + companyPrefixLength, itemReferenceLength);
    
    string companyPrefixDec = Convert.ToInt32(companyPrefix, 2).ToString().PadLeft(companyPrefixLength / 4, '0');
    string itemReferenceDec = Convert.ToInt32(itemReference, 2).ToString().PadLeft(itemReferenceLength / 4, '0');
    
    string gtinWithoutCheckDigit = companyPrefixDec + itemReferenceDec;
    int checkDigit = CalculateCheckDigit(gtinWithoutCheckDigit);
    
    return gtinWithoutCheckDigit + checkDigit.ToString();
}
public static string ChainlaineDecode(string rfid)
{
    string hexBarcode = rfid.Substring(rfid.Length - 10);
    string itemType = rfid.Substring(9, 4);
    
    long barcode = Convert.ToInt64(hexBarcode, 16);
    string itemSize = null;
    
    switch (itemType)
    {
        case "7334": itemSize = "XS"; break;
        case "6340": itemSize = "Y"; break;
        case "6280": itemSize = "S"; break;
        case "61C0": itemSize = "M"; break;
        case "61A0": itemSize = "L"; break;
        case "732D": itemSize = "XL"; break;
        case "3320": itemSize = "X"; break;
        case "7214": itemSize = "OS"; break;
        default:
            // Log warning about unknown item size
            break;
    }
    
    if (itemSize != null)
        return barcode.ToString() + itemSize;
        
    return null;
}
}