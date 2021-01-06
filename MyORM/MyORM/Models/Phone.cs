using MyORM.Attributes;

namespace MyORM.Models
{
    [TableAttribute(TableName = "phone")]
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
