using System.Collections.Generic;
using PetaPoco;

namespace PetaPoco
{
    interface IRepository {
        T Single<T>(object primaryKey);

        IEnumerable<T> Query<T>();
        List<T> Fetch<T>();
        Page<T> PagedQuery<T>(long pageNumber, long itemsPerPage, string sql, params object[] args);
        int Insert(object itemToAdd);
        int Update(object itemToUpdate, object primaryKeyValue);
        int Delete<T>(object primaryKeyValue);
    }

    public abstract class ITypeRepository<T> {
        public Database db { get; private set; }
        public Database TransactionDatabase { get { return new Database(db.Connection); } }

        protected ITypeRepository(Database db) {
            this.db = db;
        } 

        public void ChangeDb(Database newDb) {
            OnDatabaseChanged(db, newDb);
            db.Dispose();
            db = newDb;
        }

        public virtual void OnDatabaseChanged(Database olddb, Database newdb) {}

    }

    public interface ttt<T> {
        Database db { get; set; }
        void OnDatabaseChanged(Database olddb, Database newdb);
        void ChangeDb();

    }
}
