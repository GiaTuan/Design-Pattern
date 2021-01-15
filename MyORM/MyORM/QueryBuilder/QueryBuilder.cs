using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace MyORM
{
    public class QueryBuilder : IQueryBuilder
    {
        private IBusinessLogic businessLogic = new BusinessLogic();

        public override void Select<T>(string selectedValues = null)
        {
            string selectQuery = "SELECT " + (selectedValues != null ? selectedValues : "*");
            string tableNameAttribute = businessLogic.GetTableNameAttribute<T>();
            string fromQuery = "FROM " + (tableNameAttribute != null ? tableNameAttribute : typeof(T).Name);
            queryString = selectQuery + " " + fromQuery;
        }

        public override void From<T>()
        {
            string tableNameAttribute = businessLogic.GetTableNameAttribute<T>();
            string fromQuery = "FROM " + (tableNameAttribute != null ? tableNameAttribute : typeof(T).Name);
            queryString = queryString + " " + fromQuery;
        }

        public override void Where<T>(Expression<Func<T, bool>> func)
        {
            string whereQuery = "WHERE " + businessLogic.ConvertLambdaExpressionToQueryString(func);
            queryString = queryString + " " + whereQuery;
        }

        public override void GroupBy(string strGroupBy)
        {
            string groupByQuery = "GROUP BY " + strGroupBy;
            queryString = queryString + " " + groupByQuery;
        }

        public override void Having<T>(Expression<Func<T, bool>> func)
        {
            string havingQuery = "HAVING " + businessLogic.ConvertLambdaExpressionToQueryString(func);
            queryString = queryString + " " + havingQuery;
        }

        public override void Add<T>(T obj)
        {
            List<string> fields = businessLogic.GetFields(obj);
            List<string> values = businessLogic.GetValues(obj);

            //Chuyển fields thành query string
            StringBuilder fieldsQueryString = new StringBuilder();
            foreach (string field in fields)
            {
                fieldsQueryString.Append(field);
                fieldsQueryString.Append(',');
            }
            fieldsQueryString.Remove(fieldsQueryString.Length - 1, 1);


            //Chuyển values thành query string
            StringBuilder valuesQueryString = new StringBuilder();
            foreach (string value in values)
            {
                valuesQueryString.Append(value);
                valuesQueryString.Append(',');
            }
            valuesQueryString.Remove(valuesQueryString.Length - 1, 1);

            string tableNameAttribute = businessLogic.GetTableNameAttribute<T>();

            string addQuery = String.Format($"INSERT INTO {(tableNameAttribute != null ? tableNameAttribute : typeof(T).Name)}({fieldsQueryString}) VALUES({valuesQueryString})");
            queryString = addQuery;
        }

        public override void Delete<T>()
        {
            string tableNameAttribute = businessLogic.GetTableNameAttribute<T>();
            string deleteQuery = "DELETE FROM " + (tableNameAttribute != null ? tableNameAttribute : typeof(T).Name);
            queryString = deleteQuery;
        }

        public override void Update<T>(T obj)
        {
            string identityField = businessLogic.GetIdentityField(obj); //Lấy ID
            int identityValue = businessLogic.GetIdentityValue(obj); //Lấy ID Value

         
            List<string> fields = businessLogic.GetFields(obj);
            List<string> values = businessLogic.GetValues(obj);

            //Chuyển fields và values qua query string
            StringBuilder fields_valuesQueryString = new StringBuilder();

            for (int i = 0; i < fields.Count; i++)
            {
                fields_valuesQueryString.Append(fields[i]);
                fields_valuesQueryString.Append(" = ");
                fields_valuesQueryString.Append(values[i]);
                fields_valuesQueryString.Append(',');
            }
            fields_valuesQueryString.Remove(fields_valuesQueryString.Length - 1, 1);

            string tableNameAttribute = businessLogic.GetTableNameAttribute<T>();

            queryString = String.Format($"UPDATE {(tableNameAttribute != null ? tableNameAttribute : typeof(T).Name)} SET {fields_valuesQueryString} WHERE {identityField} = {identityValue}");
        }



        public override void GroupBy<T>(Expression<Func<T, object>> func)
        {
            string groupByQuery = "GROUP BY " + businessLogic.ConvertLambdaExpressionToQueryString(func);
            queryString = queryString + " " + groupByQuery;
        }


        //
        public override void Join<T>(Expression<Func<T, object>> func)
        {
            string tableNameAttribute = businessLogic.GetTableNameAttribute<T>();
            if(tableNameAttribute == null)
            {
                tableNameAttribute = typeof(T).Name;
            }
            //Console.WriteLine(tableNameAttribute);


            string joinedTablePropertyeName = businessLogic.ConvertLambdaExpressionToQueryString(func);
            //Console.WriteLine(joinedTablePropertyeName);

            string joinedTableNameAttribute = businessLogic.GetJoinedTableNameAttribute<T>(joinedTablePropertyeName);

            if (joinedTableNameAttribute == null)
            {
                joinedTableNameAttribute = Type.GetType(func.Body.Type.ToString()).Name.ToString();
            }
            //Console.WriteLine(joinedTableNameAttribute);


            var joinedTableProperties = Type.GetType(func.Body.Type.ToString()).GetProperties();
            string joinedTableIdentityField = "";

            for (int i = 0; i < joinedTableProperties.Length; i++)
            {
                var attributes = joinedTableProperties[i].GetCustomAttributes(false);  //Lay tat ca cac attributes cua property



                bool isPrimaryProperty = businessLogic.CheckIsPrimaryKey(attributes); // kiem tra xem co phai la primary key hay k?

                if (isPrimaryProperty)
                {
                    string columnNameAttribute = businessLogic.GetColumnNameAttribute(attributes);

                    if (columnNameAttribute != null)
                    {
                        joinedTableIdentityField = columnNameAttribute;
                    }
                    else
                    {
                        joinedTableIdentityField = joinedTableProperties[i].Name;
                    }
                    break;
                }
            }

            //Console.WriteLine(joinedTableIdentityField);


            string oneToOneQuery = "LEFT OUTER JOIN " + joinedTableNameAttribute + ' ';
            oneToOneQuery = oneToOneQuery + "ON " + tableNameAttribute + '.' + joinedTableNameAttribute + " = " + joinedTableNameAttribute + '.' + joinedTableIdentityField;
            queryString = queryString + " " + oneToOneQuery;
        }
    }
}