using MyORM.Attributes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

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

        public void AddDataToObj<T>(IDataReader reader, T obj) where T : new()
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

        public List<string> GetFields<T>(T obj) where T : new()
        {
            List<string> fields = new List<string>();
            var properties = obj.GetType().GetProperties();
            for (int i = 0; i < properties.Length; i++)
            {
                if (properties[i].GetValue(obj, null) == null) continue;

                var attributes = properties[i].GetCustomAttributes(false);  //lay tat ca cac attributes cua property

                bool isAutoIncreaseProperty = CheckIsAutoIncreaseKey(attributes); // kiem tra xem co phai la key auto increase hay k?
                if (isAutoIncreaseProperty) continue; // neu la key tu tang thi khong them value cua no vao string value

                string columnNameAttribute = GetColumnNameAttribute(attributes);

                if (columnNameAttribute != null) fields.Add(columnNameAttribute);
                else fields.Add(properties[i].Name);
            }

            return fields;
        }

        public List<string> GetValues<T>(T obj) where T : new()
        {
            List<string> values = new List<string>();  //ket qua values cho insert => VD values = abc,xyz,1
            var properties = obj.GetType().GetProperties();  //lay tat ca cac propety cua doi tuong hien tai

            for (int i = 0; i < properties.Length; i++)
            {
                if (properties[i].GetValue(obj, null) == null) continue;

                var attributes = properties[i].GetCustomAttributes(false);  //lay tat ca cac attributes cua property

                bool isAutoIncreaseProperty = CheckIsAutoIncreaseKey(attributes); // kiem tra xem co phai la key auto increase hay k?

                if (isAutoIncreaseProperty) continue; // neu la key tu tang thi khong them value cua no vao string value

                if (properties[i].GetValue(obj, null).GetType() == typeof(string))
                {
                    StringBuilder value = new StringBuilder();
                    value.Append("'");
                    value.Append(properties[i].GetValue(obj, null));
                    value.Append("'");

                    values.Add(value.ToString());
                }
                else values.Add(properties[i].GetValue(obj, null).ToString());
            }

            return values;
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



        public string GetIdentityColumnNameAttribute(object[] attributes)
        {
            foreach (var attribute in attributes)
            {
                if (((ColumnAttribute)attribute).IsPrimaryKey != null)
                {
                    return ((ColumnAttribute)attribute).ColumnName;
                }
            }
            return null;
        }

        public string GetIdentityField<T>(T obj) where T : new()
        {
            var properties = obj.GetType().GetProperties();
            for (int i = 0; i < properties.Length; i++)
            {
                if (properties[i].GetValue(obj, null) == null) continue;

                var attributes = properties[i].GetCustomAttributes(false);  //Lay tat ca cac attributes cua property

                string identityColumnNameAttribute = GetIdentityColumnNameAttribute(attributes);

                if (identityColumnNameAttribute != null)
                {
                    return identityColumnNameAttribute;
                }
            }

            return "";
        }

        public int GetIdentityValue<T>(T obj) where T : new()
        {
            var properties = obj.GetType().GetProperties();  //Lay tat ca cac propety cua doi tuong hien tai

            for (int i = 0; i < properties.Length; i++)
            {
                if (properties[i].GetValue(obj, null) == null) continue;

                var attributes = properties[i].GetCustomAttributes(false);  //Lay tat ca cac attributes cua property


                string identityColumnNameAttribute = GetIdentityColumnNameAttribute(attributes);

                if (identityColumnNameAttribute != null)
                {
                    return int.Parse(properties[i].GetValue(obj, null).ToString());
                }
            }
            return -1;
        }
    }
}
