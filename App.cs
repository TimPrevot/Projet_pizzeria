using System;
using System.Xml.Schema;

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
    }
}