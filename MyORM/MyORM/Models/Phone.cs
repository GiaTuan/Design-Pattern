using MyORM.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyORM.Models
{
    class Phone
    {
        [ColumnAttribute(IsPrimaryKey = true, IsAutoIncreaseKey = true)]
        public int Id { get; set; }

        [ColumnAttribute(ColumnName = "phone")]
        public string PhoneNumber { get; set; }

        public string Other { get; set; }
    }
}
