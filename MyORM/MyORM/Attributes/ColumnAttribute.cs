using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyORM.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    class ColumnAttribute: Attribute
    {
        public bool IsPrimaryKey { get; set; }
        public string ColumnName { get; set; }
        public bool IsAutoIncreaseKey { get; set; }
    }
}
