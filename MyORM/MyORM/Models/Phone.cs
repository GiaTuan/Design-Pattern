using MyORM.Attributes;

namespace MyORM.Models
{
    //
    [TableAttribute(TableName = "phone")]
    class Phone
    {
        [ColumnAttribute(ColumnName = "phone_id")]
        [ColumnAttribute(IsPrimaryKey = true)]
        [ColumnAttribute(IsAutoIncreaseKey = true)]
        public int Id { get; set; }

        [ColumnAttribute(ColumnName = "phone_number")]
        public string PhoneNumber { get; set; }

        [ColumnAttribute(ColumnName = "company")]
        public string Company { get; set; }
    }
}
