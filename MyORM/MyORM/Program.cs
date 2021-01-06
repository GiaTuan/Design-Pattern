using MyORM.Models;
using MyORM.ORM;
using System.Collections.Generic;

namespace MyORM
{
    class Program
    {
        static void Main(string[] args)
        {
            /*MyOrmProxy proxy = new MyOrmProxy();  // proxy pattern?????????
            proxy.Connect("Server=localhost\\SQLEXPRESS;Database=test;Trusted_Connection=yes;", "SQL Server"); //SQL Server
            //proxy.Connect("Server=localhost;Database=test;Uid=root;Pwd=...;", "MySQL Server");   //MySQL Server
            //proxy.Connect("Server=127.0.0.1;Port=5432;Database=test;Username=postgres;Password=...;", "PostgreSQL Server");   //MySQL Server
            proxy.Open();

            List<MyFlexibleObject> list = proxy.Select<Customer>("id,count(*)").GroupBy("id").Having<Customer>(c => c.Id == 1).ExecuteReader<MyFlexibleObject>();

            //Add
            proxy.Add(new Customer() { Name = "ntt", Age = 22, Gender = "male" }).ExecuteNonQuery();

            //Select

            //Where
            List<Customer> customers = proxy.Where<Customer>(c => c.Name == "ntt" || c.Age >= 22).ExecuteReader<Customer>();
            Console.WriteLine(customers.Count);

            //Delete

            //Update
            proxy.Update(new Customer() { Id = 1, Name = "Nguyen Thanh Tung", Age = 22 }).ExecuteNonQuery();

            proxy.Close();*/

            MyORMProxy proxy = new MyORMProxy();
            proxy.Connect("Server=localhost\\SQLEXPRESS;Database=test;Trusted_Connection=yes;", "SQL Server");
            proxy.Open();
            List<MyFlexibleObject> list = proxy.Select<Customer>("name").ExecuteReader<MyFlexibleObject>();
            proxy.Close();
        }
    }
}
