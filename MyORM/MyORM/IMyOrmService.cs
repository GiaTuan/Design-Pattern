using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace MyORM
{
    public interface IMyOrmService
    {
        bool Connect(string connectionString, string databaseType);
        void Open();

        IMyOrmService Add<T>(T obj) where T : new();
        IMyOrmService Select<T>(string selectedValues = null);
        IMyOrmService Where<T>(Expression<Func<T, bool>> func);
        IMyOrmService GroupBy(string strGroupBy);

        IMyOrmService Having<T>(Expression<Func<T, bool>> func);

        IMyOrmService Delete<T>();
        IMyOrmService Update<T>(T obj) where T : new();

        bool ExecuteNonQuery(string queryString);
        bool ExecuteNonQuery();
        List<T> ExecuteReader<T>(string queryString) where T : new();
        List<T> ExecuteReader<T>() where T : new();

        void Close();

        //===
       // List<object> ExecuteReader2()
    }
}