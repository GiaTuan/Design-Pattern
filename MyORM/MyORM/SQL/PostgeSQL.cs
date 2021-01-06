using Npgsql;
using System.Data;


namespace MyORM
{
    public class PostgeSQL : ISQL
    {
        public IDbCommand CloneDbCommand()
        {
            return new NpgsqlCommand();
        }

        public IDbConnection CloneDbConnection()
        {
            return new NpgsqlConnection();
        }
    }
}