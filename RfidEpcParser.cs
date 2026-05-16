namespace FastCheckout;

using Serilog;
using static EPCDecoder;

public static class RfidEpcParser
{
    public static string ExtractBarcodeFromRfid(string rawRfid)
    {
        try
        {
            return ProcessEpcIds(rawRfid);
        }
        catch (Exception ex)
        {
            Log.Error(LogFormatter.Exception(ex));
            return null;
        }
    }
}