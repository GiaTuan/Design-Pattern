using System.Collections.Generic;
using System.Data;


namespace MyORM
{
    public interface IBusinessLogic
    {
        string GetTableNameAttribute<T>();
        void AddDataToObj<T>(IDataReader reader, T obj) where T : new();
        List<string> GetFields<T>(T obj) where T : new();
        string GetColumnNameAttribute(object[] attributes);
        List<string> GetValues<T>(T obj) where T : new();
        bool CheckIsAutoIncreaseKey(object[] attributes);


        string GetIdentityColumnNameAttribute(object[] attributes);
        string GetIdentityField<T>(T obj) where T : new();
        int GetIdentityValue<T>(T obj) where T : new();
    }
}
