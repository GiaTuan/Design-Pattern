using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MyORM
{
    public class MyOrmProxy : IMyOrmService
    {
        MyOrmService service;

        public bool Connect(string connectionString)
        {
            if (service == null)
            {
                service = new MyOrmService();
                return service.Connect(connectionString);
            }
            else return false;
        }

        public void Open()
        {
            service.Open();
        }
        public bool Add<T>(T obj) where T : new ()
        {
            bool result = service.Add<T>(obj);
            return result;
        }

        public IMyOrmService SelectAll<T>()
        {
            return service.SelectAll<T>();
        }

       
        public List<T> ExecuteQuery<T>() where T : new()
        {
            return service.ExecuteQuery<T>();
        }

        public List<T> ExecuteQuery<T>(string queryString) where T : new()
        {
            return service.ExecuteQuery<T>(queryString);
        }

        public void Close()
        {
            service.Close();
        }
    }
}