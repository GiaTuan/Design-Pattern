using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using MySql.Data.MySqlClient;
using Npgsql;
using System.Text;

namespace MyORM
{
    public class MyOrmService : IMyOrmService
    {
        private IDbConnection connection;
        private IDbCommand command;
        private List<object> list = new List<object>();
        private IBusinessLogic businessLogic = new BusinessLogic(); //bridge pattern?????????

        private static Dictionary<string, IDbConnection> dbconnections = new Dictionary<string, IDbConnection>();
        private static Dictionary<string, IDbCommand> dbcommands = new Dictionary<string, IDbCommand>();


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

        private string queryString = null;


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

        public bool Add<T>(T obj) where T : new()
        {
            List<string> fields = businessLogic.GetFields(obj);
            List<string> values = businessLogic.GetValues(obj);


            //Chuyển fields thành query string
            StringBuilder fieldQueryString = new StringBuilder();
            foreach(string field in fields)
            {
                fieldQueryString.Append(field);
                fieldQueryString.Append(',');
            }
            fieldQueryString.Remove(fieldQueryString.Length - 1, 1);


            //Chuyển values thành query string
            StringBuilder valueQueryString = new StringBuilder();
            foreach (string value in values)
            {
                valueQueryString.Append(value);
                valueQueryString.Append(',');
            }
            valueQueryString.Remove(valueQueryString.Length - 1, 1);


            string tableNameAttribute = businessLogic.GetTableNameAttribute<T>();
            try
            {
                queryString = String.Format($"INSERT INTO {(tableNameAttribute != null ? tableNameAttribute : typeof(T).Name)}({fieldQueryString}) VALUES({valueQueryString})");
                //var command = new SqlCommand(queryString, connection);
                command.CommandText = queryString;
                command.Connection = connection;
                //var reader = command.ExecuteNonQuery();
            }
            catch (Exception)
            {
                return false;
            }
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
                //var command = new SqlCommand(queryStr, connection);
                command.CommandText = queryStr;
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

        public void Close()
        {
            connection.Close();
        }

        /////////////////////////////////////////////////////////////////////////
        public List<T> Where<T>(string where) where T : new()
        {
            string tableNameAttribute = businessLogic.GetTableNameAttribute<T>();

            List<T> result = new List<T>();
            try
            {
                queryString = String.Format($"SELECT * FROM " + (tableNameAttribute != null ? tableNameAttribute : typeof(T).Name) + " WHERE " + where.ToString());
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
                return null;
            }
            return result;
        }


        public bool Update<T>(T obj) where T : new()
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

            //Chuyển field và value qua query string
            StringBuilder field_valueQueryString = new StringBuilder();

            for (int i = 0; i < fields.Count; i++)
            {
                field_valueQueryString = field_valueQueryString.Append(fields[i]);
                field_valueQueryString = field_valueQueryString.Append(" = ");
                field_valueQueryString = field_valueQueryString.Append(values[i]);
                field_valueQueryString = field_valueQueryString.Append(',');
            }
            field_valueQueryString.Remove(field_valueQueryString.Length - 1, 1);

            string tableNameAttribute = businessLogic.GetTableNameAttribute<T>();
            try
            {
                queryString = String.Format($"UPDATE {(tableNameAttribute != null ? tableNameAttribute : typeof(T).Name)} SET {field_valueQueryString} WHERE {identityField} = {identityValue}");
                command.CommandText = queryString;
                command.Connection = connection;
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }


        public int SaveChange()
        {
            var reader = command.ExecuteNonQuery();
            if (reader == 1)
            {
                return 1;
            }
            return 0;
        }
    }
}
