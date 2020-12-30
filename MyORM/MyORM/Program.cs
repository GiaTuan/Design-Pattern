using MyORM.Models;
using System;
using System.Collections.Generic;

namespace MyORM
{
    class Program
    {
        static void Main(string[] args)
        {
            MyOrmProxy proxy = new MyOrmProxy();  // proxy pattern?????????
            proxy.Connect("Server=localhost\\SQLEXPRESS;Database=test;Trusted_Connection=yes;", "SQL Server"); //SQL Server
            //proxy.Connect("Server=localhost;Database=test;Uid=root;Pwd=...;", "MySQL Server");   //MySQL Server
            //proxy.Connect("Server=127.0.0.1;Port=5432;Database=test;Username=postgres;Password=...;", "PostgreSQL Server");   //MySQL Server

            proxy.Open();
            //Insert One
            //proxy.Add(new Customer() { Name = "nntung", Age = 22, Gender = "male" });
            //proxy.SaveChange();



            //Update One
            //proxy.Update(new Customer() { Id = 23, Name = "hellllllllllllllo", Gender = "male", Age = 23 });
            //proxy.SaveChange();


            //Where
            List<Customer> customers = proxy.Where<Customer>(u => u.Name == "tuan" || u.Id == 1);
            Console.Write(customers.Count);
            proxy.Close();
        }
    }
}
