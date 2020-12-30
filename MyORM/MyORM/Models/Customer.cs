using MyORM.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyORM.Models
{
    [TableAttribute(TableName = "customer")]
    class Customer
    {
        [ColumnAttribute(ColumnName = "id")]
        [ColumnAttribute(IsPrimaryKey = true)]
        [ColumnAttribute(IsAutoIncreaseKey = true)]
        public int Id { get; set; }


        [ColumnAttribute(ColumnName = "name")]
        public string Name { get; set; }

        [ColumnAttribute(ColumnName = "gender")]
        public string Gender { get; set; }

        [ColumnAttribute(ColumnName = "age")]
        public int Age { get; set; }
    }
}
