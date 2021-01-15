using MyORM.Models;
using MyORM.ORM;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyORM
{
    class Program
    {
        static void Main(string[] args)
        {
            MyORMProxy proxy = new MyORMProxy();    //Proxy Pattern
            proxy.Connect("Server=localhost\\SQLEXPRESS;Database=StoreManagementDB;Trusted_Connection=yes;", "SQL Server");
            //proxy.Connect("Server=localhost;Database=test;Uid=root;Pwd=...;", "MySQL Server");   //MySQL Server
            //proxy.Connect("Server=127.0.0.1;Port=5432;Database=test;Username=postgres;Password=...;", "PostgreSQL Server");   //PostgreSQL Server

            proxy.Open();


            //Select
            //List<Customer> customers = proxy.Select<Customer>().ExecuteReader<Customer>();


            //Select with Join (OneToOne)
            //List<Customer> customers = proxy.Select<Customer>().Join<Customer>(c => c.phone).ExecuteReader<Customer>();


            //Where
            //List<Customer> customers = proxy.Select<Customer>().Where<Customer>(c => c.Name == "NTT" || c.Age >= 23).ExecuteReader<Customer>();


            //Where with Join (OneToOne)
            //List<Customer> customers = proxy.Select<Customer>().Join<Customer>(c => c.phone).Where<Customer>(c => c.Name == "NTT" || c.Age >= 23).ExecuteReader<Customer>();



            //Add
            //Customer customer = new Customer()
            //{
            //    Name = "NTT",
            //    Gender = "male",
            //    Age = 22
            //};
            //proxy.Add(customer).ExecuteNonQuery();


            //Update
            //proxy.Update(new Customer() { Id = 11, Name = "Nguyen Thanh Tung", Age = 22 }).ExecuteNonQuery();



            //GroupBy + Having
            List<MyFlexibleObject> customers = proxy.Select<Customer>("age").Join<Customer>(c => c.phone).Where<Customer>(c => c.Age < 30).GroupBy<Customer>(c => new { c.Name, c.Age }).Having<Customer>(c => c.Age == 22).ExecuteReader<MyFlexibleObject>();

            //Delete


            Console.WriteLine("Count: " + customers.Count);
            //for (int i = 0; i < customers.Count; i++)
            //{
            //    Console.WriteLine("Customer ID: " + customers[i].Id);
            //    Console.WriteLine("Name: " + customers[i].Name);
            //    Console.WriteLine("Gender: " + customers[i].Gender);
            //    Console.WriteLine("Age: " + customers[i].Age);
            //    Console.WriteLine("Phone ID: " + customers[i].phone.Id);
            //    Console.WriteLine("Phone number: " + customers[i].phone.PhoneNumber);
            //    Console.WriteLine("Telecommunications company: " + customers[i].phone.Company);
            //    Console.WriteLine("-----------------------------------");
            //}




            proxy.Close();
        }
    }
}
