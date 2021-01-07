using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MyORM
{
    public interface IMyORM
    {
        bool Connect(string connectionString, string databaseType);
        void Open();

        IMyORM Add<T>(T obj) where T : new();
        IMyORM Select<T>(string selectedValues = null);
        IMyORM Where<T>(Expression<Func<T, bool>> func);
        IMyORM GroupBy(string strGroupBy);
        IMyORM Having<T>(Expression<Func<T, bool>> func);
        IMyORM Delete<T>();
        IMyORM Update<T>(T obj) where T : new();

        bool ExecuteNonQuery(string queryString);
        bool ExecuteNonQuery();
        List<T> ExecuteReader<T>(string queryString) where T : new();
        List<T> ExecuteReader<T>() where T : new();

        void Close();


        IMyORM GroupBy<T>(Expression<Func<T, object>> func);
    }
}
