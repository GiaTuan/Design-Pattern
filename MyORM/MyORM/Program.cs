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
            //proxy.Connect("Server=localhost\\SQLEXPRESS;Database=test;Trusted_Connection=yes;", "SQL Server"); //SQL Server
            //proxy.Connect("Server=localhost;Database=test;Uid=root;Pwd=...;", "MySQL Server");   //MySQL Server
            proxy.Connect("Server=127.0.0.1;Port=5432;Database=test;Username=postgres;Password=...;", "PostgreSQL Server");   //MySQL Server

            proxy.Open();

            //List<Phone> list = proxy.SelectAll<Phone>().ExecuteQuery<Phone>();
            //List<Customer> list2 = proxy.SelectAll<Customer>().ExecuteQuery<Customer>();

            //List<Customer> list3 = proxy.ExecuteQuery<Customer>("select * from customer where id = 1");

            //List<Customer> list4 = proxy.Where<Customer>(u => u.Id == 4);
            List<Customer> list4 = proxy.Where<Customer>(u => u.Name == "tuan" || u.Id == 1);
            Console.Write(list4.Count);


            /*Phone foo = new Phone();

            foo.Other = "abc";

            bool success = proxy.Add<Phone>(foo);
            */

            proxy.Close();
        }
    }
}
