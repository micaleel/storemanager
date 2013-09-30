namespace DialogueStore.Web.Infrastructure
{
    public static class StringExtensions
    {
        public static string ToTitleCase(this string s)
        {
            return System.Globalization.CultureInfo.CurrentUICulture.TextInfo.ToTitleCase(s);
        }
    }
}