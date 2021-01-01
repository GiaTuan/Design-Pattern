using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;

namespace MyORM
{
    public interface IBusinessLogic
    {
        string GetTableNameAttribute<T>();
        void AddDataToObj<T>(IDataReader reader, T obj);
        List<string> GetFields<T>(T obj);
        string GetColumnNameAttribute(object[] attributes);
        List<string> GetValues<T>(T obj);
        bool CheckIsAutoIncreaseKey(object[] attributes);

        string ConvertLambdaExpressionToQueryString(Expression body);
        string ConvertLambdaExpressionTypeToQueryString(ExpressionType nodeType);

        string GetIdentityColumnNameAttribute(object[] attributes);
        string GetIdentityField<T>(T obj) where T : new();
        int GetIdentityValue<T>(T obj) where T : new();
        void AddDataToFlexibleObj<T>(IDataReader reader, T obj) where T : MyFlexibleObject;
    }
}