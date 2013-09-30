using System.Configuration;

namespace DialogueStore.Infrastructure
{
    public static class AppConfigUtil
    {
        public static T Convert<T>(string value)
        {
            return (T)System.Convert.ChangeType(value, typeof(T));
        }

        public static TValue Get<TValue>(string key, TValue fallback)
        {
            return ConfigurationManager.AppSettings[key] != null ?
                       Convert<TValue>(ConfigurationManager.AppSettings[key])
                       : fallback;
        }

        public static TValue Get<TValue>(string key)
        {
            return ConfigurationManager.AppSettings[key] != null ?
                       Convert<TValue>(ConfigurationManager.AppSettings[key])
                       : default(TValue);
        }
    }
}