using System.Data;
using System.Data.SqlClient;

namespace MyORM
{
    public class SQLServer : ISQL
    {
        public IDbCommand CloneDbCommand()
        {
            return new SqlCommand();
        }

        public IDbConnection CloneDbConnection()
        {
            return new SqlConnection();
        }
    }
}