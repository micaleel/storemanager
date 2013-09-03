using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using StoreManager.Models;

namespace StoreManager.Repositories {
    public abstract class EntityRepository<TEntity> : BaseRepository where TEntity : class {
        protected EntityRepository(StoreManagerContext db)
            : base(db) {
        }

        public virtual DbSet<TEntity> All {
            get { return Db.Set<TEntity>(); }
        }

        public virtual TEntity Find(params object[] keyValues) {
            return Db.Set<TEntity>().Find(keyValues);
        }

        public virtual TEntity Add(TEntity entity) {
            Db.Set<TEntity>().Add(entity);
            Db.SaveChanges();

            return entity;
        }

        public virtual TEntity Update(TEntity entity) {
            Db.Entry(entity).State = EntityState.Modified;
            Db.SaveChanges();

            return entity;
        }

        public virtual void Delete(int id) {
            var entity = Find(id);
            if (entity == null) throw new EntityNotFoundException();

            Db.Set<TEntity>().Remove(entity);
            Db.SaveChanges();
        }
    }
}