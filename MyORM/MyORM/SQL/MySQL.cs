using MySql.Data.MySqlClient;
using System.Data;

namespace MyORM
{
    public class MySQL : ISQL
    {
        public IDbCommand CloneDbCommand()
        {
            return new MySqlCommand();
        }

        public IDbConnection CloneDbConnection()
        {
            return new MySqlConnection();
        }
    }
}