using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MyORM.ORM
{
    class MyORMProxy : IMyORM
    {
        MyOrm service;

        public bool Connect(string connectionString, string databaseType)
        {
            if (service == null)
            {
                service = new MyOrm();
                return service.Connect(connectionString, databaseType);
            }
            else return false;
        }

        public void Open()
        {
            service.Open();
        }

        public IMyORM Add<T>(T obj) where T : new()
        {
            return service.Add<T>(obj);
        }

        public IMyORM Select<T>(string selectedValues = null)
        {
            return service.Select<T>(selectedValues);
        }

        public IMyORM Where<T>(Expression<Func<T, bool>> func)
        {
            return service.Where<T>(func);
        }

        public IMyORM Delete<T>()
        {
            return service.Delete<T>();
        }

        public IMyORM Update<T>(T obj) where T : new()
        {
            return service.Update<T>(obj);
        }

        public IMyORM GroupBy(string strGroupBy)
        {
            return service.GroupBy(strGroupBy);
        }

        public IMyORM Having<T>(Expression<Func<T, bool>> func)
        {
            return service.Having(func);
        }

        public List<T> ExecuteReader<T>() where T : new()
        {
            return service.ExecuteReader<T>();
        }

        public List<T> ExecuteReader<T>(string queryString) where T : new()
        {
            return service.ExecuteReader<T>(queryString);
        }

        public bool ExecuteNonQuery(string queryString)
        {
            return service.ExecuteNonQuery(queryString);
        }

        public bool ExecuteNonQuery()
        {
            return service.ExecuteNonQuery();
        }

        public void Close()
        {
            service.Close();
        }



        public IMyORM GroupBy<T>(Expression<Func<T, object>> func)
        {
            return service.GroupBy<T>(func);
        }
    }
}
