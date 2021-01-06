using System.Data;

namespace MyORM
{
    public interface ISQL
    {
        IDbCommand CloneDbCommand();
        IDbConnection CloneDbConnection();
    }
}