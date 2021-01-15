using MyORM.Attributes;

namespace MyORM.Models
{
    [TableAttribute(TableName = "customer")]
    class Customer
    {
        [ColumnAttribute(ColumnName = "customer_id")]
        [ColumnAttribute(IsPrimaryKey = true)]
        [ColumnAttribute(IsAutoIncreaseKey = true)]
        public int Id { get; set; }


        [ColumnAttribute(ColumnName = "name")]
        public string Name { get; set; }


        [ColumnAttribute(ColumnName = "gender")]
        public string Gender { get; set; }

        [ColumnAttribute(ColumnName = "age")]
        public int Age { get; set; }


        //
        [ColumnAttribute(OneToOne = "phone")]
        public Phone phone { get; set; }
    }
}
