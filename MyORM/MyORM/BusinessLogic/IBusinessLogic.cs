using System.Collections.Generic;
using System.Linq.Expressions;

namespace MyORM
{
    public interface IBusinessLogic
    {
        string GetTableNameAttribute<T>();
        List<string> GetFields<T>(T obj);
        string GetColumnNameAttribute(object[] attributes);
        List<string> GetValues<T>(T obj);
        bool CheckIsAutoIncreaseKey(object[] attributes);
        string ConvertLambdaExpressionToQueryString(Expression body);
        string ConvertLambdaExpressionTypeToQueryString(ExpressionType nodeType);
        string GetIdentityField<T>(T obj) where T : new();
        int GetIdentityValue<T>(T obj) where T : new();


        bool CheckIsPrimaryKey(object[] attributes);
    }
}