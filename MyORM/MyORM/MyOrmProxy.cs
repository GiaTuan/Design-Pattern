using MyORM.Attributes;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace MyORM
{
    public class MyOrmProxy : IMyOrmService
    {
        private MyOrmService service;
        private IBusinessLogic businessLogic = new BusinessLogic(); //bridge pattern?????????

        public bool Connect(string connectionString, string databaseType)
        {
            if (service == null)
            {
                service = new MyOrmService();
                return service.Connect(connectionString, databaseType);
            }
            else return false;
        }

        public void Open()
        {
            service.Open();
        }
        public bool Add<T>(T obj) where T : new()
        {
            bool result = service.Add<T>(obj);
            return result;
        }

        public IMyOrmService SelectAll<T>()
        {
            return service.SelectAll<T>();
        }


        public List<T> ExecuteQuery<T>() where T : new()
        {
            return service.ExecuteQuery<T>();
        }

        public List<T> ExecuteQuery<T>(string queryString) where T : new()
        {
            return service.ExecuteQuery<T>(queryString);
        }

        public void Close()
        {
            service.Close();
        }

        ////////////////////////////////////////
        public List<T> Where<T>(Expression<Func<T, bool>> where) where T : new()
        {
            //Console.WriteLine(typeof(T).Name.GetType().GetProperties().Length);
            Console.WriteLine(ConvertLambdaExpressionToQueryString(where));
            return service.Where<T>(ConvertLambdaExpressionToQueryString(where));
        }


        private static string ConvertLambdaExpressionToQueryString(Expression body)
        {
            Console.WriteLine(body.GetType().BaseType);
            if (body == null)
            {
                return string.Empty;
            }
            if(body is ConstantExpression)
            {
                try
                {
                    if((((ConstantExpression)body).Value) == null){
                        return "NULL";
                    }
                    else if (((ConstantExpression)body).Value.GetType()  == typeof(string))
                    {
                        return '\'' + (((ConstantExpression)body).Value).ToString() + '\'';
                    }
                    return (((ConstantExpression)body).Value).ToString();
                }
                catch(Exception)
                {
                    return "NULL";
                }
            }
            if(body is MemberExpression)
            {
                //Chuyen doi sang columnname (neu co)
                var properties = ((MemberExpression)body).Expression.Type.GetProperties();
                for (int i = 0; i < properties.Length; i++)
                {
                    if(properties[i].Name == ((MemberExpression)body).Member.Name)
                    {
                        var attributes = properties[i].GetCustomAttributes(false);  //lay tat ca cac attributes cua property


                        foreach (var attribute in attributes)
                        {
                            if (((ColumnAttribute)attribute).ColumnName != null)
                            {
                                return ((ColumnAttribute)attribute).ColumnName;
                            }
                        }
                    }
                }
                return ((MemberExpression)body).Member.Name;
            }
            if(body is BinaryExpression)
            {
                return ConvertLambdaExpressionToQueryString(((BinaryExpression)body).Left) 
                    + ConvertLambdaExpressionTypeToQueryString(((BinaryExpression)body).NodeType) 
                    + ConvertLambdaExpressionToQueryString(((BinaryExpression)body).Right);
            }
            if(body is LambdaExpression)
            {
                return ConvertLambdaExpressionToQueryString(((LambdaExpression)body).Body);
            }
            return "";
        }


        //Chuyen LambdaExpression sang Query String
        private static string ConvertLambdaExpressionTypeToQueryString(ExpressionType nodeType)
        {
            if (nodeType.Equals(ExpressionType.AndAlso))
            {
                return " AND ";
            }
            else if (nodeType.Equals(ExpressionType.And))
            {
                return " AND ";
            }
            else if (nodeType.Equals(ExpressionType.Or))
            {
                return " OR ";
            }
            else if (nodeType.Equals(ExpressionType.OrElse))
            {
                return " OR ";
            }
            else if (nodeType.Equals(ExpressionType.GreaterThan))
            {
                return " > ";
            }
            else if (nodeType.Equals(ExpressionType.GreaterThanOrEqual))
            {
                return " >= ";
            }
            else if (nodeType.Equals(ExpressionType.LessThan))
            {
                return " < ";
            }
            else if (nodeType.Equals(ExpressionType.LessThanOrEqual))
            {
                return " < ";
            }
            else if (nodeType.Equals(ExpressionType.Equal))
            {
                return " = ";
            }
            else if (nodeType.Equals(ExpressionType.NotEqual))
            {
                return " != ";
            }
            return "";
        }



        public bool Update<T>(T obj) where T : new()
        {
            bool result = service.Update<T>(obj);
            return result;
        }

        public int SaveChange()
        {
            return service.SaveChange();
        }
    }
}
