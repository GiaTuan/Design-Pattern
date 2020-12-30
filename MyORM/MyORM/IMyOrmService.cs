using System.Collections.Generic;

namespace MyORM
{
    public interface IMyOrmService
    {
        bool Connect(string connectionString, string databaseType);
        void Open();
        bool Add<T>(T obj) where T : new(); //Insert One
        IMyOrmService SelectAll<T>();
        List<T> ExecuteQuery<T>(string queryString) where T : new();
        List<T> ExecuteQuery<T>() where T : new();
        void Close();


        bool Update<T>(T obj) where T : new(); //Update One
        int SaveChange();

    }
}
