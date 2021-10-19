using System;
using System.Data;
using System.Runtime.CompilerServices;
using System.Xml.Schema;
using Npgsql;

namespace Projet_pizzeria
{
    public class App
    {
        public void createOrder()
        {
            
        }
        
        public static void Main(string[] args)
        {
            string postgresConStr =
                "Server=localhost;Port=5432;UserId=postgres;Password=abcd1234;Database=Projet_pizzeria;";
            
            Console.WriteLine("Hello World!");
        }

        public void afficherMenu(){
            const string postgresConStr =
                "Server=localhost;Port=5432;UserId=postgres;Password=abcd1234;Database=Projet_pizzeria;";
            var dt = new DataTable();
            var ncon = new NpgsqlConnection(postgresConStr);
            var cmd = new NpgsqlCommand("SELECT * FROM menu ", ncon);
            ncon.Open();
            var dr = cmd.ExecuteReader();

            dt.Columns.Add(new DataColumn("product_id", typeof(int)));
            dt.Columns.Add(new DataColumn("product_type", typeof(int)));
            dt.Columns.Add(new DataColumn("price", typeof(int)));
            dt.Columns.Add(new DataColumn("name", typeof(string)));
            dt.Columns.Add(new DataColumn("size", typeof(int)));
            dt.Columns.Add(new DataColumn("available", typeof(bool)));

            while (dr.Read())
            {
                var row = dt.NewRow();
                row["product_id"] = dr["product_id"];
                row["product_type"] = dr["product_type"];
                row["price"] = dr["price"];
                row["name"] = dr["name"];
                row["size"] = dr["size"];
                row["available"] = dr["available"];
                dt.Rows.Add(row);
                this.actualClient.UserId = Convert.ToInt32(dr["product_id"].ToString());
                this.actualClient.FirstName = dr["product_type"].ToString();
                this.actualClient.LastName = dr["price"].ToString();
                this.actualClient.Tel = dr["name"].ToString();
                this.actualClient.Address = dr["size"].ToString();
                this.actualClient.City = dr["available"].ToString();
            }

            ncon.Close();
            PrintTable(dt);
        }

        static void PrintTable(DataTable dt)
        {
            foreach (DataColumn dc in dt.Columns)
            {
                Console.Write(dc.ColumnName + "   ");
            }

            Console.WriteLine();
            Console.WriteLine();
            foreach (DataRow dr in dt.Rows)
            {
                for (int i = 0; i < dr.ItemArray.Length; i++)
                {
                    Console.Write(dr[i] + "\t");
                }

                Console.WriteLine();
                Console.WriteLine();
            }
        }

        public void CheckInputNewUser(){

            Console.WriteLine("Please enter the user's first name: ");
            var firstname = Console.ReadLine();
            while(firstname.GetType() != typeof(string)){
                Console.WriteLine("your first name need to be a string !");
                firstname = Console.ReadLine();
            }

            Console.WriteLine("Please enter the user's last name: ");
            var lastname = Console.ReadLine();
            while(lastname.GetType() != typeof(string)){
                Console.WriteLine("your last name need to be a string !");
                lastname = Console.ReadLine();
            }

            Console.WriteLine("Please enter the user's phone number with the indicative: ");
            var tel = Console.ReadLine();
            while(tel.GetType() != typeof(int)){
                Console.WriteLine("your tel need to be a string !");
                tel = Console.ReadLine();
            }

            Console.WriteLine("Please enter the user's address: ");
            var address = Console.ReadLine();
            while(address.GetType() != typeof(string)){
                Console.WriteLine("your address need to be a string !");
                address = Console.ReadLine();
            }

            Console.WriteLine("Please enter the user's city: ");
            var city = Console.ReadLine();
            while(city.GetType() != typeof(string)){
                Console.WriteLine("your city need to be a string !");
                city = Console.ReadLine();
            }

            Console.WriteLine("Please enter the user's postal code: ");
            var postalCode = Console.ReadLine();
            while(postalCode.GetType() != typeof(int)){
                Console.WriteLine("your postalCode need to be a string !");
                postalCode = Console.ReadLine();
            }
        }

        public void CheckUserConnected(){
            //
            else if (isNew == 0)
            {
                this.addNewUser();
                // TODO put the new user's info in the actualClient variable
                Console.WriteLine("Authentication successful.");
                ActualClient.UserId = user_id;
                ActualClient.FirstName = firstname;
                ActualClient.LastName = lastname;
                ActualClient.Tel = telephone;
                ActualClient.Username = username;
                ActualClient.Address = address;
                ActualClient.City = city;
                ActualClient.PostalCode = postalCode;
                    
                Console.WriteLine("actuel client connected : " + ActualClient.FirstName + " " +
                                    ActualClient.LastName + ", tel: " + ActualClient.Tel);
            }
        }

        public void MessageClient(){
            Console.WriteLine("Your order has been processed");
        }

        public void Messagelivreur(){
            Console.WriteLine("actuel client delivery : " + ActualClient.FirstName + " " +
                            ActualClient.LastName + ", tel: " + ActualClient.Tel + ", address :" + ActualClient.Address + ", city :" + ActualClient.city);
        }

        public void MessageClerck(){
            Console.WriteLine("Order opening confirmed");
        }
    }
}