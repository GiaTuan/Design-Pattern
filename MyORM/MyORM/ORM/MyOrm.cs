using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;

namespace MyORM
{
    // Facade
    public class MyOrm : IMyORM 
    {
        private IDbConnection connection;
        private IDbCommand command;
       
        private static Dictionary<string, ISQL> sqlTypes = new Dictionary<string, ISQL>(); // Prototype

        private IQueryBuilder builder = new QueryBuilder(); // Builder
        private IBusinessLogic businessLogic = new BusinessLogic(); // Bridge
        static MyOrm()
        {
            sqlTypes.Add("SQL Server", new SQLServer());
            sqlTypes.Add("MySQL Server", new MySQL());
            sqlTypes.Add("PostgreSQL Server", new PostgeSQL());
        }

        public bool Connect(string connectionString, string databaseType)
        {
            this.connection = sqlTypes[databaseType].CloneDbConnection();
            this.command = sqlTypes[databaseType].CloneDbCommand();
            this.connection.ConnectionString = connectionString;
            return true;
        }

        public void Open()
        {
            connection.Open();
        }

        public IMyORM Select<T>(string selectedValues = null)
        {
            builder.Select<T>(selectedValues);
            return this;
        }

        public IMyORM Where<T>(Expression<Func<T, bool>> func)
        {
            builder.Where<T>(func);
            return this;
        }

        public IMyORM GroupBy(string strGroupBy)
        {
            builder.GroupBy(strGroupBy);
            return this;
        }

        public IMyORM Having<T>(Expression<Func<T, bool>> func)
        {
            builder.Having<T>(func);
            return this;
        }

        public IMyORM Add<T>(T obj) where T : new ()
        {
            builder.Add(obj);
            return this;
        }

        public IMyORM Delete<T>()
        {
            builder.Delete<T>();
            return this;
        }

        public IMyORM Update<T>(T obj) where T : new()
        {
            int identityValue = businessLogic.GetIdentityValue(obj); //Lấy ID Value

            //Thêm mới nếu không có Identity
            if (identityValue == 0)
            {
                return Add(obj);
            }

            builder.Update<T>(obj);
            return this;
        }

        public List<T> ExecuteReader<T>() where T : new()
        {
            string queryString = builder.GetQueryString();
            return ExecuteReader<T>(queryString);   
        }

        public List<T> ExecuteReader<T>(string queryStr) where T : new()
        {
            bool isJoining = false;
            if(queryStr.Contains("LEFT OUTER JOIN"))
            {
                isJoining = true;
            }

            List<T> result = new List<T>();
            try
            {
                command.CommandText = queryStr;
                command.Connection = connection;
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    T obj = new T();
                    if(obj is MyFlexibleObject)
                    {
                        Mapping.MappingDataRowToFlexibleObject(reader,obj as MyFlexibleObject);  
                    }
                    else Mapping.MappingDataRowToObject(reader, obj, isJoining); 

                    result.Add(obj);
                }
                reader.Close();
            }
            catch (Exception)
            {
            }
            return result;
        }

        public bool ExecuteNonQuery(string nonQueryString)
        {
            try
            {
                command.CommandText = nonQueryString;
                command.Connection = connection;
                var reader = command.ExecuteNonQuery();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool ExecuteNonQuery()
        {
            string queryString = builder.GetQueryString();
            return ExecuteNonQuery(queryString);
        }

        public void Close()
        {
            connection.Close();
        }



        public IMyORM GroupBy<T>(Expression<Func<T, object>> func)
        {
            builder.GroupBy<T>(func);
            return this;
        }


        //
        public IMyORM Join<T>(Expression<Func<T, object>> func)
        {
            builder.Join<T>(func);
            return this;
        }
    }
}