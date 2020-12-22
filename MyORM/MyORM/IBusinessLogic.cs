using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MyORM
{
    public interface IBusinessLogic
    {
        string GetTableNameAttribute<T>();
        void AddDataToObj<T>(SqlDataReader reader, T obj) where T : new();
        string GetFields<T>(T obj) where T : new();
        string GetColumnNameAttribute(object[] attributes);
        string GetValues<T>(T obj) where T : new();
        bool CheckIsAutoIncreaseKey(object[] attributes);
    }
}