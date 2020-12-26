using System.Data;


namespace MyORM
{
    public interface IBusinessLogic
    {
        string GetTableNameAttribute<T>();
        void AddDataToObj<T>(IDataReader reader, T obj) where T : new();
        string GetFields<T>(T obj) where T : new();
        string GetColumnNameAttribute(object[] attributes);
        string GetValues<T>(T obj) where T : new();
        bool CheckIsAutoIncreaseKey(object[] attributes);
    }
}
