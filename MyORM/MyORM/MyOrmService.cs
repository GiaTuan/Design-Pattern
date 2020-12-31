using MySql.Data.MySqlClient;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Text;

namespace MyORM
{
    public class MyOrmService : IMyOrmService
    {
        private IDbConnection connection;
        private IDbCommand command;
        private IBusinessLogic businessLogic = new BusinessLogic(); //bridge pattern?????????

        private static Dictionary<string, IDbConnection> dbconnections = new Dictionary<string, IDbConnection>();
        private static Dictionary<string, IDbCommand> dbcommands = new Dictionary<string, IDbCommand>();


        private string queryString = null;
        //private string whereQueryString = null;


        //Prototype pattern ??
        static MyOrmService()
        {
            dbconnections.Add("SQL Server", new SqlConnection());
            dbconnections.Add("MySQL Server", new MySqlConnection());
            dbconnections.Add("PostgreSQL Server", new NpgsqlConnection());

            dbcommands.Add("SQL Server", new SqlCommand());
            dbcommands.Add("MySQL Server", new MySqlCommand());
            dbcommands.Add("PostgreSQL Server", new NpgsqlCommand());
        }

        public bool Connect(string connectionString, string databaseType)
        {
            this.connection = dbconnections[databaseType];
            this.connection.ConnectionString = connectionString;

            this.command = dbcommands[databaseType];
            return true;
        }

        public void Open()
        {
            connection.Open();
        }

        public IMyOrmService Select<T>(string selectedValues = null)
        {
            string tableNameAttribute = businessLogic.GetTableNameAttribute<T>();
            queryString = String.Format("SELECT {0} FROM {1}", selectedValues != null ? selectedValues : "*", tableNameAttribute != null ? tableNameAttribute : typeof(T).Name);
            return this;

        }


        public IMyOrmService Where<T>(Expression<Func<T, bool>> func)
        {
            string whereQueryString = businessLogic.ConvertLambdaExpressionToQueryString(func);
            string tableNameAttribute = businessLogic.GetTableNameAttribute<T>();

            queryString = String.Format($"SELECT * FROM " + (tableNameAttribute != null ? tableNameAttribute : typeof(T).Name) + " WHERE " + whereQueryString);

            return this;
        }


        public IMyOrmService Add<T>(T obj) where T : new ()
        {
            List<string> fields = businessLogic.GetFields(obj);
            List<string> values = businessLogic.GetValues(obj);

            //Chuyển fields thành query string
            StringBuilder fieldsQueryString = new StringBuilder();
            foreach (string field in fields)
            {
                fieldsQueryString.Append(field);
                fieldsQueryString.Append(',');
            }
            fieldsQueryString.Remove(fieldsQueryString.Length - 1, 1);


            //Chuyển values thành query string
            StringBuilder valuesQueryString = new StringBuilder();
            foreach (string value in values)
            {
                valuesQueryString.Append(value);
                valuesQueryString.Append(',');
            }
            valuesQueryString.Remove(valuesQueryString.Length - 1, 1);

            string tableNameAttribute = businessLogic.GetTableNameAttribute<T>();

            queryString = String.Format($"INSERT INTO {(tableNameAttribute != null ? tableNameAttribute : typeof(T).Name)}({fieldsQueryString}) VALUES({valuesQueryString})");
            return this;
        }

        public IMyOrmService Delete<T>()
        {
            string tableNameAttribute = businessLogic.GetTableNameAttribute<T>();
            queryString = "DELETE FROM " + (tableNameAttribute != null ? tableNameAttribute : typeof(T).Name);
            return this;
        }

        public IMyOrmService Update<T>(T obj) where T : new()
        {
            string identityField = businessLogic.GetIdentityField(obj); //Lấy ID
            int identityValue = businessLogic.GetIdentityValue(obj); //Lấy ID Value

            //Thêm mới nếu không có Identity
            if (identityValue == -1)
            {
                return Add(obj);
            }

            List<string> fields = businessLogic.GetFields(obj);
            List<string> values = businessLogic.GetValues(obj);

            //Chuyển fields và values qua query string
            StringBuilder fields_valuesQueryString = new StringBuilder();

            for (int i = 0; i < fields.Count; i++)
            {
                fields_valuesQueryString.Append(fields[i]);
                fields_valuesQueryString.Append(" = ");
                fields_valuesQueryString.Append(values[i]);
                fields_valuesQueryString.Append(',');
            }
            fields_valuesQueryString.Remove(fields_valuesQueryString.Length - 1, 1);

            string tableNameAttribute = businessLogic.GetTableNameAttribute<T>();

            queryString = String.Format($"UPDATE {(tableNameAttribute != null ? tableNameAttribute : typeof(T).Name)} SET {fields_valuesQueryString} WHERE {identityField} = {identityValue}");
            return this;
        }


        public List<T> ExecuteReader<T>() where T : new()
        {
            return ExecuteReader<T>(queryString);   
        }

        public List<T> ExecuteReader<T>(string queryStr) where T : new()
        {
            List<T> result = new List<T>();
            try
            {
                command.CommandText = queryString;
                command.Connection = connection;
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

        public bool ExecuteNonQuery(string queryString)
        {
            try
            {
                command.CommandText = queryString;
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
            return ExecuteNonQuery(queryString);
        }
        public void Close()
        {
            connection.Close();
        }

    }
}