using System;
using System.Data;
using System.Runtime.CompilerServices;
using System.Xml.Schema;
using Npgsql;

namespace Projet_pizzeria
{
    public class App
    {
        public App()
        {
            this.actualClient = new User();
        }

        public User actualClient;

        // Function to get the client from the db if this isn't his first order, or create him and add him in the db instead
        public void checkUser()
        {
            Console.WriteLine("Is this the client's first order ? Type 1 for Yes, 0 for No :");
            var isNew = Convert.ToInt32(Console.ReadLine());
            while (isNew != 0 && isNew != 1)
            {
                Console.WriteLine("Please enter a correct value.");
                isNew = Convert.ToInt32(Console.ReadLine());
            }

            // If this is not the client's first order, we get his info from the database
            if (isNew == 1)
            {
                const string postgresConStr =
                    "Server=localhost;Port=5432;UserId=postgres;Password=abcd1234;Database=Projet_pizzeria;";
                var ncon = new NpgsqlConnection(postgresConStr);
                Console.WriteLine("Please enter the client's phone number");
                var telClient = Console.ReadLine();
                var cmd = new NpgsqlCommand("SELECT * FROM users WHERE telephone = '" + telClient + "'", ncon);
                ncon.Open();
                var dr = cmd.ExecuteReader();

                var dt = new DataTable();
                dt.Columns.Add(new DataColumn("user_id", typeof(int)));
                dt.Columns.Add(new DataColumn("firstname", typeof(string)));
                dt.Columns.Add(new DataColumn("lastname", typeof(string)));
                dt.Columns.Add(new DataColumn("telephone", typeof(string)));
                dt.Columns.Add(new DataColumn("address", typeof(string)));
                dt.Columns.Add(new DataColumn("city", typeof(string)));
                dt.Columns.Add(new DataColumn("postal_code", typeof(string)));
                dt.Columns.Add(new DataColumn("entity_id", typeof(int)));
                while (dr.Read())
                {
                    var row = dt.NewRow();
                    row["user_id"] = dr["user_id"];
                    row["firstname"] = dr["firstname"];
                    row["lastname"] = dr["lastname"];
                    row["telephone"] = dr["telephone"];
                    row["address"] = dr["address"];
                    row["city"] = dr["city"];
                    row["postal_code"] = dr["postal_code"];
                    row["entity_id"] = dr["entity_id"];
                    dt.Rows.Add(row);
                    this.actualClient.UserId = Convert.ToInt32(dr["user_id"].ToString());
                    this.actualClient.FirstName = dr["firstname"].ToString();
                    this.actualClient.LastName = dr["lastname"].ToString();
                    this.actualClient.Tel = dr["telephone"].ToString();
                    this.actualClient.Address = dr["address"].ToString();
                    this.actualClient.City = dr["city"].ToString();
                    this.actualClient.PostalCode = dr["postal_code"].ToString();
                    this.actualClient.Entity = Convert.ToInt32(dr["entity_id"].ToString());
                }

                ncon.Close();
                dt.AcceptChanges();
                Console.WriteLine("Client connected: " + this.actualClient.FirstName + " " +
                                  this.actualClient.LastName + ", tel: " + this.actualClient.Tel);
            }
            else if (isNew == 0)
            {
                Console.WriteLine("Please enter the client's first name: ");
                var firstname = Console.ReadLine();
                Console.WriteLine("Please enter the client's last name: ");
                var lastname = Console.ReadLine();
                Console.WriteLine("Please enter the client's phone number with the indicative: ");
                var tel = Console.ReadLine();
                Console.WriteLine("Please enter the client's address: ");
                var address = Console.ReadLine();
                Console.WriteLine("Please enter the client's city: ");
                var city = Console.ReadLine();
                Console.WriteLine("Please enter the client's postal code: ");
                var postalCode = Console.ReadLine();
                var entity = 2;

                const string postgreConStr =
                    "Server=localhost;Port=5432;UserId=postgres;Password=abcd1234;Database=Projet_pizzeria;";
                var ncon = new NpgsqlConnection(postgreConStr);
                var req = "INSERT INTO users VALUES (default, '" + firstname + "', '" + lastname + "', '" + tel + "', '" + address +
                          "', '" + city + "', '" + postalCode + "', '" + entity + "')";
                Console.WriteLine("req: " + req);
                var cmd = new NpgsqlCommand(req, ncon);
                ncon.Open();
                cmd.ExecuteNonQuery();
                Console.WriteLine("Client added !");
            }
        }

        public void createOrder()
        {
            Console.WriteLine("Created order");
        }

        public static void Main(string[] args)
        {
            var myApp = new App();
            Console.WriteLine("To create a new order, type 1");
            var choice = Convert.ToInt32(Console.ReadLine());
            while (choice != 1)
            {
                Console.WriteLine("Please enter a correct value.");
                choice = Convert.ToInt32(Console.ReadLine());
            }

            if (choice == 1)
            {
                myApp.checkUser();
                myApp.createOrder();
            }
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
    }
}