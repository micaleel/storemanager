using Newtonsoft.Json;

namespace DialogueStore.Infrastructure
{
    public class DialogueSettingsProvider
    {
        private const string DefaultPath = "~/App_Data/settings.json";

        public DialogueStoreSettings Load()
        {
            var path = System.Web.Hosting.HostingEnvironment.MapPath(DefaultPath);
            if (path != null && System.IO.File.Exists(path))
            {
                var text = System.IO.File.ReadAllText(path);
                return JsonConvert.DeserializeObject<DialogueStoreSettings>(text);
            }

            return new DialogueStoreSettings
            {
                ApplicationVersion = AppConfigUtil.Get("ApplicationVersion", "0.0.0"),
                ApplicationTitle = AppConfigUtil.Get("ApplicationTitle", "Dialogue Store Manager"),
            };
        }

        public void Save(DialogueStoreSettings settings)
        {
            var text = JsonConvert.SerializeObject(settings);
            var path = System.Web.Hosting.HostingEnvironment.MapPath(DefaultPath);
            if (path != null)
            {
                System.IO.File.WriteAllText(path, text);
            }
        }

    }
}