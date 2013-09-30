using DialogueStore.Web.Models;

namespace DialogueStore.Web.Repositories
{
    public abstract class BaseRepository
    {
        protected readonly DialogueStoreContext Db;

        protected BaseRepository(DialogueStoreContext db)
        {
            Db = db;
        }
    }
}