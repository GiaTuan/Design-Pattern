using MyORM.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyORM.Models
{
    [TableAttribute(TableName = "dienthoai")]
    class Phone
    {
        [ColumnAttribute(IsPrimaryKey = true, IsAutoIncreaseKey = true)]
        public int Id { get; set; }

        [ColumnAttribute(ColumnName = "phone")]
        public string PhoneNumber { get; set; }

        [ColumnAttribute(ColumnName = "idcustomer")]
        public int IdCustomer { get; set; }


    }
}
