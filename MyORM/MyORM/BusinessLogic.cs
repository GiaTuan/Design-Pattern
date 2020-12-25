using MyORM.Attributes;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyORM
{
    public class BusinessLogic : IBusinessLogic
    {
        public string GetTableNameAttribute<T>()
        {
            var attributes = typeof(T).GetCustomAttributes(typeof(TableAttribute), true);
            if(attributes.Length == 0)
            {
                return null;
            }
            else
            {
                string tableAttributeName = ((TableAttribute)attributes[0]).TableName;
                return tableAttributeName;
            }
        }

        public void AddDataToObj<T>(SqlDataReader reader, T obj) where T : new()
        {
            var properties = obj.GetType().GetProperties();

            int index = 0;

            foreach(var property in properties)
            {
                if(reader[index] != DBNull.Value)
                {
                    property.SetValue(obj, reader[index++]);
                }
            }
        }

        public string GetColumnNameAttribute(object[] attributes)
        {
            foreach(var attribute in attributes)
            {
                if(((ColumnAttribute)attribute).ColumnName != null)
                {
                    return ((ColumnAttribute)attribute).ColumnName;
                }
            }
            return null;
        }

        public string GetFields<T>(T obj) where T : new()
        {
            StringBuilder fields = new StringBuilder();
            var properties = obj.GetType().GetProperties();
            for (int i = 0; i < properties.Length; i++)
            {
                if (properties[i].GetValue(obj, null) == null) continue;

                var attributes = properties[i].GetCustomAttributes(false);  //lay tat ca cac attributes cua property

                bool isAutoIncreaseProperty = CheckIsAutoIncreaseKey(attributes); // kiem tra xem co phai la key auto increase hay k?
                if (isAutoIncreaseProperty) continue; // neu la key tu tang thi khong them value cua no vao string value

                string columnNameAttribute = GetColumnNameAttribute(attributes);

                if (columnNameAttribute != null) fields.Append(columnNameAttribute);
                else fields.Append(properties[i].Name);

                if (i < properties.Length - 1)
                {
                    fields.Append(" ");
                }
            }

            return fields.ToString().Trim().Replace(' ', ',');
        }

        public string GetValues<T>(T obj) where T : new()
        {
            StringBuilder values = new StringBuilder();  //ket qua values cho insert => VD values = abc,xyz,1
            var properties = obj.GetType().GetProperties();  //lay tat ca cac propety cua doi tuong hien tai

            for (int i = 0; i < properties.Length; i++)
            {
                if (properties[i].GetValue(obj, null) == null) continue;

                var attributes = properties[i].GetCustomAttributes(false);  //lay tat ca cac attributes cua property

                bool isAutoIncreaseProperty = CheckIsAutoIncreaseKey(attributes); // kiem tra xem co phai la key auto increase hay k?

                if (isAutoIncreaseProperty) continue; // neu la key tu tang thi khong them value cua no vao string value

                if (properties[i].GetValue(obj, null).GetType() == typeof(string))
                {
                    values.Append("'");
                    values.Append(properties[i].GetValue(obj, null));
                    values.Append("'");
                }
                else values.Append(properties[i].GetValue(obj, null));

                if (i < properties.Length - 1)
                {
                    values.Append(" ");
                }
            }

            return values.ToString().Trim().Replace(' ', ',');
        }

        public bool CheckIsAutoIncreaseKey(object[] attributes)
        {
            foreach (var attribute in attributes)
            {
                if (((ColumnAttribute)attribute).IsAutoIncreaseKey == true)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
