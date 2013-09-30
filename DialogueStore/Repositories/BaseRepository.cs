using DialogueStore.Models;

namespace DialogueStore.Repositories
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