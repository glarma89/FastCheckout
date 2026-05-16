namespace FastCheckout
{
    internal static class LogFormatter
    {
        internal static string Exception(Exception ex)
        {
            return
                $"Message:\n" +
                $"{ex.Message}" +
                $"Inner:\n" +
                $"{ex.InnerException}" +
                $"Trace:\n" +
                $"{ex.StackTrace}";
        }
    }
}
