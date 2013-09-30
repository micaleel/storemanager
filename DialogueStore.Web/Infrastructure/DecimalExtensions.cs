namespace DialogueStore.Web.Infrastructure
{
    public static class DecimalExtensions
    {
        public static string FormatCurrency(decimal amount)
        {
            return amount.ToString("c");
        }
    }
}