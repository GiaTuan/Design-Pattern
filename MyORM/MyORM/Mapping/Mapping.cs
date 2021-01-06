using System;
using System.Data;

namespace MyORM
{
    public class Mapping
    {
        public static void MappingDataRowToObject<T>(IDataReader reader, T obj)
        {
            var properties = obj.GetType().GetProperties();

            int index = 0;

            foreach (var property in properties)
            {
                if (reader[index] != DBNull.Value)
                    property.SetValue(obj, reader[index++]);
            }
        }

        public static void MappingDataRowToFlexibleObject<T>(IDataReader reader, T obj) where T : MyFlexibleObject
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                string fieldName = reader.GetName(i);
                obj[fieldName] = reader[i];
            }
        }
    }
}