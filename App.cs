using System;
using System.Data;
using System.Xml.Schema;
using Npgsql;

namespace Projet_pizzeria
{
    public class App
    {
        public User actualClient = new User();
        public void createOrder()
        {
            string postgresConStr =
                "Server=localhost;Port=5432;UserId=postgres;Password=abcd1234;Database=Projet_pizzeria;";
            NpgsqlConnection ncon = new NpgsqlConnection(postgresConStr);
            DateTime orderDate = DateTime.Now;
            Console.WriteLine("Please enter the client's phone number");
            string telClient = Console.ReadLine();
            NpgsqlCommand cmd = new NpgsqlCommand("SELECT user FROM users WHERE user.telephone ===" + telClient);
            ncon.Open();
            NpgsqlDataReader dr = cmd.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("user_id", typeof(int)));
            dt.Columns.Add(new DataColumn("firstname", typeof(string)));
            dt.Columns.Add(new DataColumn("lastname", typeof(string)));
            dt.Columns.Add(new DataColumn("telephone", typeof(string)));
            dt.Columns.Add(new DataColumn("address", typeof(string)));
            dt.Columns.Add(new DataColumn("city", typeof(string)));
            dt.Columns.Add(new DataColumn("postal_code", typeof(string)));
            while (dr.Read())
            {
                DataRow row = dt.NewRow();
                row["user_id"] = dr["user_id"];
                row["firstname"] = dr["firstname"];
                row["lastname"] = dr["lastname"];
                row["telephone"] = dr["telephone"];
                row["address"] = dr["address"];
                row["city"] = dr["city"];
                row["postal_code"] = dr["postal_code"];
                dt.Rows.Add(row);
            }
            actualClient.user_id = dt["user_id"];
            Console.WriteLine("Created order");
        }
        
        public static void Main(string[] args)
        {
            string postgresConStr =
                "Server=localhost;Port=5432;UserId=postgres;Password=abcd1234;Database=Projet_pizzeria;";
            NpgsqlConnection ncon = new NpgsqlConnection(postgresConStr);
            NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM users", ncon);
            ncon.Open();
            NpgsqlDataReader dr = cmd.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("user_id", typeof(int)));
            dt.Columns.Add(new DataColumn("firstname", typeof(string)));
            dt.Columns.Add(new DataColumn("lastname", typeof(string)));
            dt.Columns.Add(new DataColumn("telephone", typeof(string)));
            dt.Columns.Add(new DataColumn("address", typeof(string)));
            dt.Columns.Add(new DataColumn("city", typeof(string)));
            dt.Columns.Add(new DataColumn("postal_code", typeof(string)));

            while (dr.Read())
            {
                DataRow row = dt.NewRow();
                row["user_id"] = dr["user_id"];
                row["firstname"] = dr["firstname"];
                row["lastname"] = dr["lastname"];
                row["telephone"] = dr["telephone"];
                row["address"] = dr["address"];
                row["city"] = dr["city"];
                row["postal_code"] = dr["postal_code"];
                dt.Rows.Add(row);
            }
            ncon.Close();
            dt.AcceptChanges();
            PrintTable(dt);
            Console.WriteLine("Hello World!");
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