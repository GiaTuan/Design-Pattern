using MyORM.Attributes;
using MyORM.Models;
using System;
using System.Data;

namespace MyORM
{
    public class Mapping
    {
        //public static void MappingDataRowToObject<T>(IDataReader reader, T obj)
        //{
        //    var properties = obj.GetType().GetProperties();

        //    int index = 0;

        //    foreach (var property in properties)
        //    {
        //        if (reader[index] != DBNull.Value)
        //            property.SetValue(obj, reader[index++]);
        //    }
        //}

        public static void MappingDataRowToFlexibleObject<T>(IDataReader reader, T obj) where T : MyFlexibleObject
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                string fieldName = reader.GetName(i);
                obj[fieldName] = reader[i];
            }
        }


        //
        public static void MappingDataRowToObject<T>(IDataReader reader, T obj, bool isJoining)
        {
            var properties = obj.GetType().GetProperties();

            int index = 0;

            foreach (var property in properties)
            {
                var attributes = property.GetCustomAttributes(false);  //Lay tat ca cac attributes cua property



                //string columnNameAttribute = GetColumnNameAttribute(attributes);
                //if (columnNameAttribute == null)
                //{
                //    columnNameAttribute = property.Name;
                //}

                string oneToOneColumnNameAttribute = GetOneToOneColumnNameAttribute(attributes);


                if (oneToOneColumnNameAttribute != null)
                {
                    if(isJoining == false)
                    {
                        break;
                    }

                    index++;
                    Type joinedPropertyType = property.PropertyType;
                    object joinedObject = Activator.CreateInstance(joinedPropertyType);

                    var joinedTableProperties = joinedObject.GetType().GetProperties();

                    foreach (var property2 in joinedTableProperties)
                    {
                        var attributes2 = property2.GetCustomAttributes(false);  //Lay tat ca cac attributes cua property



                        //string columnNameAttribute2 = GetColumnNameAttribute(attributes2);
                        //if (columnNameAttribute2 == null)
                        //{
                        //    columnNameAttribute2 = property2.Name;
                        //}

                        if (reader[index] != DBNull.Value)  // && reader.GetName(index) == columnNameAttribute2
                            property2.SetValue(joinedObject, reader[index]);
                        index++;

                    }

                    property.SetValue(obj, joinedObject);
                }
                else
                {
                    if (reader[index] != DBNull.Value)  // && reader.GetName(index) == columnNameAttribute
                        property.SetValue(obj, reader[index]);
                    index++;
                }

            }
        }

        private static string GetOneToOneColumnNameAttribute(object[] attributes)
        {
            foreach (var attribute in attributes)
            {
                if (((ColumnAttribute)attribute).OneToOne != null)
                {
                    return ((ColumnAttribute)attribute).OneToOne;
                }
            }
            return null;
        }


        //private static string GetColumnNameAttribute(object[] attributes)
        //{
        //    foreach (var attribute in attributes)
        //    {
        //        if (((ColumnAttribute)attribute).ColumnName != null)
        //        {
        //            return ((ColumnAttribute)attribute).ColumnName;
        //        }
        //    }
        //    return null;
        //}
    }
}