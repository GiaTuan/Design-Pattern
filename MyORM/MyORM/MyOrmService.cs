using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using MyORM.Models;
using System.Diagnostics;
using MyORM.Attributes;
using System.Linq.Expressions;

namespace MyORM
{
    public class MyOrmService : IMyOrmService
    {
        private SqlConnection connection;
        private List<object> list = new List<object>();
        private IBusinessLogic businessLogic = new BusinessLogic(); //bridge pattern?????????

        private string queryString = null;
        public void Open()
        {
            connection.Open();
        }

        public bool Add<T>(T obj) where T : new ()
        {
            string fields = businessLogic.GetFields(obj);
            string values = businessLogic.GetValues(obj);

            string tableNameAttribute = businessLogic.GetTableNameAttribute<T>();
            try
            {
                queryString = String.Format($"INSERT INTO {(tableNameAttribute != null ? tableNameAttribute : typeof(T).Name)}({fields}) VALUES({values})");
                var command = new SqlCommand(queryString, connection);
                var reader = command.ExecuteNonQuery();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }


        public bool Connect(string connectionString)
        {
            connection = new SqlConnection
            {
                ConnectionString = connectionString
            };
            return true;
        }


        public IMyOrmService SelectAll<T>()
        {
            string tableNameAttribute = businessLogic.GetTableNameAttribute<T>();

            queryString = "SELECT * FROM " + (tableNameAttribute != null ? tableNameAttribute : typeof(T).Name);
            
            return this;
        }

      
        public List<T> ExecuteQuery<T>() where T : new()
        {
            List<T> result = new List<T>();
            result = ExecuteQuery<T>(queryString);   
            return result;
        }

        public List<T> ExecuteQuery<T>(string queryStr) where T : new()
        {
            List<T> result = new List<T>();
            try
            {
                var command = new SqlCommand(queryStr, connection);
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    T obj = new T();
                    businessLogic.AddDataToObj(reader, obj);
                    result.Add(obj);
                }
                reader.Close();
            }
            catch (Exception)
            {
            }
            return result;
        }

        public void Close()
        {
            connection.Close();
        }
    }
}