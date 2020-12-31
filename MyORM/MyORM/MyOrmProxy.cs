using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace MyORM
{
    public class MyOrmProxy : IMyOrmService
    {
        MyOrmService service;

        public bool Connect(string connectionString, string databaseType)
        {
            if (service == null)
            {
                service = new MyOrmService();
                return service.Connect(connectionString, databaseType);
            }
            else return false;
        }

        public void Open()
        {
            service.Open();
        }
        public IMyOrmService Add<T>(T obj) where T : new ()
        {
            return service.Add<T>(obj);
        }

        public IMyOrmService Select<T>(string selectedValues = null)
        {
            return service.Select<T>(selectedValues);
        }

        public IMyOrmService Where<T>(Expression<Func<T,bool>> func)
        {
            return service.Where<T>(func);
        }

        public IMyOrmService Delete<T>()
        {
            return service.Delete<T>();
        }

        public IMyOrmService Update<T>(T obj) where T : new()
        {
            return service.Update<T>(obj);
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


    }
}